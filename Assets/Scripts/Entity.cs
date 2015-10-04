using UnityEngine;
using UnityEngine.Assertions;

public class Entity : MonoBehaviour {
  Rigidbody rigidBody;

  public Vector3 planarPosition {
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
    rigidBody.velocity = dir;
  }

  // Set's the planar position and stops any ongoing motion
  public void SetPosition( Vector3 pos ) {
    planarPosition = pos;
    rigidBody.velocity = Vector3.zero;
  }

  virtual public void Start() {
    rigidBody = GetComponent<Rigidbody>();
    Assert.IsNotNull( rigidBody );
  }

  virtual public void Update() {
  }
}

