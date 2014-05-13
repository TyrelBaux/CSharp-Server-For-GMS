using System.Threading;

namespace GMS_Server {
	public static class MainServer {
		/// <summary>Initiates and keeps the server running.</summary>
		public static void Main() {
			// Settings for both the TCP and UDP processing threads.
			string myIP = "127.0.0.1";
			int myPort = 64198;
			int myMaxClients = 256;
			int myBufferReadSize = 256;
			int myBufferWriteSize = 256;
			int myBufferAlignment = 1;
			int myPacketHeader = 137;

			// Creates a new thread for processing transmision control protocol(TCP).
			// Remove the three lines of code below if you do not wish to use a based TCP server.
			Thread myTcpThread = new Thread( () => TcpNetwork.TcpStart( myIP , myPort , myMaxClients , myBufferReadSize , myBufferWriteSize , myBufferAlignment , myPacketHeader ) );
			myTcpThread.IsBackground = false;
			myTcpThread.Start();
			Thread.Sleep( 100 );

			// Creates a new thread for processing user datagram protocol(UDP).
			// Remove the three lines of code below if you do not wish to use a based UDP server.
			Thread myUdpThread = new Thread( () => UdpNetwork.UdpStart( myPort , myBufferReadSize , myBufferWriteSize , myBufferAlignment , myPacketHeader ) );
			myUdpThread.IsBackground = false;
			myUdpThread.Start();
			Thread.Sleep( 100 );

			// Keeps the server from closing while the TCP and UDP threads are running.
			// However this will not keep the server from unexpectedly closing.
			// Also adds a command input and console stack for user input and request.
			CmdNetwork.StartCommands();

			while( UdpNetwork.UdpServerIsOnline == true || TcpNetwork.TcpServerIsOnline == true ) {
				string myCommand = System.Console.ReadLine();
				CmdNetwork.RunCommand( myCommand );
			}

			CmdNetwork.EndCommands();
		}
	}
}
