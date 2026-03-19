using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs.Position
{
    public class UpdatePositionDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
        public decimal? Allowance { get; set; }
        public int? Level { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}