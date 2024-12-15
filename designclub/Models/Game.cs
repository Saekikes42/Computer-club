using System;
using System.Collections.Generic;

namespace designclub.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Photo { get; set; }

    public string? Description { get; set; }

    public string ExecutableFile { get; set; } = null!;
}
