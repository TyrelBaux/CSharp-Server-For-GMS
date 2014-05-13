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
