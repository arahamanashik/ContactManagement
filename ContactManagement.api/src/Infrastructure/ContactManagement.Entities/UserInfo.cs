using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PasswordHashed { get; set; }
        public string PasswordSalt { get; set; }
        public string ProviderName { get; set; }

        public ICollection<ContactInfo> ContactInfoes { get; set; }
           = new HashSet<ContactInfo>();
        public ICollection<ContactCategory> ContactCategories { get; set; }
           = new HashSet<ContactCategory>();

    }
}
