using UnityEngine;
using UnityEngine.Assertions;

/*
  data CameraMode = Free
                  | LockedOn GameObject
*/

abstract class CameraMode {
  private CameraMode() {}

  public virtual CameraMode Switch( CameraMode mode ) {
    return mode;
  }

  public virtual void Update( CameraController cc ) {}

  public sealed class Free : CameraMode {
    public override void Update( CameraController cc ) {
      Transform cct = cc.transform;

      Vector3 center = new Vector3( Screen.width / 2, Screen.height / 2 );
      Vector3 mpos = Input.mousePosition - center;
      Vector3 ratio = new Vector3( mpos.x / center.x, mpos.y / center.y, 0 );
      
      if( ratio.x < -0.9 ) {
        cct.position += cct.right * -Time.deltaTime;
      } else if( ratio.x > 0.9 ) {
        cct.position += cct.right * Time.deltaTime;
      }

      if( ratio.y < -0.9 ) {
        cct.position += cct.forward * -Time.deltaTime;
      } else if( ratio.y > 0.9 ) {
        cct.position += cct.forward * Time.deltaTime;
      }
    }
  }

  public sealed class LockedOn : CameraMode {
    readonly GameObject target;

    public LockedOn( GameObject t ) { target = t; }

    public override void Update( CameraController cc ) {
      
      if( target == null ) {
        cc.mode = Switch( new CameraMode.Free() );
        return;
      }

      cc.SetPosition( target.transform.position );
    }
  }
}

public class CameraController : MonoBehaviour {
  // Deprecated field: UnityEngine.Component.camera
  new internal Camera camera;
  internal CameraMode mode;
  internal Vector3 offset;
  // TODO: Move area, smoothing, camera speed

  public static CameraController Get() {
    return GameObject.Find( "PCameraController" ).GetComponent<CameraController>();
  }
   
  Vector3 FindOffset() {
    // TODO: Make this dependant on the map
    Vector3 screenCenter =
      new Vector3( Screen.width / 2, Screen.height / 2, 0 );
    
    Vector3 offset =
      transform.position - Utility.ScreenPointToPlanarPoint( screenCenter );

    return offset;
  }

  public void ToggleLockOn( GameObject t ) {
    if( mode is CameraMode.LockedOn ) {
      mode = mode.Switch( new CameraMode.Free() );
    } else {
      mode = mode.Switch( new CameraMode.LockedOn( t ) );
    }
  }

  public void SnapTo( GameObject t ) {
    if( !(mode is CameraMode.LockedOn) ) {
      SetPosition( t.transform.position );
    }
  }

  public void SetPosition( Vector3 pos ) {
    Vector3 newPos = pos + offset;
    // Always keep the height the same
    newPos.y = offset.y;

    transform.position = newPos;
  }

  void Start() {
    camera = GetComponentInChildren<Camera>();
    Assert.IsNotNull( camera );

    // TODO: Make this based on settings
    mode = new CameraMode.Free();

    offset = FindOffset();
  }

  public void Update() {
    mode.Update( this );
  }

}
