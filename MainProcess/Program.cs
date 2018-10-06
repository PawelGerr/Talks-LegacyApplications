using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
   class Program
   {
      static async Task Main()
      {
         Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

         Console.CancelKeyPress += (sender, args) => Environment.Exit(0);

         var processPool = new ChildProcessPool();
         var randomClient = new RandomClient(processPool);

         while (true)
         {
            var value = await randomClient.NextAsync();

            Console.WriteLine($"[{DateTime.Now}] Next random number is {value}");
            Console.ReadLine();
         }
      }
   }
}
