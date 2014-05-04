using System.Net.Sockets;

namespace GMS_Server {
	static class TcpPackets {
		public async static void TcpPacketSend( NetworkStream myStream , byte[] myBuffer , int myPeek ) {
			//Write the write-buffer to the client's network stream and send the packet of data to the client;
			try {
				await myStream.WriteAsync( myBuffer , 0 , myPeek );
				await myStream.FlushAsync();
			} catch( System.Exception ) {}
		}
		
		public static void TcpPacketRead( ref NetworkStream myStream , ref byte[] myBufferR , ref byte[] myBufferW , ref bool myThreading ) {
			
			//-----------------------------------------------------------------------------------------------------------------------------------------------------
			//--------------------------------------------------DO NOT EDIT OUTSIDE OF THE CODE BLOCK BELOW!-------------------------------------------------------
			//-----------------------------------------------------------------------------------------------------------------------------------------------------
			
			var myCheck = ByteBuffer.Buffer_Start( ref myBufferR , 1 , 0 );

			//Continue reading appended packets of data in the buffer until no packets are left;
			while( myCheck == 1 ) {
				//Get the packet ID from the read-buffer;
				var myPacket = ByteBuffer.Buffer_Readu8( ref myBufferR );
				
				switch( myPacket ) {
					//--------------------------------------Test Packets--------------------------------------
					case 254:
						//Disconnection Packet ID;
						//If the client sends a disconnection packet, stop the client's thread to disconnect the client;
						myThreading = false;
					break;
					default:
						//Cannot Validate Packet by ID;
						//Show an error if the client sends a packet of data with an invalid packet ID;
						System.Console.WriteLine( "Invalid Packet ID" );
					break;
					//----------------------------------------------------------------------------------------
				}

				//Check the buffer for any appended TCP packets that might have merged;
				//This looks for the header of the next appended packet;
				if ( ByteBuffer.Buffer_GetOffset() < myBufferR.Length ) {
					myCheck = ByteBuffer.Buffer_End( ref myBufferR );
				}
				
				//-----------------------------------------------------------------------------------------------------------------------------------------------------
				//--------------------------------------------------DO NOT EDIT OUTSIDE OF THE CODE BLOCK ABOVE!-------------------------------------------------------
				//-----------------------------------------------------------------------------------------------------------------------------------------------------
			}
		}
	}
}
