using System;
using System.IO;
using System.IO.Compression;
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

        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0L;
                GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);
                MemoryStream memoryStream2 = new MemoryStream();
                byte[] array = new byte[64];
                for (int i = gzipStream.Read(array, 0, array.Length); i > 0; i = gzipStream.Read(array, 0, array.Length))
                {
                    memoryStream2.Write(array, 0, i);
                }
                gzipStream.Close();
                return memoryStream2.ToArray();
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
