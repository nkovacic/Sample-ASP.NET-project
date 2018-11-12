using Sample.Core.Data.Database;
using Sample.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Services.Base
{
    public class BaseService
    {
        public IUnitOfWorkAsync UnitOfWork { get; set; }

        public ILogger Logger { get; set; }
    }
}
