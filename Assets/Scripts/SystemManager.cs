using UnityEngine;
using System.Collections.Generic;

public class SystemManager : MonoBehaviour {

  IGameMode gameMode = new StandardGameMode();

  List<ICommander> commanders = new List<ICommander>();

  void Start() {
    gameMode.SetupGameState();

    commanders = gameMode.CreateCommanders();
  }

  void Update() {
    foreach( ICommander cm in commanders ) {
      cm.EmitCommands();
    }
  }
}
