using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public abstract class HeroState {
  private HeroState() {}

  // Defualt does nothing
  public virtual void Update( Hero hero ) {}

  public virtual HeroState Switch( HeroState state ) {
    return state;
  }

  public sealed class Idle : HeroState {
  }

  public sealed class Moving : HeroState {
    Vector3 target;

    internal Moving( Vector3 t ) { target = t; }

    public override void Update( Hero hero ) {
      // TODO: Actually make this based on movement-speed
      float moveLength = Time.deltaTime;

      if( hero.PlanarDistance( target ) > moveLength ) {
        hero.MoveDirection( ( target - hero.PlanarPosition ).normalized );
      } else {
        hero.SetPosition( target );
        hero.StopMoving();
        hero.state = new Idle();
      }
    }
  }

  public sealed class GetInRange : HeroState {
    Entity target;

    public GetInRange( Entity t ) { target = t; }

    public override void Update( Hero hero ) {
      // TODO: Get in range
    }
  }
}

[CustomEditor( typeof( Actor ), true )]
public class Hero : Actor {

  internal HeroState state = new HeroState.Idle();
  HeroController controller;

  public void SwitchState( HeroState s ) {
    state = state.Switch( s );
  }

  [Command]
  public void CmdMoveTo( Vector3 dest ) {
    SwitchState( new HeroState.Moving( dest ) );
    RpcMoveTo( dest );
  }

  [ClientRpc]
  public void RpcMoveTo( Vector3 dest ) {
    if( !isLocalPlayer ) {
      SwitchState( new HeroState.Moving( dest ) );
    }
  }

  public override void Start() {
    base.Start();

    controller = new HeroController( this );
  }

  public override void Update() {
    base.Update();

    if( isLocalPlayer ) {
      controller.HandleInput();
    }

    // Update the hero based on their state
    state.Update( this );
  }

}
