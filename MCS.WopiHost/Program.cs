// Copyright 2014 The Authors Marx-Yu. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WopiCobaltHost
{
    class Program
    {
        static void Main()
        {
            string docsPath = ConfigurationManager.AppSettings.Get("DocsPath");
            string hostName = ConfigurationManager.AppSettings.Get("HostName");
            int port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port"));

            CobaltServer svr = new CobaltServer(docsPath, hostName, port);
            svr.Start();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine("**Don't close this window**");
            Console.ResetColor();

            Console.WriteLine("DocsPath: " + docsPath);
            Console.WriteLine("HostName: " + hostName);
            Console.WriteLine("Port: " + port);

            Console.WriteLine("Press (x) to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.X)
                {
                    break;
                }
            }

            svr.Stop();
        }
    }
}
