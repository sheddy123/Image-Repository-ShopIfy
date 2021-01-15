using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRepoAPI.Models
{
    public class EncryptPassword
    {
        public static string textToEncrypt(string passWord)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passWord)));
        }
    }
}
