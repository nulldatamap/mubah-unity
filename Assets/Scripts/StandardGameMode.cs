using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StandardGameMode : IGameMode {
  public Hero player;
  
  // Create all the needed CommanderModules
  public List<ICommander> CreateCommanders() {
    Assert.IsNotNull( player );

    List<ICommander> cmdrs = new List<ICommander>();
    cmdrs.Add( new LocalCommander( player ) );
    
    return cmdrs;
  }

  public void SetupGameState() {
    GameObject map = GameObject.Find( "PMap" );

    GameObject hero =
      GameObject.Instantiate( Resources.Load("PHero") ) as GameObject;

    player = hero.GetComponent<Hero>();

    hero.transform.parent = map.transform;

  }
}
