using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Domain.Models
{
    public partial class Computer
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? DepartmentId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime CreateTime { get; set; }
    }
}
