using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StandardGameMode : IGameMode {
  public Hero player;
  
  public void SetupGameState() {
    GameObject map = GameObject.Find( "PMap" );

    GameObject hero =
      GameObject.Instantiate( Resources.Load("PHero") ) as GameObject;

    player = hero.GetComponent<Hero>();

    hero.transform.parent = map.transform;

  }
}
