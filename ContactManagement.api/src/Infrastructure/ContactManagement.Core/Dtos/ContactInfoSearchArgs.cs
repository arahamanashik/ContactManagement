using DaffodilSoftware.Pagination.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactManagement.Core.Dtos
{
    public class ContactInfoSearchArgs : ResourceQueryParameters
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
