using System;
using System.Collections.Generic;

namespace WebApiTest.Rpc.DTOs
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public List<DepartmentDTO> Departments { get; set; } = new List<DepartmentDTO>();
    }

    public class DepartmentDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
