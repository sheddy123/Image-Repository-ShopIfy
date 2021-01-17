using ImageRepoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Repository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username, string emailAddress);
        User Authenticate(string username, string password);
        User Register(string username, string password, string emailAddress);
    }
}
