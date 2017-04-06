using Microsoft.Owin.Hosting;

namespace IdSrv.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // hosting identityserver
            using (WebApp.Start<Startup>("http://localhost:5000"))
            {
                System.Console.WriteLine("server running...");
                System.Console.ReadLine();
            }
        }
    }
}
