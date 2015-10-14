using UnityEngine;
using UnityEngine.Networking;


public class Actor : Entity {
  public Stats baseStats;
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
