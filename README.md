CSharp-Server-For-GMS
=====================

Special dedicated CSharp server source made for GameMaker: Studio users!
Below is a brief explanation of each class file and it's importance:

ByteBuffer.cs | Buffer Class File:
    This is a class file containing the byte buffer system. Due to the complete isolation of this class file
    you can even paste the class into any other project that requires use of buffers. The buffer system allows
    you to instantiate the class in order to create multiple buffers. Example instantiation of a buffer:

        // NOTE: The ByteBuffer.Create() method creates the actual useable buffer. However if used multiple times
        //       the method will simply create a new version of the buffer. It will not create a multiple buffers.
        ByteBuffer myBuffer = new ByteBuffer();
        myBuffer.Create( BufferSize );

CmdNetwork.cs | Command Input File:
    The CmdNetwork class file allows you to add an error log to your application as well as perform commands for
    assistance with running your server.

MainServer.cs | Primary Server File:
    Inside this class file, is the main code for creating the server and keeping it running. This is where you can
    add/remove TCP or UDP support for your server. This is also where the main method execution for user inputted
    commands happens. The class file does have both TCP and UDP networks preset. Here is an example of adding a TCP
    server network to your application from scratch:
    
	Thread myTcpThread = new Thread( () => TcpNetwork.TcpStart( myIP , myPort , myMaxClients , myBufferReadSize , myBufferWriteSize , myBufferAlignment , myPacketHeader ) );
	myTcpThread.IsBackground = false;
	myTcpThread.Start();
	Thread.Sleep( 100 );

    Now here is how it's done for UDP from scratch:
    
	Thread myUdpThread = new Thread( () => UdpNetwork.UdpStart( myPort , myBufferReadSize , myBufferWriteSize , myBufferAlignment , myPacketHeader ) );
	myUdpThread.IsBackground = false;
	myUdpThread.Start();
	Thread.Sleep( 100 );
