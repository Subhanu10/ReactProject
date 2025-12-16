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
                return await _context.Users.ToListAsync();
            }

            //  GET BY ID
            public async Task<User?> GetById(int id)
            {
                return await _context.Users.FindAsync(id);
            }

            //  CREATE
            public async Task Add(User user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            //  UPDATE
            public async Task Update(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            //  DELETE
            public async Task Delete(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }

            //  LOGIN
            public async Task<User?> Login(string userName, string password)
            {
                return await _context.Users
                    .FirstOrDefaultAsync(x =>
                        x.UserName == userName &&
                        x.Password == password); 
            }
        }
    }


