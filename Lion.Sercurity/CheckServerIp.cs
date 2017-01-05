using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Sercurity
{
    public class CheckServerIp
    {
        // check your ip
        public static string Checkip(string server_ip, string your_ip)
        {
            if (server_ip == your_ip)
            {
                return "OK";
            }
            return "FAlSE";
        }
    }
}
