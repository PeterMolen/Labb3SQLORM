using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string? ClassInfo { get; set; }
    }
}
