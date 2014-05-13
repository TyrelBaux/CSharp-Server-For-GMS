using System.Net.Sockets;

namespace GMS_Server {
	public static class UdpPackets {
		/// <summary>(returns void) Reads the received data from the network receiver of the UDP client.</summary>
		/// <param name="myEndPoint">Information about the received packet: sender ip, port, etc.</param>
		/// <param name="myBufferR">Buffer containing the received data.</param>
		/// <param name="myBufferW">Buffer used for sending data.</param>
		public static void UdpPacketRead( ref System.Net.IPEndPoint myEndPoint , ref ByteBuffer myBufferR , ref ByteBuffer myBufferW ) {
			var myCheck = myBufferR.StartRead( 12 );

			while( myCheck == UdpNetwork.UdpPacketHeader ) {
				var myPacket = myBufferR.Readu8();

				switch( myPacket ) {
					case 0:
						// Empty sample messagewith an ID of 0.
					break;
					default:
						// This case executes when a message with an invalid packet ID is received.
					break;
				}

				myCheck = ( byte ) ( ( myBufferR.GetPeek() < myBufferR.GetCount() ) ? myBufferR.EndRead( false ) : 0 );
			}
		}

		/// <summary>Allows for sending data independently of receiving data.</summary>
		/// <param name="myBufferW">Buffer used for sending data.</param>
		public static void UdpPacketSend( ref ByteBuffer myBufferW ) {
			
		}
	}
}
