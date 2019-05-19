using ContactManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.ViewModels
{
    public class ContactInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public int ContactCategoryId { get; set; }
        public Guid UserInfoId { get; set; }

        public ContactCategory ContactCategory { get; set; }
    }
}
