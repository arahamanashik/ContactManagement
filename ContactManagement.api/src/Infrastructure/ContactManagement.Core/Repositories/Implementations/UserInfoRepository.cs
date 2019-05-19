using AutoMapper.QueryableExtensions;
using ContactManagement.Core.Data;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using ContactManagement.Core.ViewModels;
using ContactManagement.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Core.Repositories.Implementations
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly ContactManagementContext _context;
        private IConfiguration _configuration;
        public UserInfoRepository(ContactManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public UserInfoDto Authenticate(string email, string password)
        {

            var secret = _configuration["AppSecret"].ToString();
            var key = Convert.FromBase64String(secret);

            var aa = _context.UserInfoes.ProjectTo<UserInfoViewModel>().ToList();

            var userData = _context.UserInfoes.Where(a => a.Email == email).ProjectTo<UserInfoViewModel>().FirstOrDefault();

            if (userData == null)
                return null;
            byte[] passwordSalt = Convert.FromBase64String(userData.PasswordSalt);

            //byte[] salt = new byte[128 / 8];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: passwordSalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));



            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (userData.PasswordHashed != hashed)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            //  var key = Encoding.Unicode.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userData.Id.ToString())
                    ,new Claim("UserName", userData.Name.ToString())
                    ,new Claim(ClaimTypes.Email, userData.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserInfoDto user = new UserInfoDto();

            user.Name = userData.Name;
            user.Mobile = userData.Mobile;
            user.Email = userData.Email;
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Register(UserInfoDto user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var newUser = new UserInfo()
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Mobile = user.Mobile,
                PasswordHashed = hashed,
                PasswordSalt = Convert.ToBase64String(salt)
            };

            await _context.UserInfoes.AddAsync(newUser);
        }
    }
}
