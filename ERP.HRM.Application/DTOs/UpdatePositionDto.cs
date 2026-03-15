using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs
{
    public class UpdatePositionDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;
        public string? Description { get; set; }
    }
}