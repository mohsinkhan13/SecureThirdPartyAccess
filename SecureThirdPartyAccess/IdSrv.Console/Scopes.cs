using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace IdSrv.Console
{
    public static class Scopes
    {
        public static List<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "deloitte"
                }
            };
        }
    }
}
