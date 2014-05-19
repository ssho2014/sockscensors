using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NSSocksCensors
{
    public partial class SocksCensors : Form
    {
        // The local port to listen to
        private int _localPort = 8888;
        // Remote host 
        private string _remoteHost = null;
        // Remote port
        private int _remotePort = 8888;
        // List of all active state objects
        List<StateObject> _stateObjectList = new List<StateObject>();
        // Listener
        TcpListener _listener = null;
        // Total bytes received
        private long _totalBytesReceived = 0;
        // Total bytes sent
        private long _totalBytesSent = 0;
        // Timer counter for disposing unused state objects
        private int _timerCount = 0;
        // Should we use SSL?
        private bool _isSslEnabled = true;

        public SocksCensors()
        {
            InitializeComponent();

            // Is the On/Off button showing "Start"
            btnOnOff.Tag = true;
            // Is SSL enabled?
            cbxSsl.Checked = _isSslEnabled;
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            // Check if button currently showing "Start"
            if ((bool)btnOnOff.Tag)
            {
                // Start proxy service
                StartService();
            }
            else
            {
                // Stop proxy service
                StopService();
            }
        }

        private void StopService()
        {
            // Stopy listen
            StopListen();

            // Enable all text boxes
            tbxLocalPort.Enabled = true;
            tbxRemoteHost.Enabled = true;
            tbxRemotePort.Enabled = true;
            // Change button text
            btnOnOff.Text = "Start";
            // Update tag
            btnOnOff.Tag = true;
            // SSL check box
            cbxSsl.Enabled = true;
        }

        private void StartService()
        {
            #region Check inputs
            // Check if local port is valid
            try
            {
                tbxLocalPort.Text = tbxLocalPort.Text.Trim();
                _localPort = Convert.ToInt32(tbxLocalPort.Text);
                if (_localPort <= 0 || _localPort > 65535)
                {
                    throw new OverflowException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid local port.", "Invalid local port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Make sure remote host is not empty
            tbxRemoteHost.Text = tbxRemoteHost.Text.Trim();
            _remoteHost = tbxRemoteHost.Text;
            if (tbxRemoteHost.Text.Length == 0)
            {
                MessageBox.Show("Invalid remote host. Remote host cannot be empty.",
                    "Invalid Remote Host", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Check if remote port is valid
            try
            {
                tbxRemotePort.Text = tbxRemotePort.Text.Trim();
                _remotePort = Convert.ToInt32(tbxRemotePort.Text);
                if (_remotePort <= 0 || _remotePort > 65535)
                {
                    throw new OverflowException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid remote port.", "Invalid remote port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
            #endregion

            #region Disable controls
            // Disable all text boxes
            tbxLocalPort.Enabled = false;
            tbxRemoteHost.Enabled = false;
            tbxRemotePort.Enabled = false;
            // Change button text
            btnOnOff.Text = "Stop"; 
            // Update tag
            btnOnOff.Tag = false;
            // SSL check box
            cbxSsl.Enabled = false;
            #endregion

            // Start listening
            StartListen();
        }

        private void StopListen()
        {
            Debug.WriteLine("Closing all sockets.");

            // Stop timer
            stateObjectDisposeTimer.Stop();

            // Close main socket
            if (_listener != null)
            {
                _listener.Stop();
            }

            // Close all client sockets
            foreach (StateObject state in _stateObjectList)
            {
                state.Close();
            }

            // Remove all state objects
            _stateObjectList.Clear();
        }

        private void StartListen()
        {
            Debug.WriteLine("Entered BeginListen()");

            // Start timer
            stateObjectDisposeTimer.Start();

            // Create a TCP listener
            _listener = new TcpListener(IPAddress.Any, _localPort);
            Debug.WriteLine("Listener socket created.");

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                // Start listener
                _listener.Start();
                Debug.WriteLine("Listener started at port " + _localPort);

                // Start an asynchronous socket to listen for connections.
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
                Debug.WriteLine("Waiting for a new connection...");
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot listen to port " + _localPort + ". Another program may be using it.");
                StopListen();
                Debug.WriteLine(e.ToString());
            }
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            Debug.WriteLine("Entered OnClientConnect");

            try
            {
                // Get TCP client
                TcpClient client = _listener.EndAcceptTcpClient(ar);
                Debug.WriteLine("Accepted client.");

                // Create the state object
                StateObject state = new StateObject();
                state.Client = client;
                _stateObjectList.Add(state);
                Debug.WriteLine("Created a state object.");

                // Connect to server
                ServerConnect(state);
                Debug.WriteLine("Connected to server.");

                // Wait for server data
                WaitForServerData(state);

                // Wait for client data to come in
                WaitForClientData(state);

                // Now that we're done, let the main socket go back to listen
                _listener.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void WaitForServerData(StateObject state)
        {
            try
            {
                if (_isSslEnabled)
                {
                    // Use SSL
                    // Start waiting for server data
                    state.ServerSslStream.BeginRead(
                        state.ServerBuffer, 0, state.BufferSize,
                        new AsyncCallback(OnServerDataReceived), state);
                }
                else
                {
                    // Don't use SSL
                    // Start waiting for server data
                    state.Server.GetStream().BeginRead(
                        state.ServerBuffer, 0, state.BufferSize,
                        new AsyncCallback(OnServerDataReceived), state);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void OnServerDataReceived(IAsyncResult ar)
        {
            Debug.WriteLine("Entered OnServerDataReceived()");

            try
            {
                // Get state object, client connection, and server stream
                StateObject state = (StateObject)ar.AsyncState;
                TcpClient client = state.Client;
                TcpClient server = state.Server;
                SslStream serverSslStream = state.ServerSslStream;
                int bytesCount = 0;

                // Use SSL?
                if (_isSslEnabled)
                {
                    // Use SSL
                    // Retrieve server data
                    bytesCount = serverSslStream.EndRead(ar);
                }
                else
                {
                    // Don't use SSL
                    // Retrieve server data
                    bytesCount = server.GetStream().EndRead(ar);
                }
                if (bytesCount > 0)
                {
                    // Forward data to client
                    client.GetStream().Write(state.ServerBuffer, 0, bytesCount);
                    // Update data received
                    _totalBytesReceived += bytesCount;
                    // Continue wait for server data
                    WaitForServerData(state);
                }
                else
                {
                    // Server disconnected, we'll close all associated connections.
                    state.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void ServerConnect(StateObject state)
        {
            Debug.WriteLine("Entered ServerConnect()");

            // Connect to server
            try
            {
                // Create a new TCP connection
                TcpClient server = new TcpClient(_remoteHost, _remotePort);
                state.Server = server;
                Debug.WriteLine("Connected to server.");

                // Using SSL?
                if (_isSslEnabled)
                {
                    // Get SSL Stream
                    SslStream stream = new SslStream(server.GetStream(), false,
                                  new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                    stream.AuthenticateAsClient("bbg.gov", null, System.Security.Authentication.SslProtocols.Ssl3, false);
                    state.ServerSslStream = stream;
                    state.IsSslEnabled = true;
                    Debug.WriteLine("Established an SSL connection.");
                }
                else
                {
                    // Mark SSL as disabled in the state object
                    state.IsSslEnabled = false;
                    Debug.WriteLine("Established a non-SSL connection.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error connecting to server: " + e.ToString());
            }
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            // We accept all certificate
            return true;
        }

        private void WaitForClientData(StateObject state)
        {
            try
            {
                // Start waiting for client data
                state.Client.GetStream().BeginRead(
                    state.ClientBuffer, 0, state.BufferSize,
                    new AsyncCallback(OnClientDataReceived), state);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void OnClientDataReceived(IAsyncResult ar)
        {
            Debug.WriteLine("Entered OnClientDataReceived()");

            try
            {
                // Get state object
                StateObject state = (StateObject)ar.AsyncState;
                // Get client connection
                TcpClient client = state.Client;
                // Get server connection
                TcpClient server = state.Server;
                // Get server stream
                SslStream serverSslStream = state.ServerSslStream;

                // Retrieve data
                int bytesCount = client.GetStream().EndRead(ar);
                if (bytesCount > 0)
                {
                    // Using SSL?
                    if (_isSslEnabled)
                    {
                        // Using SSL
                        // Forward data to server
                        serverSslStream.Write(state.ClientBuffer, 0, bytesCount);
                    }
                    else
                    {
                        // Don't use SSL
                        // Forward data to server
                        server.GetStream().Write(state.ClientBuffer, 0, bytesCount);
                    }
                    // Update bytes sent
                    _totalBytesSent += bytesCount;
                    // Continue to wait for more data
                    WaitForClientData(state);
                }
                else
                {
                    // Client disconnected, we'll close all associated connections
                    state.Close();
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void stateObjectDisposeTimer_Tick(object sender, EventArgs e)
        {
            // Stop timer
            stateObjectDisposeTimer.Stop();

            // Update traffic info
            double dataIn = _totalBytesReceived;
            if (dataIn >= 1073741824.0)
            {
                // Display in GB
                dataIn = dataIn / 1073741824;
                tsslDataReceived.Text = String.Format("{0:0.000} GB", dataIn);
            }
            else if (dataIn >= 1048576.0)
            {
                // Display in MB
                dataIn = dataIn / 1048576;
                tsslDataReceived.Text = String.Format("{0:0.000} MB", dataIn);
            }
            else if (dataIn >= 1024.0)
            {
                // Display in KB
                dataIn = dataIn / 1024;
                tsslDataReceived.Text = String.Format("{0:0.000} KB", dataIn);
            }
            else
            {
                // Display in Byte
                tsslDataReceived.Text = String.Format("{0:0} Bytes", dataIn);
            }
            double dataOut = _totalBytesSent;
            if (dataOut >= 1073741824.0)
            {
                // Display in GB
                dataOut = dataOut / 1073741824;
                tsslDataSent.Text = String.Format("{0:0.000} GB", dataOut);
            }
            else if (dataOut >= 1048576.0)
            {
                // Display in MB
                dataOut = dataOut / 1048576;
                tsslDataSent.Text = String.Format("{0:0.000} MB", dataOut);
            }
            else if (dataOut >= 1024.0)
            {
                // Display in KB
                dataOut = dataOut / 1024;
                tsslDataSent.Text = String.Format("{0:0.000} KB", dataOut);
            }
            else
            {
                // Display in Byte
                tsslDataSent.Text = String.Format("{0:0} Bytes", dataOut);
            }

            // Clean up state object about every 10 seconds
            if (_timerCount >= 10)
            {
                // Number of state objects removed
                int nDisposed = 0;

                // Remove unused state objects
                for (int i = 0; i < _stateObjectList.Count; i++)
                {
                    // If the state object can be disposed, we'll remove it from the list
                    StateObject state = _stateObjectList[i];
                    if (state.CanDispose)
                    {
                        _stateObjectList.Remove(state);
                        i++;
                        nDisposed++;
                    }
                }

                Debug.WriteLine("Disposed " + nDisposed + " state object(s).");
                _timerCount = 0;
            }
            _timerCount++;

            stateObjectDisposeTimer.Start();
        }

        private void cbxSsl_CheckedChanged(object sender, EventArgs e)
        {
            _isSslEnabled = cbxSsl.Checked;
        }

        //private void GetAdvertisingInterval(AdvertiserInfo ai)
        //{
        //    #region Establish SSL connection to proxy server

        //    // Connect to server
        //    TcpClient client = new TcpClient(ai.IpAddress.ToString(), ai.Port);
        //    Debug.WriteLine("GetAdvertisingInterval: Connected to proxy server.");
        //    // Get SSL Stream
        //    SslStream stream = new SslStream(client.GetStream(), false,
        //                  new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
        //    stream.AuthenticateAsClient("bbg.gov", null, System.Security.Authentication.SslProtocols.Tls, false);
        //    Debug.WriteLine("GetAdvertisingInterval: Established an SSL connection.");

        //    #endregion

        //    #region Socks4 greeting

        //    // Greeting request
        //    byte[] greeting = new byte[9];
        //    greeting[0] = 0x04; // Socks version 4
        //    greeting[1] = 0x01; // Command code - TCP/IP stream
        //    // Port
        //    byte[] portByte = BitConverter.GetBytes((short)ai.Port);
        //    Array.Reverse(portByte);
        //    Array.Copy(portByte, 0, greeting, 2, 2);
        //    // IP Address
        //    Array.Copy(ai.IpAddress.GetAddressBytes(), 0, greeting, 4, 4);
        //    // User 
        //    greeting[8] = 0x00;

        //    // Send greeting
        //    stream.Write(greeting, 0, 9);

        //    // Wait for response
        //    bool isSocksHandshakeDone = false;
        //    byte[] socksReply = new byte[2048];
        //    byte[] reply = new byte[2048];
        //    int totalBytesCount = 0;
        //    while (!isSocksHandshakeDone)
        //    {
        //        // Read data from stream
        //        int bytesCount = stream.Read(socksReply, 0, 2048);
        //        // Check if too much data was received
        //        if (bytesCount + totalBytesCount > 2048)
        //        {
        //            // Too much data, we'll terminate
        //            stream.Close();
        //            return;
        //        }
        //        // Check if we have enough data to continue
        //        if (bytesCount > 0)
        //        {
        //            // Copy stream data to "reply"
        //            Array.Copy(socksReply, 0, reply, totalBytesCount, bytesCount);
        //            // Increment totalBytesCount
        //            totalBytesCount += bytesCount;
        //            // Do we have enough data?
        //            if (totalBytesCount > 2)
        //            {
        //                // We have enough data to move on
        //                // Check the 2nd byte
        //                if (reply[1] != 0x5a)
        //                {
        //                    // Request denied
        //                    // Close stream and return
        //                    stream.Close();
        //                    return;
        //                }
        //            }
        //        }
        //    }

        //    #endregion

        //    #region HTTP exchange

        //    #endregion
        //}
    }
}
