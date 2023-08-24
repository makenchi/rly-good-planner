using UserAuthApi.Entities;

namespace UserAuthApi.Services.Interfaces
{
    public interface IUserService
    {
        Task createUser(User user);
        Task removeUser(User user);
        Task updateUser(User user);
        Task<User> getUserById(string email);
    }
}
