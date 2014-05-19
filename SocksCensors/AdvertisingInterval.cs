using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace NSSocksCensors
{
    public class AdvertiserInfo
    {
        public IPAddress IpAddress = null;
        public int Port = 0;
        public int Interval = 1; // minute
    }
}
