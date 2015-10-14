using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public enum Team {
  Neutral,
  Friendly,
  Enemy,
}

public class Entity : NetworkBehaviour {
  internal Rigidbody rigidBody;

  internal Team team = Team.Neutral;

  public int netId = 0;

  // TODO: Bad naming?
  public Team Team {
    get { return team; }

    set {
      team = value;
      gameObject.layer = Utility.GetLayerValue( team, isLocalPlayer ); 
    }
  }

  bool targetable = true;

  public bool IsTargetable {
    get { return targetable; }
  }

  public Vector3 PlanarPosition {
    get {
      Vector3 pos = transform.position;
      pos.y = 0;
      return pos;
    }

    set {
      Assert.Equals( value.y, 0 );
      transform.position = new Vector3( value.x, transform.position.y, value.z );
    }
  }

  // The distance between another point, ignoring the Y-axis
  // ASSUMES b is planar too
  public float PlanarDistance( Vector3 b ) {

    Vector3 planarA = transform.position;
    planarA.y = 0;

    return Vector3.Distance( planarA, b);
  }

  public void MoveDirection( Vector3 dir ) {
    // TODO: Base this on movement speed
    rigidBody.isKinematic = false;
    rigidBody.velocity = dir;
  }

  public void StopMoving() {
    rigidBody.isKinematic = true;
  }

  // Set's the planar position and stops
  public void SetPosition( Vector3 pos ) {
    PlanarPosition = pos;
  }

  virtual public void Start() {
    rigidBody = GetComponent<Rigidbody>();
    Assert.IsNotNull( rigidBody );
  }

  virtual public void Update() {}
}

