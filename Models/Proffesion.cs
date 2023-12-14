using System;
using System.Collections.Generic;

namespace Labb3SQLORM.Models
{
    public partial class Proffesion
    {
        public int ProffesionId { get; set; }
        public string Title { get; set; } = null!;
    }
}
