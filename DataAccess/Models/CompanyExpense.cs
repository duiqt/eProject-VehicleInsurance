using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class CompanyExpense
{
    public int Id { get; set; }

    public string DateOfExpense { get; set; } = null!;

    public string TypeOfExpense { get; set; } = null!;

    public decimal AmountOfExpense { get; set; }
}
