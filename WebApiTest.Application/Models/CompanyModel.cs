using System;
using System.Collections.Generic;

namespace WebApiTest.Application.Models
{
    public class CompanyRequest
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentRequest
    {
        public Guid? Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class CompanyModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public List<DepartmentModel> Departments { get; set; } = new List<DepartmentModel>(); 
    }

    public class DepartmentModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
