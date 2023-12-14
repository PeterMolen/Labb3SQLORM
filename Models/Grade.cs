using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Grade
    {
        public int GradesId { get; set; }
        public string? GradeSet { get; set; }
        public DateTime? GradeDateSet { get; set; }
        public string? GradesInfo { get; set; }
    }
}
