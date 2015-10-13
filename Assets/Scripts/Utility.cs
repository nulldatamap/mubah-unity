using UnityEngine;
using UnityEngine.Assertions;
using System;

public class Utility {

  public static Vector3 ScreenPointToPlanarPoint( Vector3 loc ) {
    Assert.Equals( loc.z, 0 );
    Ray ray = Camera.main.ScreenPointToRay( loc );
    
    // TODO: Make this less hard-coded
    Plane floor = new Plane( Vector3.up, Vector3.zero );

    float distance;

    if( !floor.Raycast( ray, out distance ) ) {
      string errmsg = "Click ray was parrallel with map floor.";
      Debug.LogException( new InvalidOperationException( errmsg ) );
    }

    Vector3 point = ray.GetPoint( distance );
    Assert.Equals( point.y, 0 );

    return point;
  }

  public static Vector3 GetClickedPlanarPoint() {
    return ScreenPointToPlanarPoint( Input.mousePosition );
  }

  // Only works on actors on Friendly/Enemy/Neutral layers
  public static Option<Entity> GetClickedEntity( int qual = 3072 ) {
    
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
    RaycastHit hit;

    if( Physics.Raycast( ray, out hit, 100.0f ) ) {
      // TODO: Make make sure we're at the root object?
      Entity entity = hit.transform.gameObject.GetComponent<Entity>();
      
      Assert.IsNotNull( entity );

      return Option<Entity>.Some( entity );
    }

    return Option<Entity>.None();
  }

  public static int GetLayerValue( Team team, bool player ) {
    if( player ) {
      return PlayerLayer;
    }

    switch( team ) {
      case Team.Neutral:
        return NeutralLayer;
      case Team.Friendly:
        return FriendlyLayer;
      case Team.Enemy:
        return EnemyLayer;
    }

    return 0;
  }



  public static int MapLayer      = 8;
  public static int FriendlyLayer = 9;
  public static int EnemyLayer    = 10;
  public static int NeutralLayer  = 11;
  public static int PlayerLayer   = 12;

  public static int MapLayerMask      = 1 << MapLayer;
  public static int FriendlyLayerMask = 1 << FriendlyLayer;
  public static int EnemyLayerMask    = 1 << EnemyLayer;
  public static int NeutralLayerMask  = 1 << NeutralLayer;
  public static int PlayerLayerMask   = 1 << PlayerLayer;
}