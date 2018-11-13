using Sample.Core.Data.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Models
{
    [Table("Dictionaries")]
    public class Dictionary: Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
