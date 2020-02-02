using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleApp
{
    /// <summary>
    /// This class can load and return several versions of log4net (mixing 1.2.10 and newer versions is allowed)
    /// when requested by the AppDomain resolver.
    /// </summary>
    public class Log4netAssemblyResolver : IDisposable
    {
        private readonly IDictionary<string, Assembly> _log4netAssemblies = new Dictionary<string, Assembly>();

        public Log4netAssemblyResolver(string[] log4netAssemblies, string configFile)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolver;

            foreach (string file in log4netAssemblies)
            {
                Assembly log4netAssembly = Assembly.LoadFile(file);
                _log4netAssemblies[log4netAssembly.FullName] = log4netAssembly;

                Type xmlConfigurator = log4netAssembly.GetType("log4net.Config.XmlConfigurator");
                MethodInfo configureMethod = xmlConfigurator.GetMethod("Configure", new[] {typeof(FileInfo)});
                configureMethod?.Invoke(null, new object[] {new FileInfo(configFile)});
            }
        }

        private Assembly CurrentDomainAssemblyResolver(object sender, ResolveEventArgs args)
        {
            string log4netFullName = args.RequestingAssembly?.FullName ?? args.Name;

            Assembly resolved;
            if (_log4netAssemblies.TryGetValue(log4netFullName, out resolved))
            {
                return resolved;
            }

            return null;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolver;
        }
    }
}
