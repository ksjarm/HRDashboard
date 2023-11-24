using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Model
{
     [Table("user")]
        public class User
        {
            [Column("id")]
            public int Id { get; set; }
            [Column("name")]
            public string Name {get;set;} = "";
            [Column("surname")]
            public string Surname {get;set;} = "";
            [Column("phonenumber")]
            public string? PhoneNumber {get;set;}
            [Column("adress")]
            public string Adress {get;set;} = "";
            [Column("username")]
            public string Username { get; set; } = "";
            [Column("password")]
            public string Password { get; set; } = "";
            [Column("role")]
            public string Role { get; set; } = "";

        }

}