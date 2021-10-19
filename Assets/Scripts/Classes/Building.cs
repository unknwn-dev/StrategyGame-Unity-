using System.Collections.Generic;

public class Building
{
    public enum BuildType
    {
        Castle = 0,
        Farm = 1
    }

    public BuildType Type;
    public int HP;
    public Player Owner;
    public List<Cell> Territories;

    public Building(BuildType type, Player pl)
    {
        Type = type;
        Owner = pl;

        if (type == BuildType.Castle) HP = MainScript.Instance.Settings.CastleHP;
        else if (type == BuildType.Farm) HP = MainScript.Instance.Settings.FarmHP;
    }
}
