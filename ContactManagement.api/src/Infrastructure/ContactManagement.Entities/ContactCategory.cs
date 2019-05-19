using ContactManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Entities
{
    public class ContactCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserInfoId { get; set; }

        public UserInfo UserInfo { get; set; }
        public ICollection<ContactInfo> ContactInfoes { get; set; }
           = new HashSet<ContactInfo>();
    }
}
