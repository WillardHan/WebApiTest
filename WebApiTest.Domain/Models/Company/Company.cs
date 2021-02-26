using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Domain.Models
{
    public partial class Company
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime CreateTime { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
