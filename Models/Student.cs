using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; } = null!;
        public string StudentLastName { get; set; } = null!;
        public string? Personal { get; set; }
    }
}
