using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(int id);
        Task<User?> Login(string userName, string password);
        Task Add(Role role);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL USERS
        public async Task<IEnumerable<User>> GetAll()
        {
            //return await _context.Users
            //    .FromSqlRaw(@"
            //        CALL GetAllUsers()
            //    ")
            //    .AsNoTracking()
            //    .ToListAsync();

            return await _context.Users.Include(x => x.Role).ToListAsync();
                
        }

        // GET USER BY ID
        public async Task<User?> GetById(int id)
        {
            return await _context.Users
                .FromSqlRaw("CALL GetUserById({0})", id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        // ADD USER
        public async Task Add(User user)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL AddUser({0}, {1}, {2}, {3})",
                user.UserName,
                user.Password,
                user.Email,
                user.RoleId
            );
        }

        // UPDATE USER
        public async Task Update(User user)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL UpdateUser({0}, {1}, {2}, {3}, {4})",
                user.Id,
                user.UserName,
                user.Email,
                user.Password,
                user.RoleId
            );
        }

        // DELETE USER
        public async Task Delete(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL DeleteUser({0})", id
            );
        }

        // LOGIN
        public async Task<User?> Login(string userName, string password)
        {
            return await _context.Users
                .FromSqlRaw("CALL sp_Login({0}, {1})", userName, password)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


         //ADD ROLE
        public async Task Add(Role role)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL AddRole({0})",
                role.RoleName
            );
        }

    }

}



