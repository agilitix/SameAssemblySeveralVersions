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

            // Create some classes doing logs, each one depends on a particular version of log4net.
            Class1 cl1 = new Class1();
            Class2 cl2 = new Class2();
            Class3 cl3 = new Class3();

            Console.ReadLine();

            log4netResolver.Dispose();
        }
    }
}
