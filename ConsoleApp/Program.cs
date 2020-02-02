using System;
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
            // This executable path.
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Pre-load and resolve several log4net versions. The config file must be compatible with all versions.
            Log4netAssemblyResolver log4netResolver = new Log4netAssemblyResolver(new[]
                                                                                  {
                                                                                      Path.Combine(path, @"log4net1210\log4net.dll"),
                                                                                      Path.Combine(path, @"log4net204\log4net.dll"),
                                                                                      Path.Combine(path, @"log4net208\log4net.dll")
                                                                                  },
                                                                                  "log4net.config");

            Class1 cl1 = new Class1();
            Class2 cl2 = new Class2();
            Class3 cl3 = new Class3();

            Console.ReadLine();

            log4netResolver.Dispose();
        }
    }
}
