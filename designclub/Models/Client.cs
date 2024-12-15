using System;
using System.Collections.Generic;

namespace designclub.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly BirthDate { get; set; }

    public string? Gender { get; set; }

    public decimal BonusBalance { get; set; }

    public string UserClient { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string RoleClient { get; set; } = null!;

    public virtual ICollection<Providedminutepackage> Providedminutepackages { get; set; } = new List<Providedminutepackage>();
}
