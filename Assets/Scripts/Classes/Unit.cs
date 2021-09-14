using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public enum UnitType
    {
        Null = -1,
        Citizen = 0,
        Warrior = 1
    }

    public Player Owner;
    public UnitType Type;
    public int HP;
    public int Damage;
    public bool IsMakeStep = false;

    public Unit(Player own, UnitType type)
    {
        Owner = own;
        Type = type;

        if(type == UnitType.Citizen)
        {
            HP = MainScript.Instance.Settings.CitizenHP;
            Damage = MainScript.Instance.Settings.CitizenDmg;
        }
        else if (type == UnitType.Warrior)
        {
            HP = MainScript.Instance.Settings.WarriorHP;
            Damage = MainScript.Instance.Settings.WarriorDmg;
        }
    }
}
