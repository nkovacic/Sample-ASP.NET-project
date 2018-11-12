using Sample.Core.Expressions;
using Sample.Core.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Core.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public FileSizeAttribute()
        {
            _maxFileSize = (int)Math.Pow(10, 6) * 6;
        }
        public FileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueType = value.GetType();

            if (valueType == typeof(FileModel))
            {
                var fileModel = (FileModel)value;

                return CheckFileModelSize(fileModel);
            }
            else if (ExpressionsHelper.IsPropertyNonStringEnumerable(valueType) && ExpressionsHelper.GetElementType(valueType) == typeof(FileModel))
            {
                var fileModels = value as IEnumerable<FileModel>;

                if (fileModels != null)
                {
                    foreach (var fileModel in fileModels)
                    {
                        if (!CheckFileModelSize(fileModel))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                throw new ArgumentException("FileModels don't have any values.");
            }
            else
            {
                throw new ArgumentException("Property hast to hava a FileModel or IEnumerable<FileModel> type.");
            }
        }

        private bool CheckFileModelSize(FileModel fileModel)
        {
            return fileModel.ContentLength <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage("File " + name + " was too large. Maximum size is " + Math.Round((double)_maxFileSize / 1000, 3).ToString() + " KB.");
        }
    }
}