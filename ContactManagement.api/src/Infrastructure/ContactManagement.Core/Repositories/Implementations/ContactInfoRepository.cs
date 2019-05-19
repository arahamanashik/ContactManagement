using AutoMapper.QueryableExtensions;
using ContactManagement.Core.Data;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using ContactManagement.Core.ViewModels;
using ContactManagement.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Implementations
{

    public class ContactInfoRepository : IContactInfoRepository
    {
        private IHttpContextAccessor _accessor;
        private readonly ContactManagementContext _context;
        private readonly IHostingEnvironment _hosting;
        public Guid UserId { get; set; }
        public ContactInfoRepository(IHttpContextAccessor accessor, ContactManagementContext context, IHostingEnvironment hosting)
        {
            _accessor = accessor;
            _context = context;
            _hosting = hosting;

            UserId = Guid.Parse(_accessor?.HttpContext?.User?.Identity?.Name);
        }
        public void Delete(ContactInfoDto contactInfo)
        {
            var contact = _context.ContactInfoes.Where(a => a.Id == contactInfo.Id).FirstOrDefault();
            _context.ContactInfoes.Remove(contact);
            _context.SaveChanges();
        }
        public Task<List<ContactInfoViewModel>> GetAll(ContactInfoSearchArgs args)
        {
            // var data = _context.ContactInfoes.Where(a => a.UserInfoId == UserId).AsQueryable();
            //.ProjectTo<ContactInfoViewModel>().ToListAsync();

            var data = _context.ContactInfoes.Where(a =>
            a.UserInfoId == UserId
            &&
            (
                a.Name.Contains(args.Name??"") ||
                a.Mobile.Contains(args.Mobile ?? "") ||
                a.Email.Contains(args.Email ?? "") ||
                a.Address.Contains(args.Address ?? "")
                )
            )
                .ProjectTo<ContactInfoViewModel>().ToListAsync();

            return data;
        }
        public async Task<ContactInfoViewModel> GetById(int id)
        {
            var data = await _context.ContactInfoes.Where(a => a.Id == id)
                .ProjectTo<ContactInfoViewModel>().FirstOrDefaultAsync();
            return data;
        }
        public void Save(ContactInfoDto contactInfo)
        {
            string fileName = "";
            if (contactInfo.ProfilePicture != null)
                fileName = Guid.NewGuid() + Path.GetExtension(contactInfo.ProfilePicture.FileName);
            var contact = new ContactInfo()
            {
                Name = contactInfo.Name,
                Email = contactInfo.Email,
                Mobile = contactInfo.Mobile,
                Address = contactInfo.Address,
                ProfilePicture = fileName,
                ContactCategoryId = contactInfo.ContactCategoryId,
                UserInfoId = UserId
            };
            _context.ContactInfoes.Add(contact);
            _context.SaveChanges();

            if (contactInfo.ProfilePicture != null)
            {
                var rootPath = $"{_hosting.WebRootPath}/Documents/ProfilePicture";
                WriteFile($"{rootPath}", fileName, contactInfo.ProfilePicture);
            }

        }
        private void WriteFile(string path, string fileName, IFormFile file)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream($"{path}/{fileName}", FileMode.Create))
                file.CopyTo(fileStream);
        }
        private void RemoveFile(string path, string fileName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.Delete($"{path}/{fileName}");
        }
        public void Update(ContactInfoDto contactInfo)
        {
            var contact = _context.ContactInfoes.Where(a => a.Id == contactInfo.Id).FirstOrDefault();

            string fileName = "";
            if (contactInfo.ProfilePicture != null)
            {
                var rootPath = $"{_hosting.WebRootPath}/Documents/ProfilePicture";
                RemoveFile($"{rootPath}/", $"{contact.ProfilePicture}");

                fileName = Guid.NewGuid() + Path.GetExtension(contactInfo.ProfilePicture.Name);
                contact.ProfilePicture = fileName;

                WriteFile($"{rootPath}", fileName, contactInfo.ProfilePicture);
            }

            _context.ContactInfoes.Update(contact);
            _context.SaveChanges();
        }
    }
}
