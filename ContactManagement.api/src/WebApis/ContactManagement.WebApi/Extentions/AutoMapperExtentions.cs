using AutoMapper;
using ContactManagement.Core.ViewModels;
using ContactManagement.Entities;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace ContactManagement.WebApi.Extentions
{
    public static class AutoMapperExtentions
    {
        public static void ConfigAutoMapper(this IApplicationBuilder app)
        {
            Mapper.Initialize(m =>
            {
                m.CreateMap<UserInfo, UserInfoViewModel>();
                m.CreateMap<ContactInfo, ContactInfoViewModel>();
                m.CreateMap<ContactCategory, ContactCategoryViewModel>();

            });
        }
    }
}
