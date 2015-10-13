using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Assertions;
using System.Collections.Generic;

// TODO: Add a network manager

public class SystemManager : NetworkManager {

  public IGameMode gameMode = new StandardGameMode();

  public CameraController cameraController;

  public static SystemManager Get() {
    return GameObject.Find( "PSystem" ).GetComponent<SystemManager>();
  }

  void Start() {
    Assert.IsNotNull( cameraController );
  }

  void Update() {
  }

  /*override public void OnStartServer() {
    // Setup the word without players
  }

  override public void OnServerConnect( NetworkConnection conn ) {
    
  }

  override public void OnServerAddPlayer( NetworkConnection conn, short cid ) {

  }

  override public void OnStartClient( NetworkClient client ) {

  }*/

  override public void OnClientConnect( NetworkConnection conn ) {
    ClientScene.AddPlayer( conn, 0 );
  }

}

