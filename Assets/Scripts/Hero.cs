using UnityEngine;

/*
  data HeroState = Idle
                 | Moving Vector3
*/

abstract class HeroState {
  private HeroState() {}

  // Defualt does nothing
  public virtual void Update( Hero hero ) {}

  public virtual HeroState SwitchState( HeroState state ) {
    return state;
  }

  public sealed class Idle : HeroState {}

  public sealed class Moving : HeroState {
    Vector3 target;

    internal Moving( Vector3 t ) { target = t; }

    public override void Update( Hero hero ) {
      // TODO: Actually make this based on movement-speed
      float moveLength = Time.deltaTime;

      if( hero.PlanarDistance( target ) > moveLength ) {
        hero.MoveDirection( ( target - hero.planarPosition ).normalized );
      } else {
        hero.SetPosition( target );
        hero.state = new Idle();
      }
    }
  }
}


public class Hero : Actor, ICommandable {

  internal HeroState state = new HeroState.Idle();

  public void GotCommand( Command cmd ) {

    if ( cmd is Command.MoveTo ) {
      Vector3 dest = ( (Command.MoveTo) cmd ).destination;
      state = state.SwitchState( new HeroState.Moving( dest ) );
    }

  }

  public override void Update() {
    base.Update();

    // Update the hero based on their state
    state.Update( this );
  }

}
