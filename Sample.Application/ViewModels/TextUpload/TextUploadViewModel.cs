using Sample.Core.Files;
using Sample.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.ViewModels
{
    public class TextUploadViewModel
    {
        [Required]
        [FileSize(5 * 1000 * 1000)]
        public FileModel File { get; set; }
    }
}
