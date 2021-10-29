using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Game
{
    public List<Player> Players = new List<Player>();
    [SerializeField]
    public Dictionary<Vector3Int, Cell> World = new Dictionary<Vector3Int, Cell>();
    public int PlayerStep = 0;
    public int Steps = 0;
    public bool IsLoaded;
    private string savename;
    public string SaveName{
        get {
            return savename;
        }
        set {
            IsLoaded = true;
            savename = value;
        }
    }

    public Game() {

    }

    public void SaveGame(string name) {
        string savePath = Settings.Instance.SaveFolder + "/" + name + ".gmsv";

        SerializibleGame game = new SerializibleGame(this);

        using (Stream stream = File.Open(savePath, FileMode.Create)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, game);
        }
    }

    public void LoadGame(string name) {
        string savePath = Settings.Instance.SaveFolder + "/" + name + ".gmsv";

        SerializibleGame loaded;

        using (Stream stream = File.Open(savePath, FileMode.Open)) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            loaded = (SerializibleGame)binaryFormatter.Deserialize(stream);
        }

        Game gameCopy = loaded.WriteToGame();

        World = gameCopy.World;
        Players = gameCopy.Players;
        PlayerStep = gameCopy.PlayerStep;
        Steps = gameCopy.Steps;

        IsLoaded = true;

        UpdateGame();
    }

    private void UpdateGame() {
        GameController GCInst = GameController.Instance;
        GCInst.CellModsTilemap.ClearAllTiles();
        GCInst.GroundTilemap.ClearAllTiles();

        foreach (var c in World) {
            if(c.Value.IsGround){
                GCInst.GroundTilemap.SetTile(c.Key, GCInst.Settings.GroundTile);
                if (c.Value.Rec.Type == Recources.CellType.Forest) {
                    GCInst.CellModsTilemap.SetTile(c.Key, GCInst.Settings.ForestTile);
                }
                else if (c.Value.Rec.Type == Recources.CellType.Mountain) {
                    GCInst.CellModsTilemap.SetTile(c.Key, GCInst.Settings.MountainTile);
                }
                if(c.Value.Building != null && c.Value.Building.Type == Building.BuildType.Castle) {
                    c.Value.BuildCity();
                }
                c.Value.UpdateOwn();
            }
        }
    }
}
