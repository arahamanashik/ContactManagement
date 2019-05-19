using ContactManagement.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Abstractions
{
    public interface IUserInfoRepository
    {
        UserInfoDto Authenticate(string username, string password);
        Task Register(UserInfoDto user);
        Task<int> CommitAsync();
    }
}
