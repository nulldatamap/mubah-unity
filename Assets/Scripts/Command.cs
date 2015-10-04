using UnityEngine;

public abstract class Command {

  public sealed class MoveTo : Command {
    public readonly Vector3 destination;
    public MoveTo( Vector3 d ) { destination = d; }
  }

}

public interface ICommander {
  void EmitCommands();
}

public interface ICommandable {
  void GotCommand( Command cmd );
}
