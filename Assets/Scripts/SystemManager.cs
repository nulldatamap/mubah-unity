using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Assertions;
using System.Collections.Generic;

// TODO: Add a network manager

public class SystemManager : MonoBehaviour {

  public MutliplayerManager multiplayerManager;
  public GameMode gameMode;
  public CameraController cameraController;

  public static SystemManager Get() {
    return GameObject.Find( "PSystem" ).GetComponent<SystemManager>();
  }

  void Start() {
    Assert.IsNotNull( cameraController );

    multiplayerManager = new MutliplayerManager();

    Debug.Log( "Q to host, W to connect" );
  }

  void Update() {
    if( !multiplayerManager.running ) {
      if( Input.GetKeyDown( KeyCode.Q ) ) {
        Debug.Log( "Hosting..." );
        multiplayerManager.LocalHost();
      } else if( Input.GetKeyDown( KeyCode.W ) ) {
        Debug.Log( "Connecting..." );
        multiplayerManager.LocalConnect();
      }
    } else {
      multiplayerManager.Update();
    }
  }

}

