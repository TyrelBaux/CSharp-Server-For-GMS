using System.Net.Sockets;
using System.Net;
using System;

namespace GMS_Server {
	public static class UdpNetwork {
		public static bool UdpServerIsOnline = false;
		public static UdpClient UdpListeningSocket;
		public static IPAddress UdpIPAddress;
		public static int UdpListeningPort = 0;
		public static byte UdpPacketHeader = 0;

		/// <summary>Creates and starts a UDP client on the specified port.</summary>
		/// <param name="myUdpPort">Port to intiate the UDP client on.</param>
		/// <param name="myBufferWSize">Size of th UDP write buffer.</param>
		/// <param name="myBufferAlign">Alignment for the UDP read and write buffer.</param>
		public static void UdpStart( int myUdpPort , int myBufferRSize , int myBufferWSize , int myBufferAlign , int myHeader ) {
			UdpServerIsOnline = true;

			try {
				ByteBuffer myBufferW = new ByteBuffer();
				myBufferW.Create( myBufferWSize , myBufferAlign );

				ByteBuffer myBufferR = new ByteBuffer();
				myBufferR.Create( myBufferRSize , myBufferAlign );

				UdpListeningPort = myUdpPort;
				UdpPacketHeader = ( byte ) myHeader;
				UdpListeningSocket = new UdpClient( myUdpPort );
				UdpListeningSocket.AllowNatTraversal( true );

				UdpHandle( UdpListeningSocket , myBufferW , myBufferR );
			} catch( System.Exception ) {
				UdpServerIsOnline = false;
			}
		}

		/// <summary>Handles receiving data on the specified Port and IP of the UDP client.</summary>
		/// <param name="myUdpSocket">Socket of the UDP client to get the received data from.</param>
		/// <param name="myBufferW">Buffer that data will be written and sent when the UDP client sends data.</param>
		private static async void UdpHandle( UdpClient myUdpSocket , ByteBuffer myBufferW , ByteBuffer myBufferR ) {
			while( UdpServerIsOnline ) {
				try {
					if ( myUdpSocket.Available > 0 ) {
						Array.Clear( myBufferR.Buffer , 0 , myBufferR.Buffer.Length );
						Array.Clear( myBufferW.Buffer , 0 , myBufferW.Buffer.Length );
						myBufferR.BytePeek = 0;
						myBufferW.BytePeek = 0;

						UdpReceiveResult myUdpReceiver = await myUdpSocket.ReceiveAsync();
						Array.Copy( myUdpReceiver.Buffer , myBufferR.Buffer , myUdpReceiver.Buffer.Length );
						IPEndPoint myIPEndPoint = myUdpReceiver.RemoteEndPoint;
						UdpPackets.UdpPacketRead( ref myIPEndPoint , ref myBufferR , ref myBufferW );
					}

					UdpPackets.UdpPacketSend( ref myBufferW );
				} catch( System.Exception ) {
					UdpServerIsOnline = false;
				}
			}

			myUdpSocket.Close();
		}
	}
}
