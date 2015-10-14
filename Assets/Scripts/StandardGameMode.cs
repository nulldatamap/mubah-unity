using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StandardGameMode : GameMode {
  
   public StandardGameMode( bool host ) : base( host ) {}

  public override void SetupGameState() {
    
    GameObject map = GameObject.Find( "PMap" );

    Hero hero = NetSpawn<Hero>( "PHero", Vector3.zero );

    hero.transform.parent = map.transform;
  }
}
