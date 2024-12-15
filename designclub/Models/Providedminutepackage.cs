using System;
using System.Collections.Generic;

namespace designclub.Models;

public partial class Providedminutepackage
{
    public int Id { get; set; }

    public int PackageId { get; set; }

    public int? ClientId { get; set; }

    public DateTime Date { get; set; }

    public decimal Cost { get; set; }

    public string? Status { get; set; } // Nullable string, если статус может быть NULL

    public string? ServiceType { get; set; } // Nullable строка, если ServiceType может быть NULL

    public virtual Client? Client { get; set; }

    public virtual Minutepackage Package { get; set; } = null!;
}

