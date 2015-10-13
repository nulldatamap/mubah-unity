using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

[CustomEditor( typeof( Entity ), true )]
public class Actor : Entity {
  [SyncVar]
  public Stats baseStats;
  [SyncVar]
  public Stats effectiveStats;

  public override void Start() {
    base.Start();

    if( isLocalPlayer ) {
      team = Team.Friendly;
    }

    baseStats = new Stats();
    effectiveStats = baseStats;
  }

  public void UpdateEffectiveStats() {
    effectiveStats = baseStats;
  }

}
