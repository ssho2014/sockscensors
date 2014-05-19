using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.Security;

namespace NSSocksCensors
{
    public class StateObject
    {
        // Close all sockets in the current object
        public void Close()
        {
            try
            {
                if (Server != null)
                {
                    // Using SSL?
                    if (IsSslEnabled)
                    {
                        // Shutdown SSL stream
                        ServerSslStream.Flush();
                        Server.Close();
                    }
                    else
                    {
                        // Shutdown stream
                        Server.GetStream().Flush();
                        Server.Close();
                    }
                }
                if (Client != null)
                {
                    // Shutdown client socket
                    Client.GetStream().Flush();
                    Client.Close();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                CanDispose = true;
            }
        }

        // Whether the current object is obsoleted and can be disposed
        public bool CanDispose = false;

        // Default buffer size
        private const int DefaultBufferSize = 4096;
        public int BufferSize { get { return DefaultBufferSize; } }
        
        // Server connection
        public TcpClient Server = null;
        // Server SSL stream
        public SslStream ServerSslStream = null;
        // Server buffer
        public byte[] ServerBuffer = new byte[DefaultBufferSize];

        // Client socket
        public TcpClient Client = null;
        // Client buffer
        public byte[] ClientBuffer = new byte[DefaultBufferSize];

        // Using SSL
        public bool IsSslEnabled = false;
    }
}
