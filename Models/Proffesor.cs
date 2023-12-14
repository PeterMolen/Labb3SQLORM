using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Proffesor
    {
        public int ProffesorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
    }
}
