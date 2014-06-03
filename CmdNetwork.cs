using System;

namespace GMS_Server {
	public static class CmdNetwork {
		public static System.Collections.Generic.List<string> CmdLog;
		public static System.Diagnostics.Stopwatch CmdClock;

		/// <summary>Initiates the command network stack.</summary>
		public static void StartCommands() {
			CmdLog = new System.Collections.Generic.List<string>();
			CmdClock = new System.Diagnostics.Stopwatch();
			CmdClock.Start();
		}

		/// <summary>Stops the command network stack.</summary>
		public static void EndCommands() {
			CmdClock.Stop();
			CmdLog.Clear();
		}

		/// <summary>Adds a server log line to the end of the command stack.</summary>
		/// <param name="myLog">Log message to add to the command stack.</param>
		public static void AppendLog( string myLog ) {
			CmdLog.Add( CmdNetwork.CmdClock.ElapsedMilliseconds.ToString() + " : " + myLog );

			if ( CmdLog.Count >= 100 ) {
				CmdLog.RemoveAt( 0 );
			}
		}

		/// <summary>Executes a command based on master user input.</summary>
		/// <param name="myCommand">Command as string to execute.</param>
		public static void RunCommand( string myCommand ) {
			if ( myCommand.ToLower() != null && CmdClock.IsRunning == true ) {
				switch( myCommand ) {
					case "help": case "?":
						Console.WriteLine( "help or ? : show command list:" );
						Console.WriteLine( "server.log : shows server log(100 messages max)." );
					break;
					case "server.log":
						Console.WriteLine( "-- Server Log --" );
						
						foreach( string myLog in CmdLog ) {
							Console.WriteLine( myLog );
						}

						Console.WriteLine( "----------------" );
					break;
					case "server.tcpstop":
						TcpNetwork.TcpServerIsOnline = false;
					break;
					case "server.udpstop":
						UdpNetwork.UdpServerIsOnline = false;
					break;
					case "server.count":
						Console.WriteLine( "(TCP) Total Clients: " + TcpNetwork.TcpSocketList.Count.ToString() );
					break;
					case "server.tcpstat":
						Console.WriteLine( "(TCP) Server Status: " + TcpNetwork.TcpServerIsOnline.ToString() );
					break;
					case "server.udpstat":
						Console.WriteLine( "(UDP) Server Status: " + UdpNetwork.UdpServerIsOnline.ToString() );
					break;
					case "server.clearlog":
						CmdLog.Clear();
					break;
					default:
						Console.WriteLine( "Error: Invalid Command Input." );
					break;
				}
			}
		}
	}
}
