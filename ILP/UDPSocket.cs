using com.espertech.esper.client;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ILP
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        private EPServiceProvider _epService;

        /// <summary>
        /// ToDo!!
        /// </summary>
        public UDPSocket(EPServiceProvider TmpEpService)
        {
            _epService = TmpEpService;
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Server_Robot(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            ReceiveFromRobot();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Server_Train(string address, int port)
        {
            Console.WriteLine("Server is starting...");
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            ReceiveFromVirtSensor();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Server_Printer(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            ReceiveFromPrinter();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Client(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                //Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }

        /// <summary>
        /// ToDo!!
        /// </summary>
        private void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    //Console.WriteLine("RECV: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }

            }, state);
        }

        /// <summary>
        /// Receive UDP stream from Robot and send as Event to runtime Engine
        /// </summary>
        private void ReceiveFromRobot()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    //Console.WriteLine("RECV ROBOT: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));

                    _epService.EPRuntime.SendEvent(new RobotCurrentEvent(float.Parse(Encoding.ASCII.GetString(so.buffer, 0, bytes))));
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }

            }, state);
        }

        /// <summary>
        /// Receive UDP stream from Robot and send as Event to runtime Engine
        /// </summary>
        private void ReceiveFromVirtSensor()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    //Console.WriteLine("RECV Virtual Sensor: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    //Console.WriteLine("RECV Converted: " + double.Parse(Encoding.ASCII.GetString(so.buffer, 0, bytes), NumberStyles.Float, CultureInfo.InvariantCulture));
                    
                    _epService.EPRuntime.SendEvent(new TrainCurrentEvent(float.Parse(Encoding.ASCII.GetString(so.buffer, 0, bytes))));
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }
            }, state);
        }

        /// <summary>
        /// Receive UDP stream from Printer and send as Event to runtime engine
        /// </summary>
        private void ReceiveFromPrinter()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    //Console.WriteLine("RECV PRINTER: {0}: {1}, {2}", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));

                    _epService.EPRuntime.SendEvent(new PrinterCurrentEvent(float.Parse(Encoding.ASCII.GetString(so.buffer, 0, bytes))));
                }
                catch (System.ObjectDisposedException)
                {
                    return;
                }

            }, state);
        }

        public void EndReceiving()
        {
            _socket.Close();
        }
    }
}
