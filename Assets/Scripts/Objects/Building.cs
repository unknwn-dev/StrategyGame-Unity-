using System.Collections.Generic;

[System.Serializable]
public class Building
{
    public enum BuildType
    {
        Castle = 0,
        Farm = 1
    }

    public BuildType Type;
    public int HP;

    public Building(BuildType type)
    {
        Type = type;

        if (type == BuildType.Castle) HP = GameController.Instance.Settings.CastleHP;
        else if (type == BuildType.Farm) HP = GameController.Instance.Settings.FarmHP;
    }
}
