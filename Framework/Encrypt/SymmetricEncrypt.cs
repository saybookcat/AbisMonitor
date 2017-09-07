using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Encrypt
{
    public class SymmetricEncrypt
    {
        private static SymmetricAlgorithm _mobjCryptoService;
        private static string _key;
        static SymmetricEncrypt()
        {
            _mobjCryptoService = new RijndaelManaged();
            _key = "Guz(%&hj7x89H$yuBI0212s76*h%(HilJ$lhj!y6&(*j2322111de)7";
        }

        /// <summary>    
        /// 获得密钥    
        /// </summary>    
        /// <returns>密钥</returns>    
        private static byte[] GetLegalKey()
        {
            string sTemp = _key;
            _mobjCryptoService.GenerateKey();
            byte[] bytTemp = _mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private static byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg9!rN430f566*&*(ghUb#HBh(u%g$jhWk7&!hg4ui%$hjk";
            _mobjCryptoService.GenerateIV();
            byte[] bytTemp = _mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>    
        /// 加密方法    
        /// </summary>    
        /// <param name="source">待加密的串</param>    
        /// <returns>经过加密的串</returns>    
        public static string Encrypto(string source)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source)) return null;
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(source);
                MemoryStream ms = new MemoryStream();
                _mobjCryptoService.Key = GetLegalKey();
                _mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = _mobjCryptoService.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>    
        /// 解密方法    
        /// </summary>    
        /// <param name="source">待解密的串</param>    
        /// <returns>经过解密的串</returns>    
        public static string Decrypto(string source)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source)) return null;
                byte[] bytIn = Convert.FromBase64String(source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                _mobjCryptoService.Key = GetLegalKey();
                _mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = _mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
