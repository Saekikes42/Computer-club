using Microsoft.EntityFrameworkCore;
namespace designclub
{
    public static class DatabaseConfiguration
    {
        public static string GetConnectionString() =>
            Environment.GetEnvironmentVariable("COMPUTERCLUB_DB")
            ?? "Server=localhost;User=root;Password=;Database=ComputerClub";
        public static MySqlServerVersion ServerVersion { get; } =
            new(new Version(8, 0, 40));
    }
}