using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdSrv.Console
{
    public static class Users
    {
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "mohsin.khan",
                    Password = "Password",
                    Subject = "1",
                },
                new InMemoryUser
                {
                    Username = "mohsink13",
                    Password = "Password",
                    Subject = "2",
                }
            };
        }
    }
}
