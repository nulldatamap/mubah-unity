using UnityEngine;
using UnityEngine.Assertions;
using System;

public class Utility {
  public static Vector3 ScreenPointToPlanarPoint( Vector3 screenLoc ) {

    Assert.Equals( screenLoc.z, 0 );

    Ray ray = Camera.main.ScreenPointToRay( screenLoc );
    
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
}