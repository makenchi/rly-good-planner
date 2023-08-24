using UserAuthApi.Entities;

namespace UserAuthApi.Data.Interfaces
{
    public interface IRepositoryUser
    {
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
        Task<User> GetById(string email);
    }
}
