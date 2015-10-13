using UnityEngine;
using UnityEngine.Networking;

public class HeroController {
  Hero hero;  

  public HeroController( Hero h ) {
    hero = h;
  }

  // Move this to a input handlings code
  public void HandleInput() {
    if( Input.GetButtonDown("Fire1") ) {

      Option<Entity> oTargetEntity = Utility.GetClickedEntity();

      if( oTargetEntity.IsSome() ) {
        Debug.Log( "I certainly hit something" );
      }

      if( oTargetEntity.Map( x => x.IsTargetable ).UnwrapOr( false ) ) {
        Debug.Log( "Tried to target someone!" );
      } else {
        Vector3 targetPoint = Utility.GetClickedPlanarPoint();
        hero.SwitchState( new HeroState.Moving( targetPoint ) );
        hero.CmdMoveTo( targetPoint );
      }
    }

    // TODO: Base this on key-bindings
    if( Input.GetKeyDown( KeyCode.Y ) ) {
      CameraController.Get().ToggleLockOn( hero.gameObject );
    }

    if( Input.GetKey( KeyCode.Space ) ) {
      CameraController.Get().SnapTo( hero.gameObject );
    }
  }

}
