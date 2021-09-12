using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recources
{
    public enum CellType {
        Flat = 0,
        Forest = 1,
        River = 2,
        Mountain = 3
    }

    public CellType Type;
    public int Population;
    public float PopGrowth;
    public float Income;
    public float Happiness;
    public float Taxes;

    public Recources(CellType type, float happy, float tax)
    {
        Taxes = tax;
        Type = type;

        if(type == CellType.Flat)
        {
            Population = Random.Range(3000, 6000);
            Happiness = happy;
            PopGrowth = Random.Range(-10 * happy, 10 * happy);
            Income = (Population / 100) * Taxes;
        }
        else if (type == CellType.River)
        {
            Population = Random.Range(2500, 5500);
            Happiness = happy + 2;
            PopGrowth = Random.Range(-10 * happy, 10 * happy);
            Income = (Population / 100) * Taxes + 100;
        }
        else if (type == CellType.Forest)
        {
            Population = Random.Range(1000, 3000);
            Happiness = happy + 5;
            PopGrowth = Random.Range(-8 * happy, 8 * happy);
            Income = (Population / 100) * Taxes + 200;
        }
    }

    public void MakeStep()
    {
        Population += Mathf.RoundToInt(PopGrowth);
        PopGrowth += Random.Range(-8 * Happiness, 8 * Happiness) / 10;
        Happiness += (-PopGrowth / 1000) / Taxes;
        Income = Taxes * Population;
    }

    public static CellType GetRandomCellType()
    {
        float random = Random.Range(0.01f,1);

        if (random <= Settings.Instance.ForestChance) return CellType.Forest;
        else if (random > Settings.Instance.ForestChance && random <= Settings.Instance.MounatinChance) return CellType.Mountain;
        else return CellType.Flat;
    }
}
