using designclub.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace designclub
{
    public partial class App : Application
    {
        public static ComputerClubContext context { get; set; } = null!;

        public static bool IsDemoMode { get; set; }

        public App()
        {
            context = new ComputerClubContext(new DbContextOptions<ComputerClubContext>());
        }
    }
}
