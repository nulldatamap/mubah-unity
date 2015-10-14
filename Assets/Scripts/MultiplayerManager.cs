using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

// TODO: Implement
struct Snapshot {}

enum MultiplayerState {
  Disconnected,
  Connecting,
  WaitingForClient,
  Ready
}

class Recv {
  // Make this configurable
  byte[] buffer;
  int chanId;
  int recvSize;
  int conId;
  NetworkError error;

  NetworkEventType msg;

  bool handledData = false;
  bool handledConnect = false;
  bool handledDisconnect = false;
  bool handledError = false;

  public Recv( int hid ) {
    // TODO: Make the buffer size configurable
    buffer = new byte[1024];
    byte err;

    msg = NetworkTransport.ReceiveFromHost( hid, out conId, out chanId, buffer
                                          , 1024, out recvSize, out err );

    error = (NetworkError)err;
  }

  public delegate void OnNothingf();
  public delegate void OnDataf( byte[] buffer, int len, int conId, int chanId );
  public delegate void OnConnectf( int conId );
  public delegate void OnDisconnectf( int conId );
  public delegate void OnErrorf( NetworkError err );

  public Recv Ignore( NetworkEventType t ) {
    switch( t ) {
      case NetworkEventType.ConnectEvent:
        handledConnect = true;
        break;
      case NetworkEventType.DisconnectEvent:
        handledDisconnect = true;
        break;
      case NetworkEventType.DataEvent:
        handledData = true;
        break;
    }
    return this;
  }

  public Recv OnNothing( OnNothingf f ) {
    if( msg == NetworkEventType.Nothing ) {
      f();
    }
    return this;
  }

  public Recv OnData( OnDataf f ) {
    handledData = true;
    if( msg == NetworkEventType.DataEvent ) {
      f( buffer, recvSize, conId, chanId );
    }
    return this;
  }

  public Recv OnConnect( OnConnectf f ) {
    handledConnect = true;
    if( msg == NetworkEventType.ConnectEvent ) {
      f( conId );
    }
    return this;
  }

  public Recv OnDisconnect( OnDisconnectf f ) {
    handledDisconnect = true;
    if( msg == NetworkEventType.DisconnectEvent ) {
      f( conId );
    }
    return this;
  }

  public Recv OnError( OnErrorf f ) {
    handledError = true;
    if( error != NetworkError.Ok ) {
      f( error );
    }
    return this;
  }

  public Recv ErrorOnRest() {
    if( error != NetworkError.Ok && !handledError ) {
      throw new InvalidOperationException( "Got unhandled recveive error: " + error );
    }
    
    if( msg == NetworkEventType.ConnectEvent && !handledConnect ) {
      throw new InvalidOperationException( "Got unhandled connect event");
    }

    if( msg  == NetworkEventType.DisconnectEvent && !handledDisconnect ) {
      throw new InvalidOperationException( "Got unhandled disconnect event" );
    }

    if( msg  == NetworkEventType.DataEvent && !handledData ) {
      throw new InvalidOperationException( "Got unhandled data event" );
    }
    return this;
  }
}

public class MutliplayerManager {
  
  int mainChannel;
  HostTopology topology;
  int hostId;
  int connectionId;

  // TODO: Don't actually directly expose these
  public bool running = false;

  Dictionary<int, Snapshot> clientSnapshots = new Dictionary<int,Snapshot>();
  
  MultiplayerState state = MultiplayerState.Disconnected;

	public MutliplayerManager() {
    // Packet config
    GlobalConfig gConfig = new GlobalConfig();
    gConfig.MaxPacketSize = 500;
    
    NetworkTransport.Init( gConfig );

    // Connection config
    ConnectionConfig config = new ConnectionConfig();
    int mainChannel = config.AddChannel(QosType.Unreliable);
    
    topology = new HostTopology( config, 10 );
  }

  public void LocalHost() {
    hostId = NetworkTransport.AddHost( topology, 4114, "127.0.0.1" );
    // TODO: Setup client information
    running = true;
    // Wait for a client
    state = MultiplayerState.WaitingForClient;
  }
  
  public void LocalConnect() {
    hostId = NetworkTransport.AddHost( topology );
    
    byte error;
    int cid = NetworkTransport.Connect( hostId, "127.0.0.1", 4114, 0, out error );

    if( (NetworkError)error != NetworkError.Ok ) {
      Debug.LogError( "Failed to connect: " + (NetworkError)error );
      return;
    }
    running = true;
    state = MultiplayerState.Connecting;
    connectionId = cid;
  }

  public void CheckConnectionAccepted() {
    if( state != MultiplayerState.Connecting ) {
      throw new InvalidOperationException( "Expected `Connecting` state for `CheckConnectionAccepted`" );
    }

    new Recv( hostId )
      .OnError( err => {
        Debug.LogError( "Failed to receive: " + (NetworkError)err );
        return;
      } )
      .OnConnect( conId => {
        if( conId == connectionId ) {
          state = MultiplayerState.Ready;
          Debug.LogError( "Client connected" );
        } else {
          // TODO: handle this error
          byte _err;
          NetworkTransport.Disconnect( hostId, conId, out _err );
        }
      } )
      .OnDisconnect( conId => {
        if( connectionId == conId ) {
          state = MultiplayerState.Disconnected;
          return;
        }

        throw new InvalidOperationException( "Got `DisconnectEvent` in client state" );
      } )
      .ErrorOnRest();
  }

  public void CheckClientConnected() {
    if( state != MultiplayerState.WaitingForClient ) {
      throw new InvalidOperationException( "Expected `WaitingForClient` state in `CheckClientConnected`" );
    }

    new Recv( hostId )
      .OnError( err => {
        Debug.LogError( "Failed to receive: " + (NetworkError)err );
      } )
      .OnConnect( conId => {
        // TODO: Actually register the connected player
        state = MultiplayerState.Ready;
        Debug.LogError( "Connected to host" );
      } )
      .ErrorOnRest();
  }


  public void Update() {
    switch( state ) {
      case MultiplayerState.Disconnected:
        break;
      case MultiplayerState.Connecting:
        CheckConnectionAccepted();
        break;
      case MultiplayerState.WaitingForClient:
        CheckClientConnected();
        break;
      case MultiplayerState.Ready:
        // TODO
        break;
    }
  }
}
