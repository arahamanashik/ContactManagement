using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.Dtos
{
    public class ContactInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }

        public IFormFile ProfilePicture { get; set; }
        public int ContactCategoryId { get; set; }

    }
}
