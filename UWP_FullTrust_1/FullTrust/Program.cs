using System;
using System.Diagnostics;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation;

namespace FullTrust
{
    class Program
    {

        static void Main(string[] args)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
               Windows.Storage.ApplicationData.Current.LocalSettings;
            Console.Title = "Hello World";
            String value = localSettings.Values["parameters"] as String;
            String command = localSettings.Values["command"] as String;
            String python = localSettings.Values["python"] as String;

            String cmd = command + " -n";


            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = python;
            start.Arguments = string.Format("{0} \"{1}\"", cmd, value);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process process = Process.Start(start);
        }
        
    }
}
