﻿using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace CronPluginService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string [] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "/gui")
                {
                    Application.Run(new ServiceControlForm());
                }
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                                        {
                                            new CronPluginService()
                                        };
                ServiceBase.Run(servicesToRun);
            }
        }

        public static void ConsoleThread()
        {
            PrintMenu();

            IServiceContext context = ServiceControllerFactory.Instance.GetScheduledJobController();

            ConsoleKey lastKey;
            do
            {
                lastKey = Console.ReadKey(true).Key;

                switch (lastKey)
                {
                    case ConsoleKey.S:
                        context.Start(null);
                        break;

                    case ConsoleKey.T:
                        context.Stop();
                        break;

                    case ConsoleKey.P:
                        context.Pause();
                        break;

                    case ConsoleKey.R:
                        context.Continue();
                        break;

                    case ConsoleKey.Q:
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                if (lastKey != ConsoleKey.Q)
                {
                    PrintMenu();
                }

            } while (lastKey != ConsoleKey.Q);
        }

        private static void PrintMenu()
        {
            Console.WriteLine("S\tStart Service");
            Console.WriteLine("T\tStop Service");
            Console.WriteLine("P\tPause Service");
            Console.WriteLine("R\tResume Service");
            Console.WriteLine("Q\tQuit");
        }
    }
}
