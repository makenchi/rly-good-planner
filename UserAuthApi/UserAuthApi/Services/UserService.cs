using UserAuthApi.Data.Interfaces;
using UserAuthApi.Entities;
using UserAuthApi.Services.Interfaces;

namespace UserAuthApi.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUser _repositoryUser;
        public UserService(IRepositoryUser repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }

        public async Task createUser(User user)
        {
            user.isConfirmed = false;
            user.isBlocked = false;
            user.CreatedAt = DateTime.Now;
            user.LastModifiedAt = DateTime.Now;

            await _repositoryUser.Add(user);
        }

        public async Task<User> getUserById(string email)
        {
            var user = await _repositoryUser.GetById(email);
            
            return user;
        }

        public async Task removeUser(User user)
        {
            await _repositoryUser.Delete(user);
        }

        public async Task updateUser(User user)
        {
            await _repositoryUser.Update(user);
        }
    }
}
