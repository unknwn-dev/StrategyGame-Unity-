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
    public bool IsMakeStep = false;

    public Unit(Player own, UnitType type)
    {
        Owner = own;
        Type = type;
    }
}
