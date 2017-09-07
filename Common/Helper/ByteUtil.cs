using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class ByteUtil
    {

        public static byte[] String2AsciiBytes(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        /// <summary>  
        /// 获取字符串字节编码  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static byte[] String2UTF8ByteArray(string str)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(str);
        }

        /// <summary>  
        /// 将字节编码转化为UTF-8字符串  
        /// </summary>  
        /// <param name="bytes"></param>  
        /// <returns></returns>  
        public static string ByteArray2String(byte[] bytes)
        {
            return System.Text.UTF8Encoding.UTF8.GetString(bytes);
        }

        /// <summary>  
        /// 将Short转换为无符号Int  
        /// </summary>  
        /// <returns>The short2 integer.</returns>  
        /// <param name="s">S.</param>  
        public static int ConvertShort2Integer(short s)
        {
            return 0x0000ffff & s;
        }

        /// <summary>  
        /// 转换Short数组为Byte数组  
        /// </summary>  
        /// <returns>The short array2 byte array.</returns>  
        /// <param name="s">S.</param>  
        public static byte[] ConvertShortArray2ByteArray(short[] s)
        {
            if (s == null || s.Length <= 0)
            {
                return null;
            }
            byte[] res = new byte[s.Length * 2];
            for (int i = 0; i < s.Length; i++)
            {
                res[i * 4] = (byte)(s[i] >> 24);
                res[i * 4 + 1] = (byte)(s[i] >> 16);
                res[i * 4 + 2] = (byte)(s[i] >> 8);
                res[i * 4 + 3] = (byte)s[i];
            }
            return res;
        }

        /// <summary>  
        /// 将Byte数组转换为Int数组  
        /// </summary>  
        /// <returns>The byte array2 int array.</returns>  
        /// <param name="s">S.</param>  
        public static int[] ConvertByteArray2IntArray(byte[] s)
        {
            if (s == null || s.Length <= 0)
            {
                return null;
            }
            int[] res = new int[s.Length / 4];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = ToInt(s[i * 4], s[i * 4 + 1], s[i * 4 + 2], s[i * 4 + 3]);
            }
            return res;
        }

        /// <summary>  
        /// 将Byte数组转换成Short数组  
        /// </summary>  
        /// <returns>The byte array2 short array.</returns>  
        /// <param name="s">S.</param>  
        public static short[] ConvertByteArray2ShortArray(byte[] s)
        {
            if (s == null || s.Length <= 0)
            {
                return null;
            }
            short[] res = new short[s.Length / 2];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = (short)ToInt(s[i * 2], s[i * 2 + 1]);
            }
            return res;
        }

        /// <summary>  
        /// 将2-Byte转换成Short  
        /// </summary>  
        /// <returns>The short.</returns>  
        /// <param name="a1">A1.</param>  
        /// <param name="a2">A2.</param>  
        public static int ToInt(byte a1, byte a2)
        {
            return (a1 << 8) & 0x0000ff00 | a2 & 0x000000ff;
        }

        /// <summary>  
        /// 将4-Byte转换成1-Int  
        /// </summary>  
        /// <returns>The int.</returns>  
        /// <param name="a1">A1.</param>  
        /// <param name="a2">A2.</param>  
        /// <param name="a3">A3.</param>  
        /// <param name="a4">A4.</param>  
        public static int ToInt(byte a1, byte a2, byte a3, byte a4)
        {
            return (a1 << 24) | (a2 << 16) & 0x00ff0000 | (a3 << 8) & 0x0000ff00 | a4 & 0x000000ff;
        }

        /// <summary>  
        /// 将十六进制字符转换成数组  
        /// </summary>  
        /// <returns>The hpy char2 byte.</returns>  
        /// <param name="c">C.</param>  
        public static byte ConvertHpyChar2Byte(char c)
        {
            if (c > 57)
            {
                return (byte)(c - 87);
            }
            else
            {
                return (byte)(c - 48);
            }
        }

        /// <summary>  
        ///  将数组转换成十六进制字符  
        /// </summary>  
        /// <returns>The hyp2 char.</returns>  
        /// <param name="value">Value.</param>  
        public static char ConvertHyp2Char(int value)
        {
            if (value < 0 || value > 15)
            {
                throw new Exception("Param Value Is Error...");
            }
            if (value < 10)
            {
                return (char)(48 + value);
            }
            else
            {
                return (char)(87 + value);
            }
        }

        /// <summary>
        /// 附加byte[]数组 ，把buffer2附加到buffer1之后
        /// </summary>
        /// <param name="buffer1"></param>
        /// <param name="buffer2"></param>
        /// <returns></returns>
        public static byte[] AppendByteArray(byte[] buffer1, byte[] buffer2)
        {
            int length = buffer1.Length + buffer2.Length;
            byte[] buffer = new byte[length];

            Buffer.BlockCopy(buffer1, 0, buffer, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, buffer, buffer1.Length, buffer2.Length);
            return buffer;
        }

        /// <summary>
        /// 从指定位数截取指定长度的字节数组
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="num">开始截取的位数</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static byte[] InterceptByteArray(byte[] buffer, int num, int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(buffer, num, temp, 0, length);
            return temp;
        }




        /// <summary>
        /// 在 s 中查找 pattern 。
        /// 如果找到，返回 pattern 在 s 中第一次出现的位置(0起始)。
        /// 如果没找到，返回 -1。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int IndexOf(byte[] s, byte[] pattern)
        {
            int slen = s.Length;
            int plen = pattern.Length;
            for (int i = 0; i <= slen - plen; i++)
            {
                int j = 0;
                for (; j < plen; j++)
                {
                    if (s[i + j] != pattern[j]) break;
                }
                if (j == plen) return i;
            }
            return -1;
        }

        #region 转字节序

        /// <summary>
        /// 将int转为低字节在前，高字节在后的byte数组
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static byte[] ToLH(int n)
        {
            byte[] b = new byte[4];
            b[0] = (byte)(n & 0xff);
            b[1] = (byte)(n >> 8 & 0xff);
            b[2] = (byte)(n >> 16 & 0xff);
            b[3] = (byte)(n >> 24 & 0xff);
            return b;
        }

        public static byte[] ToHH(int n)
        {
            byte[] b = new byte[4];
            b[3] = (byte)(n & 0xff);
            b[2] = (byte)(n >> 8 & 0xff);
            b[1] = (byte)(n >> 16 & 0xff);
            b[1] = (byte)(n >> 24 & 0xff);
            return b;
        }

        public static byte GetHighByte(short value)
        {
            return (byte)((value & 0xf0) >> 4);
        }

        public static byte GetHighByte(sbyte value)
        {
            return (byte)((value & 0xf0) >> 4);
        }

        public static byte GetLowByte(short value)
        {
            return (byte)(value & 0xf);
        }

        public static byte GetLowByte(sbyte value)
        {
            return (byte)(value & 0xf);
        }
        #endregion

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="b1">byte数组1</param>
        /// <param name="b2">byte数组2</param>
        /// <returns>是否相等</returns>
        public static bool Equals(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;
            return true;
        }


        /// <summary>
        /// 过滤信令中1010的情况
        /// </summary>
        /// <param name="recBytes"></param>
        /// <returns></returns>
        public static byte[] FilterDouble10(byte[] recBytes)
        {
            int len = recBytes.Length;
            if (len <= 4)
                return new byte[0];

            byte[] bytesTmp = new byte[len - 4];

            int end = len - 2;
            int index = 0;
            for (int i = 2; i < end; i++)
            {
                if (i == (end - 1))
                {
                    bytesTmp[index] = recBytes[i];
                    index++;
                    break;
                }

                if ((recBytes[i] == 0x10) && (recBytes[i + 1] == 0x10))
                {
                    bytesTmp[index] = 0x10;
                    index++;
                    i++;
                    continue;
                }

                bytesTmp[index] = recBytes[i];
                index++;
            }

            byte[] rtn = new byte[index + 4];
            rtn[0] = 0x10;
            rtn[1] = 0x02;
            System.Array.Copy(bytesTmp, 0, rtn, 2, index);
            rtn[rtn.Length - 2] = 0x10;
            rtn[rtn.Length - 1] = 0x03;

            return rtn;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        ///<summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
    }
}
