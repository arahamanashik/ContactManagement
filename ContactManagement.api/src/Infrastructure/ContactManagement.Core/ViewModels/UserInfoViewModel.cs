﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.ViewModels
{
    public class UserInfoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PasswordHashed { get; set; }
        public string PasswordSalt { get; set; }
        public string ProviderName { get; set; }
        public string Token { get; set; }
    }
}
