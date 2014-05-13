using System.Net.Sockets;
using System.Net;

namespace GMS_Server {
	public static class TcpPackets {
		/// <summary>Reads the received data from the network stream of the TCP client.</summary>
		/// <param name="myIPEndPoint">Information about the received packet: sender ip, port, etc.</param>
		/// <param name="myStream">The network stream used for sending data back to the client that the data was received from.</param>
		/// <param name="myThreading">Whether or not to keep the specific TCP client connected via the client's thread.</param>
		/// <param name="myBufferR">Buffer containing the received data.</param>
		/// <param name="myBufferW">Buffer used for sending data.</param>
		public static void TcpPacketRead( ref IPEndPoint myIPEndPoint , ref NetworkStream myStream , ref bool myThreading , ref ByteBuffer myBufferR , ref ByteBuffer myBufferW ) {
			var myCheck = myBufferR.StartRead( 0 );

			while( myCheck == TcpNetwork.TcpPacketHeader ) {
				var myPacket = myBufferR.Readu8();

				switch( myPacket ) {
					case 254:
						// Client disconnection message.
						// Make sure to send this message on the client to the server in order for the server to register that the client disconnected.
						myThreading = false;
					break;
					default:
						// This case executes when a message with an invalid packet ID is received.
					break;
				}

				myCheck = ( byte ) ( ( myBufferR.GetPeek() < myBufferR.GetCount() ) ? myBufferR.EndRead( true ) : 0 );
			}
		}

		/// <summary>Allows for sending data independently of receiving data.</summary>
		/// <param name="myStream">The network stream used for sending data to the client.</param>
		/// <param name="myBufferW">Buffer used for sending data.</param>
		public static void TcpPacketSend( ref NetworkStream myStream , ref bool myThreading , ref ByteBuffer myBufferW ) {
			
		}
	}
}
