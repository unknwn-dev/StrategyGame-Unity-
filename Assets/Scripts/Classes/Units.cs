using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units
{
    public Player Owner;
    public int Number;

    public Units(Player own, int numb)
    {
        Owner = own;
        Number = numb;
    }
}
