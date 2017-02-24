using Common;
using Common.Models;
using DataAccess;

namespace BusinessLogic
{
    public class UsersBl
    {
        public bool AuthenticateUser(LoginModel user)
        {
            return new UsersRepo().AuthenticateUser(user);
        }

        public User GetUserByUsername(string username)
        {
            return new UsersRepo().GetUserByUsername(username);
        }
    }
}
