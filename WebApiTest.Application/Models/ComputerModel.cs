using System;

namespace WebApiTest.Application.Models
{
    public class ComputerRequest
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class ComputerModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
