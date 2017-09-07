using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class IpAndMacHelper
    {
        public static void GetLocalMessage(out IPAddress ip, out IPAddress mask, out string mac)
        {
            ip = null;
            mask = null;
            mac = null;

            try
            {
                //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //ManagementObjectCollection nics = mc.GetInstances();
                System.Management.ManagementObjectSearcher query = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection queryCollection = query.Get();

                string strIp = null, strMask = null, strMac = null;
                foreach (var o in queryCollection)
                {
                    var nic = (ManagementObject)o;
                    if (Convert.ToBoolean(nic["IPEnabled"]) == false)
                        continue;

                    if (nic["IPAddress"] == null)
                        continue;
                    var strIpAddress = nic["IPAddress"] as string[];
                    if (strIpAddress != null) strIp = strIpAddress[0];

                    if (nic["IPSubnet"] == null)
                        continue;
                    var strIpSubnet = nic["IPSubnet"] as string[];
                    if (strIpSubnet != null) strMask = strIpSubnet[0];

                    if (nic["MACAddress"] == null)
                        continue;
                    strMac = nic["MACAddress"] as string;

                    if (strIp != null) ip = IPAddress.Parse(strIp);
                    if (strMask != null) mask = IPAddress.Parse(strMask);
                    mac = strMac;
                    break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static long ConvertIpAddress(IPAddress ip)
        {
            if (ip == null)
            {
                return -1;
            }

            byte[] b = ip.GetAddressBytes();

            long addr = (b[3] & 0xFF);
            addr = (addr << 8) + (b[2] & 0xFF);
            addr = (addr << 8) + (b[1] & 0xFF);
            addr = (addr << 8) + (b[0] & 0xFF);

            return addr;
        }

        public static string IpNetWorkToHostString(long ip)
        {
            long result;
            if (long.TryParse(ip.ToString(), out result))
            {
                try
                {
                    if (ip <= 0) return null;
                    var tempIP = (uint)IPAddress.NetworkToHostOrder((Int32)ip);

                    IPAddress ipAddress = new IPAddress(tempIP);

                    return ipAddress.ToString();
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static string IpNetWorkToHostString(uint ip)
        {
            try
            {
                if (ip == 0) return "0.0.0.0";
                var tempIp = (uint)IPAddress.NetworkToHostOrder((Int32)ip);
                IPAddress ipAddress = new IPAddress(tempIp);
                return ipAddress.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static uint ConvertIpAddressToNetwork(IPAddress ip)
        {
            var temp = ConvertIpAddress(ip);
            var netWorkIp = (uint)IPAddress.HostToNetworkOrder((Int32)temp);

            return netWorkIp;
        }


        /// <summary>
        /// 验证String类型的Ip，合格时返回unit格式IP，错误时返回null
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static uint? ValidateIpToUnit(string ip)
        {
            var re = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            if (!re.IsMatch(ip))
            {
                return null;
            }
            IPAddress ipa;
            bool flag = IPAddress.TryParse(ip, out ipa);
            if (!flag || ipa.Equals(IPAddress.Any) || ipa.Equals(IPAddress.Broadcast))
            {
                return null;
            }
            return ConvertIpAddressToNetwork(ipa);
        }

        public static uint? ConvertIpToNetwork(string ip)
        {
            IPAddress ipa;
            bool flag = IPAddress.TryParse(ip, out ipa);
            if (!flag || ipa.Equals(IPAddress.Any) || ipa.Equals(IPAddress.Broadcast))
            {
                return null;
            }
            return ConvertIpAddressToNetwork(ipa);
        }

        public static int ConvertIpAddressToHost(IPAddress ip)
        {
            var temp = ConvertIpAddress(ip);
            var netWorkIp = IPAddress.NetworkToHostOrder((Int32)temp);

            return netWorkIp;
        }

        public static IPAddress ConvertIpAddress(long ip)
        {
            return new IPAddress(ip);
        }

        public static long ConvertMac(string mac)
        {
            long returnValue = -1;

            if (string.IsNullOrWhiteSpace(mac))
            {
                return -1;
            }

            Regex re = new Regex(@"^([0-9a-fA-F]{1,2}):([0-9a-fA-F]{1,2}):([0-9a-fA-F]{1,2}):([0-9a-fA-F]{1,2}):([0-9a-fA-F]{1,2}):([0-9a-fA-F]{1,2})$");
            Match m = re.Match(mac);
            if (m.Success)
            {
                long lngTmp = 0;
                if (long.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = lngTmp << 40;

                if (long.TryParse(m.Groups[2].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = returnValue + (lngTmp << 32);

                if (long.TryParse(m.Groups[3].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = returnValue + (lngTmp << 24);

                if (long.TryParse(m.Groups[4].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = returnValue + (lngTmp << 16);

                if (long.TryParse(m.Groups[5].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = returnValue + (lngTmp << 8);

                if (long.TryParse(m.Groups[6].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lngTmp) == false)
                {
                    returnValue = 0;
                    return returnValue;
                }
                else
                    returnValue = returnValue + lngTmp;
            }
            else
                returnValue = 0;

            return returnValue;
        }

        public static string ConvertMac(long mac)
        {
            byte[] bytes = new byte[6];

            bytes[0] = (byte)(mac & 0xFF);
            bytes[1] = (byte)((mac >> 8) & 0xFF);
            bytes[2] = (byte)((mac >> 16) & 0xFF);
            bytes[3] = (byte)((mac >> 24) & 0xFF);
            bytes[4] = (byte)((mac >> 32) & 0xFF);
            bytes[5] = (byte)((mac >> 40) & 0xFF);

            return string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]);
        }

    }
}
