using ContactManagement.Core.Dtos;
using ContactManagement.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Abstractions
{
    public interface IContactCategoryRepository
    {
        void Save(ContactCategoryDto category);
        Task<List<ContactCategoryViewModel>> GetAll();
        Task<ContactCategoryViewModel> GetById(int id);
        void Update(ContactCategoryDto category);
        void Delete(ContactCategoryDto category);

    }
}
