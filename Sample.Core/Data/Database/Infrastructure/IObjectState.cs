
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.Core.Data.Database
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}