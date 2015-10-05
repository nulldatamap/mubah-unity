using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class SystemManager : MonoBehaviour {

  public IGameMode gameMode = new StandardGameMode();

  List<ICommander> commanders = new List<ICommander>();

  public CameraController cameraController;

  public static SystemManager Get() {
    return GameObject.Find( "PSystem" ).GetComponent<SystemManager>();
  }

  void Start() {
    cameraController =
      GameObject.Find( "PCameraController" ).GetComponent<CameraController>();
    
    Assert.IsNotNull( cameraController );

    gameMode.SetupGameState();

    commanders = gameMode.CreateCommanders();
  }

  void Update() {
    foreach( ICommander cm in commanders ) {
      cm.EmitCommands();
    }
  }
}
