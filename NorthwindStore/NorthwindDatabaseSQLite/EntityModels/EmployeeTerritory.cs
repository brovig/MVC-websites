using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDatabase
{
    [Keyless]
    public partial class EmployeeTerritory
    {
        [Column("EmployeeID", TypeName = "int")]
        public int EmployeeId { get; set; }
        [Required]
        [Column("TerritoryID", TypeName = "nvarchar")]
        public string TerritoryId { get; set; } = null!;
    }
}
