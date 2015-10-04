using UnityEngine;

public class Actor : Entity {
  Stats baseStats;
  Stats effectiveStats;

  public void Start() {
    base.Start();

    baseStats = new Stats();
    effectiveStats = baseStats;
  }

  public void UpdateEffectiveStats() {
    effectiveStats = baseStats;
  }

}
