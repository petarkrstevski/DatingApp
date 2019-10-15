using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Username == username);

            if(user == null)
            {
                return null;
            }

            if(!VerifyUserPassword(password,user))
            {
                return null;
            }

            return user;
        }



        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password,out passwordHash,out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

           return user;
        }



        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=> x.Username == username))
            {
                return true;
            }

            return false;
        }

        #region  PrivateMethods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512)
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyUserPassword(string password, User user)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var userPasswordHash = user.PasswordHash; 

                for (int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if(computedPasswordHash[i] != userPasswordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion  PrivateMethods
    }
}