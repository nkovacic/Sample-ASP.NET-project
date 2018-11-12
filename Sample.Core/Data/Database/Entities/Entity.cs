using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sample.Core.Data.Database.Entities
{
    public abstract class Entity : IObjectState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAt { get; internal set; }

        [JsonIgnore]
        public DateTimeOffset UpdatedAt { get; internal set; }

        [JsonIgnore]
        [NotMapped]
        public ObjectState ObjectState { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = CreatedAt;
            ObjectState = ObjectState.Unchanged;
        }

        public bool IsCreatedState()
        {
            return ObjectState == ObjectState.Added;
        }

        public bool IsChangedState()
        {
            return ObjectState == ObjectState.Modified;
        }

        public bool IsNotDeletedState()
        {
            return ObjectState != ObjectState.Deleted;
        }

        public virtual void StateChanged()
        {
            ObjectState = ObjectState.Modified;
        }

        public virtual void StateCreated()
        {
            ObjectState = ObjectState.Added;
        }

        public virtual void StateDeleted()
        {
            ObjectState = ObjectState.Deleted;
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, SampleCoreModule.JsonSettings);
        }

        public virtual JObject ToJObject()
        {
            return JObject.Parse(ToJson());
        }
    }
}