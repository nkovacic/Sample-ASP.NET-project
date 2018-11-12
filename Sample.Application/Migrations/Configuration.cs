namespace Sample.Application.Migrations
{
    using Sample.Application.Models;
    using Sample.Core.Data.Database;
    using Sample.Core.Data.Database.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Linq.Expressions;

    internal sealed class Configuration : DbMigrationsConfiguration<Sample.Core.Data.Database.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Sample.Core.Data.Database.DataContext context)
        {
            var dictionaries = new List<Dictionary>()
            {
                new Dictionary
                {

                }
            };
        }


        private void InsertOrUpdateEntities<TEntity>(Expression<Func<TEntity, object>> uniqueAttributeExpresion,
            IEnumerable<TEntity> entities, DataContext context, bool saveChanges = true) where TEntity : Entity
        {
            foreach (var entity in entities)
            {

                //SyncObjectGraph(entity, context);
            }

            context.Set<TEntity>().AddOrUpdate(uniqueAttributeExpresion, entities.ToArray());

            if (saveChanges)
            {
                context.SaveChanges();
                //context.SyncObjectsStatePostCommit();
            }

        }

        private HashSet<object> _entitesChecked;

        private void SyncObjectGraph(object entity, Sample.Core.Data.Database.DataContext context) // scan object graph for all 
        {
            // instantiating _entitesChecked so we can keep track of all entities we have scanned, avoid any cyclical issues
            if (_entitesChecked == null)
                _entitesChecked = new HashSet<object>();

            // if already processed skip
            if (_entitesChecked.Contains(entity))
                return;

            // add entity to alreadyChecked collection
            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            // discovered entity with ObjectState.Added, sync this with provider e.g. EF
            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                context.SyncObjectState((IObjectState)entity);

            // Set tracking state for child collections
            foreach (var prop in entity.GetType().GetProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                var trackableRef = prop.GetValue(entity, null) as IObjectState;
                if (trackableRef != null)
                {
                    // discovered entity with ObjectState.Added, sync this with provider e.g. EF
                    if (trackableRef.ObjectState == ObjectState.Added)
                        context.SyncObjectState((IObjectState)entity);

                    // recursively process the next property
                    SyncObjectGraph(prop.GetValue(entity, null), context);
                }

                // Apply changes to 1-M properties
                var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;

                // collection was empty, nothing to process, continue
                if (items == null) continue;

                // collection isn't empty, continue to recursively scan the elements of this collection
                foreach (var item in items)
                    SyncObjectGraph(item, context);
            }
        }
    }
}
