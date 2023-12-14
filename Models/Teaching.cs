using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Teaching
    {
        public int? TeachingId { get; set; }
        public int FkProffesorId { get; set; }
        public int FkClassId { get; set; }

        public virtual Class FkClass { get; set; } = null!;
        public virtual Proffesor FkProffesor { get; set; } = null!;
    }
}
