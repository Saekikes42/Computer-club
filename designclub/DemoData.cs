using designclub.Models;

namespace designclub;

public static class DemoData
{
    public static Client Client { get; } = new()
    {
        Id = 1,
        FullName = "Демо-клиент",
        Phone = "+7 900 000-00-00",
        Email = "demo@example.com",
        BirthDate = new DateOnly(2000, 1, 1),
        BonusBalance = 1250,
        UserClient = "demo",
        Pass = string.Empty,
        RoleClient = "client"
    };

    public static List<Client> Clients { get; } =
    [
        Client,
        new Client
        {
            Id = 2,
            FullName = "Иван Петров",
            Phone = "+7 900 111-22-33",
            Email = "ivan@example.com",
            BirthDate = new DateOnly(2001, 5, 12),
            BonusBalance = 640,
            UserClient = "ivan",
            Pass = string.Empty,
            RoleClient = "client"
        }
    ];

    public static List<Minutepackage> Services { get; } =
    [
        new Minutepackage
        {
            Id = 1,
            Name = "Игровой час",
            Description = "60 минут за игровым компьютером",
            Minutes = 60,
            Cost = 250,
            Type = "Игровое время"
        },
        new Minutepackage
        {
            Id = 2,
            Name = "Ночной пакет",
            Description = "Пакет игрового времени на всю ночь",
            Minutes = 480,
            Cost = 1200,
            Type = "Пакет"
        }
    ];

    public static List<Providedminutepackage> ProvidedServices { get; } =
    [
        new Providedminutepackage
        {
            Id = 1,
            ClientId = Client.Id,
            PackageId = Services[0].Id,
            Date = DateTime.Now,
            Cost = Services[0].Cost,
            Status = "Завершено",
            ServiceType = Services[0].Type,
            Client = Client,
            Package = Services[0]
        }
    ];
}
