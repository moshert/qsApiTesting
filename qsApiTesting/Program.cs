using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qlik.Engine;

namespace qsApiTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:4848";
            ILocation location = getQsConnection(uri);

            Console.Write("Qlik Version: ");
            printQlikSenseDesktopVersionNumber(location);
            listApps(location);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.ReadKey();
            }
        }

        private static ILocation getQsConnection(string uri)
        {
            Uri qsUri = new Uri(uri);
            ILocation location = Location.FromUri(qsUri);

            return location;
        }

        private static void printQlikSenseDesktopVersionNumber(ILocation loc)
        {
            ILocation location = loc;

            // Defining the location as a direct connection to Qlik Sense Personal
            location.AsDirectConnectionToPersonalEdition();

            using (IHub hub = location.Hub())
            {
                Console.WriteLine(hub.ProductVersion());
            }
        }

        private static void listApps(ILocation loc)
        {
            ILocation location = loc;

            foreach (var appIdentifier in location.GetAppIdentifiers())
            {
                try
                {
                    using (var app = location.App(appIdentifier))// Location specified in Accessing
                    {
                        var layout = app.GetAppLayout();
                        Console.Write(layout.Title + " [" + app.Type + "]");
                    }
                }
                catch (MethodInvocationException e)
                {
                    Console.WriteLine("Could not open app: " + appIdentifier.AppName + Environment.NewLine + e.InvocationError.Message);
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine("Timeout for: " + appIdentifier.AppName + Environment.NewLine + e.Message);
                }
            }
        }


    }
}
