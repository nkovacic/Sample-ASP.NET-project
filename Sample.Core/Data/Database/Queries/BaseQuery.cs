using Sample.Core.Data.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Data.Database
{
    public class BaseQuery<TEntity> : QueryObject<TEntity> where TEntity : Entity
    {
        public BaseQuery<TEntity> FilterById(Guid? id)
        {
            if (id.HasValue)
            {
                And(entity => entity.Id == id);
            }

            return this;
        }

        public BaseQuery<TEntity> FilterByIds(params Guid[] ids)
        {
            if (ids != null && ids.Any())
            {
                if (ids.Length == 1)
                {
                    var firstId = ids[0];

                    And(entity => entity.Id == firstId);
                }
                else
                {
                    And(entity => ids.Contains(entity.Id));
                }
            }

            return this;
        }

        public BaseQuery<TEntity> FilterByStartDate(DateTimeOffset? startDate)
        {
            if (startDate.HasValue)
            {
                And(entity => entity.CreatedAt > startDate.Value);
            }

            return this;
        }

        public BaseQuery<TEntity> FilterByEndDate(DateTimeOffset? endDate)
        {
            if (endDate.HasValue)
            {
                And(entity => entity.CreatedAt > endDate.Value);
            }

            return this;
        }

        public BaseQuery<TEntity> WithoutIds(params Guid[] ids)
        {
            if (ids != null && ids.Any())
            {
                if (ids.Length == 1)
                {
                    var firstId = ids[0];

                    And(entity => entity.Id != firstId);
                }
                else
                {
                    And(entity => !ids.Contains(entity.Id));
                }
            }

            return this;
        }
    }
}
