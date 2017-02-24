using System.Linq;
using Common;
using Common.Models;

namespace DataAccess
{
    public sealed class UsersRepo : ConnectionClass
    {
        // TODO Must be case sensitive 
        public bool AuthenticateUser(LoginModel user)
        {
            return Entity.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password) != null;
        }

        public User GetUserByUsername(string username)
        {
            return Entity.Users.SingleOrDefault(u => u.Username == username);
        }
    }
}
