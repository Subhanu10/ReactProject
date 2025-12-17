using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
   public  interface IUserRepository
   {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(int id);
        Task<User?> Login(string userName, string password);

   }    
        public class UserRepository : IUserRepository
        {
            private readonly AppDbContext _context;

            public UserRepository(AppDbContext context)
            {
                _context = context;
            }

            //  GET ALL
            public async Task<IEnumerable<User>> GetAll()
            {
                return await _context.Users
                .FromSqlRaw("CALL GetAllUsers()")
                .ToListAsync();
            }

            //  GET BY ID
            public async Task<User?> GetById(int id)
            {
                return _context.Users
                .FromSqlRaw("CALL GetUserById({0})", id)
                .AsEnumerable()
                .FirstOrDefault();
            }

            //  CREATE
            public async Task Add(User user)
            {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL AddUser({0}, {1}, {2})",
                user.UserName,
                user.Password,
                user.Email
                );
            
            }

            //  UPDATE
            public async Task Update(User user)
            {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL UpdateUser({0}, {1}, {2}, {3})",
                user.Id,
                user.UserName,
                user.Password,
                user.Email
                );
            
            }

            //  DELETE
            public async Task Delete(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL DeleteUser({0})", id);
                }
            }

            //  LOGIN
            public async Task<User?> Login(string userName, string password)
            {
            return _context.Users
                .FromSqlRaw("CALL sp_Login({0}, {1})", userName, password)
                .AsEnumerable()
                .FirstOrDefault();

            }
        }
}


