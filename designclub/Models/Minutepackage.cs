using System;
using System.Collections.Generic;

namespace designclub.Models;

public partial class Minutepackage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Minutes { get; set; }

    public decimal Cost { get; set; }

    public bool IsArchived { get; set; }

    public string? Type { get; set; }



    public virtual ICollection<Providedminutepackage> Providedminutepackages { get; set; } = new List<Providedminutepackage>();
}
