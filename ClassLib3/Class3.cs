﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace ClassLib3
{
    public class Class3
    {
        protected static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Class3()
        {
            _logger.Info(nameof(Class3) + " " + _logger.GetType().Assembly.FullName);
        }
    }
}
