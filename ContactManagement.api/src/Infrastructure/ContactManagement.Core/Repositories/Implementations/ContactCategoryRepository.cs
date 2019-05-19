using AutoMapper.QueryableExtensions;
using ContactManagement.Core.Data;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using ContactManagement.Core.ViewModels;
using ContactManagement.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Implementations
{
    public class ContactCategoryRepository : IContactCategoryRepository
    {
        private IHttpContextAccessor _accessor;
        private readonly ContactManagementContext _context;
        public Guid UserId { get; set; }
        public ContactCategoryRepository(IHttpContextAccessor accessor, ContactManagementContext context)
        {
            _accessor = accessor;
            _context = context;

            UserId = Guid.Parse(_accessor?.HttpContext?.User?.Identity?.Name);
        }
        public void Delete(ContactCategoryDto category)
        {
            var searchCategory = _context.ContactCategories.Where(a => a.Id == category.Id).FirstOrDefault();
            _context.ContactCategories.Remove(searchCategory);
            _context.SaveChanges();
        }

        public async Task<List<ContactCategoryViewModel>> GetAll()
        {
            var data = await _context.ContactCategories.Where(a => a.UserInfoId == UserId)
               .ProjectTo<ContactCategoryViewModel>().ToListAsync();
            return data;
        }

        public async Task<ContactCategoryViewModel> GetById(int id)
        {
            var data = await _context.ContactCategories.Where(a => a.Id == id)
               .ProjectTo<ContactCategoryViewModel>().FirstOrDefaultAsync();
            return data;
        }

        public void Save(ContactCategoryDto category)
        {
            var cat = new ContactCategory()
            {
                Title = category.Title,
                Description = category.Description,
                UserInfoId = UserId
            };
            _context.ContactCategories.Add(cat);
            _context.SaveChanges();

        }

        public void Update(ContactCategoryDto category)
        {
            var searchCategory = _context.ContactCategories.Where(a => a.Id == category.Id).FirstOrDefault();
            _context.ContactCategories.Update(searchCategory);
            _context.SaveChanges();
        }
    }
}
