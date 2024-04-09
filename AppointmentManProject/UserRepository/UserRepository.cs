using AppointmentManProject.Data;
using AppointmentManProject.Models;

namespace AppointmentManProject.UserRepository
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
    }
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public User GetUserByUsername(string username)
        {
            return context.user.FirstOrDefault(u => u.userName == username);
        }
    }
}
