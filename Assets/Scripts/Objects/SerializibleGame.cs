using System.Collections.Generic;

[System.Serializable]
public class SerializibleGame
{
    public List<SerializiblePlayer> Players = new List<SerializiblePlayer>();
    public Dictionary<SerializibleV3Int, SerializibleCell> World = new Dictionary<SerializibleV3Int, SerializibleCell>();
    public int PlayerStep;
    public int Steps;

    public SerializibleGame(Game game) {
        PlayerStep = game.PlayerStep;
        Steps = game.Steps;

        foreach(var c in game.World) {
            World.Add(new SerializibleV3Int(c.Key), new SerializibleCell(c.Value));
        }

        foreach(var p in game.Players) {
            Players.Add(new SerializiblePlayer(p));
        }
    }

    public Game WriteToGame() {
        Game game = new Game();

        foreach (var p in Players) {
            game.Players.Add(p.ToPlayer());
        }

        foreach(var c in World) {
            game.World.Add(c.Key.ToUnityVector(), c.Value.ToCell(game.Players));
        }

        game.PlayerStep = PlayerStep;
        game.Steps = Steps;

        return game;
    }
}
