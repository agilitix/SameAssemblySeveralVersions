using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleApp
{
    /// <summary>
    /// This class can load and return several versions of log4net when requested by the AppDomain resolver
    /// (mixing 1.2.10 and newer versions is allowed).
    /// </summary>
    public class Log4netAssemblyResolver : IDisposable
    {
        private readonly IDictionary<string, Assembly> _log4netAssemblies = new Dictionary<string, Assembly>();

        public Log4netAssemblyResolver(string[] log4netAssemblies, string configFile)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolver;

            foreach (string file in log4netAssemblies)
            {
                // Load log4net assembly inside current domain.
                Assembly log4netAssembly = Assembly.LoadFile(file);
                _log4netAssemblies[log4netAssembly.FullName] = log4netAssembly;

                // Setup log4net with given config file:
                // XmlConfigurator.Configure(new FileInfo(configFile));
                Type xmlConfigurator = log4netAssembly.GetType("log4net.Config.XmlConfigurator");
                MethodInfo configureMethod = xmlConfigurator.GetMethod("Configure", new[] {typeof(FileInfo)});
                configureMethod?.Invoke(null, new object[] {new FileInfo(configFile)});
            }
        }

        private Assembly CurrentDomainAssemblyResolver(object sender, ResolveEventArgs args)
        {
            Assembly resolved;

            if (_log4netAssemblies.TryGetValue(args.Name, out resolved))
            {
                return resolved;
            }

            if (!string.IsNullOrEmpty(args.RequestingAssembly?.FullName))
            {
                _log4netAssemblies.TryGetValue(args.RequestingAssembly?.FullName, out resolved);
            }

            return resolved;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolver;
        }
    }
}
