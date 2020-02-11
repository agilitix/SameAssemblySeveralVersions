using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApp
{
    /// <summary>
    /// This class can load and return several versions of log4net when requested by the AppDomain resolver
    /// (mixing 1.2.10 and newer versions is allowed).
    /// </summary>
    public class Log4netResolver : IDisposable
    {
        private readonly IDictionary<string /*full name*/, Assembly> _log4netAssemblies = new Dictionary<string, Assembly>();

        public Log4netResolver(string[] log4netAssemblies = null)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolver;

            LoadLog4netAssemblies(log4netAssemblies ?? GetLog4netAssemblies());
        }

        public void Configure(string configFile)
        {
            foreach (Assembly log4netAssembly in _log4netAssemblies.Values)
            {
                // Setup log4net with given config file:
                // XmlConfigurator.Configure(new FileInfo(configFile));
                Type xmlConfigurator = log4netAssembly.GetType("log4net.Config.XmlConfigurator");
                MethodInfo configureMethod = xmlConfigurator.GetMethod("Configure", new[] {typeof(FileInfo)});
                configureMethod?.Invoke(null, new object[] {new FileInfo(configFile)});
            }
        }

        public Version[] GetVersions()
        {
            return _log4netAssemblies.Select(x => x.Value.GetName().Version).ToArray();
        }

        public AssemblyName[] GetNames()
        {
            return _log4netAssemblies.Select(x => x.Value.GetName()).ToArray();
        }

        private void LoadLog4netAssemblies(string[] log4netAssemblies)
        {
            foreach (string file in log4netAssemblies)
            {
                // Load log4net assembly inside current domain.
                Assembly log4netAssembly = Assembly.LoadFile(file);
                _log4netAssemblies[log4netAssembly.FullName] = log4netAssembly;
            }
        }

        /// <summary>
        /// Find log4net assemblies recursively from executable folder, the files are filtered by version.
        /// </summary>
        private string[] GetLog4netAssemblies()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] log4netAssemblies = Directory.GetFiles(path, "log4net.dll", SearchOption.AllDirectories)
                                                  .GroupBy(x => FileVersionInfo.GetVersionInfo(x).FileVersion)
                                                  .Select(x => x.FirstOrDefault())
                                                  .ToArray();
            return log4netAssemblies;
        }

        private Assembly CurrentDomainAssemblyResolver(object sender, ResolveEventArgs args)
        {
            Assembly resolved;

            if (!_log4netAssemblies.TryGetValue(args.Name, out resolved))
            {
                _log4netAssemblies.TryGetValue(args.RequestingAssembly?.FullName ?? "", out resolved);
            }

            return resolved;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolver;
        }
    }
}
