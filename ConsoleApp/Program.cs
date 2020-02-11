using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ClassLib1;
using ClassLib2;
using ClassLib3;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pre-load and resolve several log4net versions. The config file must be compatible
            // with all log4net assemblies found (recursively) in the executable directory.
            Log4netResolver log4netResolver = new Log4netResolver();
            log4netResolver.Configure("log4net.config");

            var versions = log4netResolver.GetVersions();
            var names = log4netResolver.GetNames();

            Console.WriteLine($"Loaded versions = {string.Join(", ", versions.Select(x => x.ToString()))}");
            Console.WriteLine($"Loaded code base = {string.Join("\n", names.Select(x => x.CodeBase))}");

            // Create some classes doing logs, each one depends on a particular version of log4net.
            Class1 cl1 = new Class1();
            Class2 cl2 = new Class2();
            Class3 cl3 = new Class3();

            Console.ReadLine();

            log4netResolver.Dispose();
        }
    }
}
