using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data.Interfaces;
using UserAuthApi.Entities;

namespace UserAuthApi.Data.Repos
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly DataContext _context;
        public RepositoryUser(DataContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task Delete(User user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<User> GetById(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task Update(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
