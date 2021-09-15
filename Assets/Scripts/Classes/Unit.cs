using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit
{
    public enum UnitType
    {
        Null = -1,
        Citizen = 0,
        Warrior = 1,
        Settlers = 2
    }

    public Player Owner;
    public Tile UnitTile = new Tile();
    public UnitType Type;
    public int HP;
    public int Damage;
    public int MaintenanceCost;
    public bool IsMakeStep = false;

    public Unit(Player own, UnitType type)
    {
        Owner = own;
        Type = type;

        Settings sett = MainScript.Instance.Settings;

        if (type == UnitType.Citizen)
        {
            HP = sett.CitizenHP;
            Damage = sett.CitizenDmg;
            UnitTile.sprite = sett.CitizSpr;
            MaintenanceCost = sett.CitizenMaintenanceCost;
        }
        else if (type == UnitType.Warrior)
        {
            HP = sett.WarriorHP;
            Damage = sett.WarriorDmg;
            UnitTile.sprite = sett.WarriorSpr;
            MaintenanceCost = sett.WarriorMaintenanceCost;
        }
        else if (type == UnitType.Settlers)
        {
            HP = sett.SettlersHP;
            UnitTile.sprite = sett.SettlersSpr;
            Damage = 0;
            MaintenanceCost = sett.SettlersMaintenanceCost;
        }

        Color UnitColor = new Color();
        UnitColor.r = Owner.PlayerColor.r - 0.2f ;
        UnitColor.g = Owner.PlayerColor.g - 0.2f;
        UnitColor.b = Owner.PlayerColor.b - 0.2f;
        UnitColor.a = 1;

        UnitTile.color = UnitColor;
    }

    public void BuildCity(Cell cell)
    {
        cell.Building = new Building(Building.BuildType.Castle, Owner);
        cell.Units = null;
        cell.UpdateOwn();

        MainScript MSC = MainScript.Instance;

        MSC.CellModsTilemap.SetTile(cell.CellPos, MSC.Settings.CastleTile);

        foreach (var neighbor in cell.Neighbors())
        {
            if (cell.Owner == null)
            {
                cell.Owner = Owner;
                cell.UpdateOwn();
            }
            if (MSC.World[neighbor].Owner == null && MSC.World[neighbor].IsGround)
            {
                MSC.World[neighbor].Owner = Owner;
                MSC.World[neighbor].UpdateOwn();
            }
        }

        MSC.ActionsPanel.GetComponent<UnitsActionScript>().CloseMenu();
    }
}
