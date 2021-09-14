using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit
{
    public enum UnitType
    {
        Null = -1,
        Citizen = 0,
        Warrior = 1
    }

    public Player Owner;
    public Tile UnitTile = new Tile();
    public UnitType Type;
    public int HP;
    public int Damage;
    public bool IsMakeStep = false;

    public Unit(Player own, UnitType type)
    {
        Owner = own;
        Type = type;

        if (type == UnitType.Citizen)
        {
            HP = MainScript.Instance.Settings.CitizenHP;
            Damage = MainScript.Instance.Settings.CitizenDmg;
            UnitTile.sprite = MainScript.Instance.Settings.CitizSpr;
        }
        else if (type == UnitType.Warrior)
        {
            HP = MainScript.Instance.Settings.WarriorHP;
            Damage = MainScript.Instance.Settings.WarriorDmg;
            UnitTile.sprite = MainScript.Instance.Settings.WarriorSpr;
        }

        Color UnitColor = new Color();
        UnitColor.r = Owner.PlayerColor.r - 0.2f ;
        UnitColor.g = Owner.PlayerColor.g - 0.2f;
        UnitColor.b = Owner.PlayerColor.b - 0.2f;
        UnitColor.a = 1;

        UnitTile.color = UnitColor;
    }
}
