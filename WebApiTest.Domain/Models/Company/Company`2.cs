using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest.Domain.Models
{
    public partial class Company
    {
        public Company()
        { 
        
        }

        public Company(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

        public void Update(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }
}
