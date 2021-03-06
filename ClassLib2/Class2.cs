﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace ClassLib2
{
    public class Class2
    {
        protected static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Class2()
        {
            _logger.Info(nameof(Class2) + " " + _logger.GetType().Assembly.FullName);
        }
    }
}
