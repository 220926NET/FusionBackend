﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class UserDTO
    {
        public Guid userId { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? leetCodeName { get; set; }
        public int? problemsCompleted { get; set; }
        public UserDTO() { }
        public UserDTO(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
