using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Lime.Helper
{
    public static class StringConverter
    {
        public static byte[] StringToBytes(string s)
        {
            return Encoding.Default.GetBytes(s);
        }
        public static string BytestoString(byte[] b)
        {
            return Encoding.Default.GetString(b);
        }

        public static byte[] Decompress(byte[] B)
        {
            try
            {
                MemoryStream ms = new MemoryStream(B);
                GZipStream gzipStream = new GZipStream((Stream)ms, CompressionMode.Decompress);
                byte[] buffer = new byte[4];
                ms.Position = checked(ms.Length - 5L);
                ms.Read(buffer, 0, 4);
                int count = BitConverter.ToInt32(buffer, 0);
                ms.Position = 0L;
                byte[] AR = new byte[checked(count - 1 + 1)];
                gzipStream.Read(AR, 0, count);
                gzipStream.Dispose();
                ms.Dispose();
                return AR;
            }
            catch
            {
                return null;
            }
        }

        public static string Encode(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        } // StringConverter.Encode string UTF-8

        public static string Decode(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        } // Decode string UTF-8

    }
}
