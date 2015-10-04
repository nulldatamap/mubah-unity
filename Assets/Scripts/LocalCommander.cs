using UnityEngine;
using UnityEngine.Assertions;
using System;

public class LocalCommander : ICommander {

	ICommandable receiver;

	public LocalCommander( ICommandable r ) {
		receiver = r;
	}

  Vector3 ClickScan() {
    Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

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

  public void EmitCommands() {
    if( Input.GetButtonDown("Fire1") ) {
      Vector3 target = this.ClickScan();
			receiver.GotCommand( new Command.MoveTo( target ) );
    }
  }
}
