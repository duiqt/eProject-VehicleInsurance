using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
