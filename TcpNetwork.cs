using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Net;

namespace GMS_Server {
	public static class TcpNetwork {
		public static bool TcpServerIsOnline = false;
		public static List<TcpClient> TcpSocketList;
		public static List<Thread> TcpThreadList;
		public static TcpListener TcpListeningSocket;
		public static IPAddress TcpIPAddress;
		public static int TcpListeningPort = 0;
		public static byte TcpPacketHeader = 0;
		private static int TcpBufferReadSize = 0;
		private static int TcpBufferWriteSize = 0;
		private static int TcpBufferAlignment = 1;
		private static int TcpMaxClients = 0;
		
		/// <summary>Creates and starts the TCP listener(server) on the specified IP address and port.</summary>
		/// <param name="myTcpIP">IP address of the TCP listener(server).</param>
		/// <param name="myTcpPort">Port of the TCP listener(server).</param>
		/// <param name="myMaxClients">Max number of clients to accept before rejecting client connections.</param>
		/// <param name="myBufferRSize">Size of the read buffer.</param>
		/// <param name="myBufferWSize">Size of the write buffer.</param>
		/// <param name="myBufferAlign">Byte alignment of both the read and write buffer.</param>
		public static void TcpStart( string myTcpIP , int myTcpPort , int myMaxClients , int myBufferRSize , int myBufferWSize , int myBufferAlign , int myHeader ) {
			TcpServerIsOnline = true;
			TcpBufferAlignment = myBufferAlign;

			try {
				TcpMaxClients = myMaxClients;
				TcpPacketHeader = ( byte ) myHeader;
				TcpIPAddress = IPAddress.Parse( myTcpIP );
				TcpListeningPort = myTcpPort;
	
				TcpSocketList = new List<TcpClient>( myMaxClients );
				TcpThreadList = new List<Thread>( myMaxClients );
				TcpListeningSocket = new TcpListener( TcpIPAddress , TcpListeningPort );

				TcpBufferReadSize = myBufferRSize;
				TcpBufferWriteSize = myBufferWSize;
				TcpListeningSocket.Start( myMaxClients );
				TcpAccept();
			} catch( Exception ) {
				TcpServerIsOnline = false;
			}
		}

		/// <summary>Searches for and accepts clients attempting to connect to the server.</summary>
		private static async void TcpAccept() {
			while(  TcpServerIsOnline ) {
				try {
					if ( TcpListeningSocket.Pending() == true ) {
						TcpClient myClient = await TcpListeningSocket.AcceptTcpClientAsync();
						myClient.LingerState = new LingerOption( true , 0 );
						TcpSocketList.Add( myClient );
						myClient.NoDelay = true;
						
						Thread myThread = new Thread( () => TcpNetwork.TcpHandle( myClient ) );
						TcpThreadList.Add( myThread );
						myThread.Start();
					}
				} catch( Exception ) {}
			}

			TcpNetwork.TcpExit();
		}

		/// <summary>Handles all client data and receiving data from the client. The client will be sent a disconnection message(id 254) if the
		/// the client has connected when the server has reached maximum capacity(MaxClients).</summary>
		/// <param name="myClient">Socket of the specific TCP client.</param>
		private static async void TcpHandle( TcpClient myClient ) {
			NetworkStream myStream = myClient.GetStream();

			ByteBuffer myBufferR = new ByteBuffer();
			myBufferR.Create( TcpBufferReadSize , TcpBufferAlignment );

			ByteBuffer myBufferW = new ByteBuffer();
			myBufferW.Create( TcpBufferWriteSize , TcpBufferAlignment );

			if ( TcpSocketList.Count <= TcpMaxClients ) {
				CmdNetwork.AppendLog( CmdNetwork.CmdClock.ElapsedMilliseconds.ToString() + " : Client : " + TcpSocketList.FindIndex( x => x == myClient ).ToString() + " Connected;" );
				bool myThreading = true;
	
				while( myThreading ) {
					try {
						if ( myStream.DataAvailable == true ) {
							Array.Clear( myBufferR.Buffer , 0 , TcpBufferReadSize );
							Array.Clear( myBufferW.Buffer , 0 , TcpBufferWriteSize );
							myBufferR.BytePeek = 0;
							myBufferW.BytePeek = 0;
	
							int mySize = myClient.Available;
							await myStream.ReadAsync( myBufferR.Buffer , 0 , mySize );
							IPEndPoint myIPEndPoint = ( IPEndPoint ) myClient.Client.RemoteEndPoint;
	
							TcpPackets.TcpPacketRead( ref myIPEndPoint , ref myStream , ref myThreading , ref myBufferR , ref myBufferW );
						}

						TcpPackets.TcpPacketSend( ref myStream , ref myThreading , ref myBufferW );
					} catch( Exception ) {
						myThreading = false;
					}
	
					if ( myClient.Connected == false ) {
						myThreading = false;
					}
				}
				
				CmdNetwork.AppendLog( CmdNetwork.CmdClock.ElapsedMilliseconds.ToString() + " : Client: " + TcpSocketList.FindIndex( x => x == myClient ).ToString() + " Disconnected;" );
			} else {
				myBufferW.Writeu8( TcpPacketHeader );
				myBufferW.Writeu8( 254 );
				myBufferW.SendTcp( myStream );
			}

			int myID = TcpSocketList.FindIndex( x => x == myClient );
			TcpThreadList.RemoveAt( myID );
			TcpSocketList.RemoveAt( myID );

			myStream.Close();
			myClient.Close();
		}

		/// <summary>Finalizes closing the server properly.</summary>
		private static void TcpExit() {
			foreach( Thread myThread in TcpThreadList ) {
				myThread.Abort();
			}

			foreach( TcpClient myClient in TcpSocketList ) {
				myClient.GetStream().Close();
				myClient.Close();
			}

			TcpServerIsOnline = false;
			TcpListeningSocket.Stop();
		}
	}
}
