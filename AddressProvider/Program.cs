using Microsoft.EntityFrameworkCore;
using AddressProvider.Models;
using Microsoft.AspNetCore.Hosting;

namespace AddressProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Startup.WebApp(args).Run();
        }
    }
}
