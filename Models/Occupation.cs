using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Occupation
    {
        public int? Occid { get; set; }
        public int FkProffesorId { get; set; }
        public int FkProffesionId { get; set; }

        public virtual Proffesion FkProffesion { get; set; } = null!;
        public virtual Proffesor FkProffesor { get; set; } = null!;
    }
}
