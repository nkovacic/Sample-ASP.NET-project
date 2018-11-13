using Sample.Application.ViewModels;
using Sample.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Services
{
    public class TextUploadService : BaseService
    {
        public async Task<ServiceResult<TextUploadResultViewModel>> ReverseLinesFromUpload(TextUploadViewModel textUpload)
        {
            var serviceResult = new ServiceResult<TextUploadResultViewModel>();

            using (var streamReader = new StreamReader(textUpload.File.Content))
            {
                var lines = new List<string>();
                string currentLine = null;

                while ((currentLine = await streamReader.ReadLineAsync()) != null)
                {
                    lines.Add(currentLine);
                }

                lines.Reverse();

                serviceResult.Data = new TextUploadResultViewModel
                {
                    ReversedLines = lines
                };
            }

            return serviceResult;
        }
    }
}
