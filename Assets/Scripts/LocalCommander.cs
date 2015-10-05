using UnityEngine;

public class LocalCommander : ICommander {

  Hero receiver;

  public LocalCommander( Hero r ) {
    receiver = r;
  }

  Vector3 ClickScan() {
    return Utility.ScreenPointToPlanarPoint( Input.mousePosition );
  }

  public void EmitCommands() {

    if( Input.GetButtonDown("Fire1") ) {
      Vector3 target = this.ClickScan();
      receiver.GotCommand( new Command.MoveTo( target ) );
    }

    // TODO: Base this on key-bindings
    if( Input.GetKeyDown( KeyCode.Y ) ) {
      CameraController.Get().ToggleLockOn( receiver.gameObject );
    }
  }
}
