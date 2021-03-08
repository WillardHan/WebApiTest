using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest2.Domain.Models
{
    public partial class User
    {
        public User()
        {

        }

        public User(string code, string name)
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
