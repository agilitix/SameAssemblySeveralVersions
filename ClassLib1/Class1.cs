using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace ClassLib1
{
    public class Class1
    {
        protected static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Class1()
        {
            XmlConfigurator.Configure(new FileInfo("log4net1.config"));
            _logger.Info(nameof(Class1));
        }
    }
}
