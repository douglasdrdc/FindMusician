using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindMusician.API.Models;

namespace FindMusician.API.Services
{
    public class UserService
    {
        private List<User> Users { get; set; }

        public UserService()
        {
            this.Users = new List<User>();
            this.Users.Add(new User() { Login = "usuario01", AccessKey = "94be650011cf412ca906fc335f615cdc" });
            this.Users.Add(new User() { Login = "usuario02", AccessKey = "531fd5b19d58438da0fd9afface43b3c" });
        }

        public List<User> Get()
        {
            return this.Users;
        }

        public User Get(string id)
        {
            return this.Users.Where(x => x.Login == id).FirstOrDefault();
        }
    }
}
