using UnityEngine;

public class Recources {
    public enum CellType {
        Flat = 0,
        Forest = 1,
        River = 2,
        Mountain = 3
    }

    public CellType Type;
    public float Income;
    public int MPForMove;

    public Recources(CellType type) {
        Type = type;

        if (type == CellType.Flat) {
            Income = 1;
            MPForMove = GameController.Instance.Settings.FlatMPForMove;
        }
        else if (type == CellType.River) {
            Income = 2;
        }
        else if (type == CellType.Forest) {
            Income = 2;
            MPForMove = GameController.Instance.Settings.ForestMPForMove;
        }
    }

    public static CellType GetRandomCellType() {
        float random = Random.Range(0.01f, 1);

        if (random <= GameController.Instance.Settings.ForestChance) return CellType.Forest;
        else if (random > GameController.Instance.Settings.ForestChance && random <= GameController.Instance.Settings.MounatinChance) return CellType.Mountain;
        else return CellType.Flat;
    }
}
