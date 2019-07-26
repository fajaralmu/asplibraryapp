using OurLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Service
{

    public class UserService :BaseService
    {
        public bool IsValid(user User)
        {
            if(User == null)
            {
                return false;
            }
            return GetUser(User.username, User.password) != null;
        }

        public user GetUser(string Username, string Password)
        {
            if(dbEntities == null || Username == null || Password == null)
            {
                return null;
            }
            user User = (from u in dbEntities.users
                         where u.username.Equals(Username) && u.password.Equals(Password)
                         select u).SingleOrDefault();
            if (User != null)
            {
                return User;
            }
            return null;
        }

    }
}