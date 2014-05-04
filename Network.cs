//Include necessary namespaces;
using System;
using System.Net.Sockets;
using System.Threading;

//Main server system namespace;
namespace GMS_Server {
	//Initiates network TCP server assets;
	public static class NetworkVars {
		public static bool TcpServerIsOnline = false;
		public static System.Collections.Generic.List<TcpClient> TcpSocketList;
		public static System.Collections.Generic.List<Thread> TcpThreadList;
		public static TcpListener TcpListeningSocket;
		public static System.Net.IPAddress TcpIPAddress;
		public static int TcpListeningPort = 0;
		public static int TcpBufferReadSize = 0;
		public static int TcpBufferWriteSize = 0;
	}
	
	public class Network {
		
		public void TcpStart( string AccessIP , int AccessPort , int MaxClients , int BufferRSize , int BufferWSize ) {
			//Initiate TCP listener server assets;
			try {
				NetworkVars.TcpIPAddress = System.Net.IPAddress.Parse( AccessIP );
				NetworkVars.TcpListeningPort = AccessPort;
	
				NetworkVars.TcpSocketList = new System.Collections.Generic.List<TcpClient>( MaxClients );
				NetworkVars.TcpThreadList = new System.Collections.Generic.List<Thread>( MaxClients );
				NetworkVars.TcpListeningSocket = new TcpListener( NetworkVars.TcpIPAddress , NetworkVars.TcpListeningPort );
				NetworkVars.TcpBufferReadSize = BufferRSize;
				NetworkVars.TcpBufferWriteSize = BufferWSize;
				NetworkVars.TcpListeningSocket.Start( MaxClients );
				TcpAccept();
			} catch( Exception ) {
				Console.WriteLine( "Failed To Initiate Server!" );
			}
		}
		
		public async void TcpAccept() {
			//Perform main server functions when server is online;
			while(  NetworkVars.TcpServerIsOnline ) {
				try {
					if ( NetworkVars.TcpListeningSocket.Pending() == true ) {
						//Asynchronously accept the new client;
						TcpClient myClient = await NetworkVars.TcpListeningSocket.AcceptTcpClientAsync();
						myClient.LingerState = new System.Net.Sockets.LingerOption( true , 0 );
						NetworkVars.TcpSocketList.Add( myClient );
						myClient.NoDelay = true;
							
						//Create new thread for this client to handle client data sending / receiving / processing;
						Thread myThread = new Thread( () => TcpHandle( myClient ) );
						NetworkVars.TcpThreadList.Add( myThread );
						myThread.Start();
						
						Console.WriteLine( "Client: " + Convert.ToString( NetworkVars.TcpSocketList.Count - 1 ) + " connected!" );
					}
				} catch( Exception ex ) {
					//Throw an exception if there was a problem accepting a client and stop the server;
					Console.WriteLine( ex.ToString() );
				}
			}
			
			TcpExit();
		}
		
		public async void TcpHandle( TcpClient myClient ) {
			//Accept any packets of data received from the client :: Create client threading assets;
			NetworkStream myStream = myClient.GetStream(); //Client's Network Stream For Data Handling;
			byte[] myBufferR = new byte[ NetworkVars.TcpBufferReadSize ]; //Read-Buffer;
			byte[] myBufferW = new byte[ NetworkVars.TcpBufferWriteSize ]; //Write-Buffer;
			bool myThreading = true; //Allow Thread To Process;
			myClient.NoDelay = true;
			
			while( myThreading ) {
				//Check for any available packets from the client's network stream;
				try {
					if ( myStream.DataAvailable == true ) {
						//Read the packet of data from the client's network stream into the read-buffer if data was found;
						Array.Clear( myBufferR , 0 , NetworkVars.TcpBufferReadSize );
						Array.Clear( myBufferW , 0 , NetworkVars.TcpBufferWriteSize );
						int mySize = myClient.Available;
						await myStream.ReadAsync( myBufferR , 0 , mySize );

						//Process the packet of data read from the stream;
						TcpPackets.TcpPacketRead( ref myStream , ref myBufferR , ref myBufferW , ref myThreading );
					}
				} catch( Exception ) {
					Console.WriteLine( "-- TCP Receive Buffer Error --" );
					Console.WriteLine( "   Received Data Is Too Large For The Receive Buffer" );
					Console.WriteLine( "   Packet Of Data Discarded" );
					Console.WriteLine( "   Client Disconnected" );
					myThreading = false;
				}
				
				Array.Clear( myBufferW , 0 , NetworkVars.TcpBufferWriteSize );
				ByteBuffer.Buffer_SetOffset( 0 );
				ByteBuffer.Buffer_Writeu8( ref myBufferW , ( byte ) 253 );
				TcpPackets.TcpPacketSend( myStream , myBufferW , ByteBuffer.Buffer_GetOffset() );

				if ( myClient.Connected == false ) {
					myThreading = false;
				}
			}
			
			//If the thread for the client is ended, delete tcpclient and thread enteries and close the thread;
			int myID = NetworkVars.TcpSocketList.FindIndex( x => x == myClient );
			Console.WriteLine( "Client: " + Convert.ToString( myID ) + " disconnected!" );
			NetworkVars.TcpThreadList.RemoveAt( myID );
			NetworkVars.TcpSocketList.RemoveAt( myID );
			myStream.Close();
			myClient.Close();
		}
		
		public void TcpExit() {
			//Abort all client threads;
			foreach( Thread myThread in NetworkVars.TcpThreadList ) {
				myThread.Abort();
			}
			
			//Close all client connections;
			foreach( TcpClient myClient in NetworkVars.TcpSocketList ) {
				myClient.GetStream().Close();
				myClient.Close();
			}
			
			//Stop server TCP listener socket;
			NetworkVars.TcpListeningSocket.Stop();
		}
	}
	
	public class Net_Server {
		///Main program for the Tcp Server;
		public static void Main() {
			Console.WriteLine( "Server IP Address?" );
			string myIP = Console.ReadLine();
			
			Network myNetwork = new Network();
			Thread myTcpThread = new Thread( () => myNetwork.TcpStart( myIP , 64198 , 256 , 256 , 256 ) );
			NetworkVars.TcpServerIsOnline = true;
			myTcpThread.Start();
			
			while( myTcpThread.IsAlive == false ) {
				NetworkVars.TcpServerIsOnline = false;
			}
		}
	}
}
