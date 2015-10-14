using UnityEngine;
using System.Collections.Generic;


public abstract class GameMode {
  int nextId;
  bool isHost;

  public Option<Hero> localPlayer = Option<Hero>.None();
  public List<Hero> players = new List<Hero>();
  
  public GameMode( bool host ) {
    isHost = host;
    nextId = 1;
  }

  public E NetSpawn<E>( string prefab, Vector3 pos ) where E: Entity {
    // TODO: Make sure clients don't try to net spawn anything

    GameObject go =
      GameObject.Instantiate( Resources.Load( prefab ) ) as GameObject;

    E ent = go.GetComponent<E>();
    ent.netId = nextId;
    nextId += 1;

    // TODO: Send / register spawn informaiton

    return ent;
  }

  public abstract void SetupGameState();
}
