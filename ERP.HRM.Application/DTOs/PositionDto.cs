using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.DTOs
{
    public class PositionDto
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
        public decimal? Allowance { get; set; }
        public string? Status { get; set; }
        public int? Level { get; set; }
        public string? Description { get; set; }

        // Thông tin tổng hợp
        public int EmployeeCount { get; set; }
    }
}