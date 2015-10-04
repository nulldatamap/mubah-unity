using System.Collections.Generic;

public interface IGameMode {
  List<ICommander> CreateCommanders();
  void SetupGameState();
}
