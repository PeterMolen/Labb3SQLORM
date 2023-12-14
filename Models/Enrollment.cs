using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Enrollment
    {
        public int? EnrollmentId { get; set; }
        public int FkStudentId { get; set; }
        public int FkClassId { get; set; }

        public virtual Class FkClass { get; set; } = null!;
        public virtual Student FkStudent { get; set; } = null!;
    }
}
