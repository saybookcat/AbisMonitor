using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Encrypt
{
    public static class Md5Encrypt
    {
        public static string EncryptPassword(string plainPassWord)
        {
            if (plainPassWord == null)
            {
                plainPassWord = "";
            }

            MD5 md5 = MD5.Create();
            byte[] bPwd = md5.ComputeHash(Encoding.UTF8.GetBytes(plainPassWord));

            StringBuilder strHexPwd = new StringBuilder();
            foreach (byte b in bPwd)
                strHexPwd.Append(b.ToString("X2"));

            return strHexPwd.ToString();
        }
    }
}
