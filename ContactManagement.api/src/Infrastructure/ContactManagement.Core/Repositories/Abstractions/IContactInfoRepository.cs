using ContactManagement.Core.Dtos;
using ContactManagement.Core.ViewModels;
using DaffodilSoftware.Pagination.Sql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Abstractions
{
    public interface IContactInfoRepository
    {
        void Save(ContactInfoDto category);
        Task<List<ContactInfoViewModel>> GetAll(ContactInfoSearchArgs args);
        Task<ContactInfoViewModel> GetById(int id);
        void Update(ContactInfoDto category);
        void Delete(ContactInfoDto category);

    }
}
