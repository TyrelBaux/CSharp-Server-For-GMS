namespace GMS_Server {
	static class ByteBuffer {
		private static int myAlign = 0;
		private static ushort myPeek = 0;
		private static int myEndType = 0;
		
		public static byte Buffer_Start( ref byte[] myBuffer , int myByteAlign , int myOffset ) {
			myPeek = ( ushort ) myOffset;
			myAlign = myByteAlign;
			byte myHeader = Buffer_Readu8( ref myBuffer );
			return myHeader;
		}
		
		public static void Buffer_SetOffset( int myOffset ) {
			myPeek = ( ushort ) myOffset;
		}
		
		public static int Buffer_GetOffset() {
			return ( int ) myPeek;
		}
		
		public static byte Buffer_End( ref byte[] myBuffer ) {
			myPeek -= ( ushort ) myEndType;
			byte myHeader = Buffer_Readu8( ref myBuffer );
			return myHeader;
		}
		
		public static byte Buffer_Readu8( ref byte[] myBuffer ) {
			byte result = myBuffer[ myPeek ];
			myPeek += ( ushort ) ( ( 1 >= myAlign ) ? 1 : myAlign );
			myEndType = ( ( 1 >= myAlign ) ? 0 : myAlign - 1 );
			return result;
		}
		
		public static sbyte Buffer_Reads8( ref byte[] myBuffer ) {
			sbyte result = ( sbyte ) myBuffer[ myPeek ];
			myPeek += ( ushort ) ( ( 1 >= myAlign ) ? 1 : myAlign );
			myEndType = ( ( 1 >= myAlign ) ? 0 : myAlign - 1 );
			return result;
		}
		
		public static ushort Buffer_Readu16( ref byte[] myBuffer ) {
			ushort result = ( ushort ) ( myBuffer[ myPeek ] + ( myBuffer[ myPeek + 1 ] << 8 ) );
			myPeek += ( ushort ) ( ( 2 >= myAlign ) ? 2 : myAlign );
			myEndType = ( ( 2 >= myAlign ) ? 0 : myAlign - 2 );
			return result;
		}
		
		public static short Buffer_Reads16( ref byte[] myBuffer ) {
			short result = ( short ) ( myBuffer[ myPeek ] + ( myBuffer[ myPeek + 1 ] << 8 ) );
			myPeek += ( ushort ) ( ( 2 >= myAlign ) ? 2 : myAlign );
			myEndType = ( ( 2 >= myAlign ) ? 0 : myAlign - 2 );
			return result;
		}
		
		public static uint Buffer_Readu32( ref byte[] myBuffer ) {
			uint result = ( uint ) ( myBuffer[ myPeek ] + ( myBuffer[ myPeek + 1 ] << 8 ) + ( myBuffer[ myPeek + 2 ] << 16 ) + ( myBuffer[ myPeek + 3 ] << 24 ) );
			myPeek += ( ushort ) ( ( 4 >= myAlign ) ? 4 : myAlign );
			myEndType = ( ( 4 >= myAlign ) ? 0 : myAlign - 4 );
			return result;
		}
		
		public static int Buffer_Reads32( ref byte[] myBuffer ) {
			int result = ( int ) ( myBuffer[ myPeek ] + ( myBuffer[ myPeek + 1 ] << 8 ) + ( myBuffer[ myPeek + 2 ] << 16 ) + ( myBuffer[ myPeek + 3 ] << 24 ) );
			myPeek += ( ushort ) ( ( ( 4 >= myAlign ) ? 4 : myAlign ) );
			myEndType = ( ( 4 >= myAlign ) ? 0 : myAlign - 4 );
			return result;
		}
		
		public static string Buffer_Readstr( ref byte[] myBuffer , int myStrLen ) {
			string result =  System.BitConverter.ToString( myBuffer , myPeek , myStrLen );
			myPeek += ( ushort ) ( ( myStrLen >= myAlign ) ? myStrLen : myAlign );
			myEndType = ( ( myStrLen >= myAlign ) ? 0 : myAlign - myStrLen );
			return result;
		}
		
		public static float Buffer_Readf32( ref byte[] myBuffer ) {
			float myValue = System.BitConverter.ToSingle( myBuffer , myPeek );
			myPeek += ( ushort ) ( ( 4 >= myAlign ) ? 4 : myAlign );
			myEndType = ( ( 4 >= myAlign ) ? 0 : myAlign - 4 );
			return myValue;
		}
		
		public static double Buffer_Readf64( ref byte[] myBuffer ) {
			double myValue = System.BitConverter.ToDouble( myBuffer , myPeek );
			myPeek += ( ushort ) ( ( 8 >= myAlign ) ? 8 : myAlign );
			myEndType = ( ( 8 >= myAlign ) ? 0 : myAlign - 8 );
			return myValue;
		}
		
		public static void Buffer_Writeu8( ref byte[] myBuffer , byte myValue ) {
			myBuffer[ myPeek ] = myValue;
			myPeek += ( ushort ) ( ( ( 1 >= myAlign ) ? 1 : myAlign ) );
		}
		
		public static void Buffer_Writes8( ref byte[] myBuffer , sbyte myValue ) {
			myBuffer[ myPeek ] = ( byte ) myValue;
			myPeek += ( ushort ) ( ( ( 1 >= myAlign ) ? 1 : myAlign ) );
		}
		
		public static void Buffer_Writeu16( ref byte[] myBuffer , ushort myValue ) {
			myBuffer[ myPeek ] = ( byte ) ( myValue >> 0 );
			myBuffer[ myPeek + 1 ] = ( byte ) ( myValue >> 8 );
			myPeek += ( ushort ) ( ( 2 >= myAlign ) ? 2 : myAlign );
		}
		
		public static void Buffer_Writes16( ref byte[] myBuffer , short myValue ) {
			myBuffer[ myPeek ] = ( byte ) ( myValue >> 0 );
			myBuffer[ myPeek + 1 ] = ( byte ) ( myValue >> 8 );
			myPeek += ( ushort ) ( ( 2 >= myAlign ) ? 2 : myAlign );
		}
		
		public static void Buffer_Writeu32( ref byte[] myBuffer , uint myValue ) {
			myBuffer[ myPeek ] = ( byte ) ( myValue >> 0 );
			myBuffer[ myPeek + 1 ] = ( byte ) ( myValue >> 8 );
			myBuffer[ myPeek + 2 ] = ( byte ) ( myValue >> 16 );
			myBuffer[ myPeek + 3 ] = ( byte ) ( myValue >> 24 );
			myPeek += ( ushort ) ( ( 4 >= myAlign ) ? 4 : myAlign );
		}
		
		public static void Buffer_Writes32( ref byte[] myBuffer , int myValue ) {
			myBuffer[ myPeek ] = ( byte ) ( myValue >> 0 );
			myBuffer[ myPeek + 1 ] = ( byte ) ( myValue >> 8 );
			myBuffer[ myPeek + 2 ] = ( byte ) ( myValue >> 16 );
			myBuffer[ myPeek + 3 ] = ( byte ) ( myValue >> 24 );
			myPeek += ( ushort ) ( ( 4 >= myAlign ) ? 4 : myAlign );
		}
		
		public static void Buffer_WriteStr( ref byte[] myBuffer , string myString ) {
			myBuffer[ myPeek ] = ( byte ) ( myString.Length >> 0 );
			myBuffer[ myPeek + 1 ] = ( byte ) ( myString.Length >> 8 );
			myPeek += 2;
			
			for( int i = 0; i < myString.Length; i ++ ) {
				myBuffer[ myPeek ] = ( byte ) myString[ i ];
				myPeek ++;
			}
			
			myPeek += ( ushort ) ( ( myString.Length >= myAlign ) ? 0 : myAlign - ( myString.Length ) );
		}
		
		public static void Buffer_Writef32( ref byte[] myBuffer , float myValue , ref byte myAlign ) {
			byte[] myBytes = System.BitConverter.GetBytes( myValue );
			
			for( int i = 0; i < myBytes.Length; i ++ ) {
				myBuffer[ myPeek ] = myBytes[ i ];
				myPeek ++;
			}
			
			myPeek += ( ushort ) ( ( 4 >= myAlign ) ? 0 : myAlign - 4 );
		}
		
		public static void Buffer_Writef64( ref byte[] myBuffer , float myValue , ref byte myAlign ) {
			byte[] myBytes = System.BitConverter.GetBytes( myValue );
			
			for( int i = 0; i < myBytes.Length; i ++ ) {
				myBuffer[ myPeek ] = myBytes[ i ];
				myPeek ++;
			}
			
			myPeek += ( ushort ) ( ( 8 >= myAlign ) ? 0 : myAlign - 8 );
		}
	}
}
