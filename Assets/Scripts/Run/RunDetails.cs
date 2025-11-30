using System;
using System.Collections.Generic;
using RobbieWagnerGames.RPG;
using UnityEngine;

[Serializable]
public class RunDetails
{
    public int kerfufflesWon = 0;
    
    [SerializeField] private List<UnitData> playerParty = new List<UnitData>();
    public List<UnitData> PlayerParty => new List<UnitData>(playerParty);
    
    [HideInInspector] public UnitData playerCustomUnit;

    public List<UnitData> unitOptions = new List<UnitData>();
    public int runSeed;

    public void AddUnitToParty(UnitData unit)
    {
        if (unit != null && !playerParty.Contains(unit))
        {
            playerParty.Add(unit);
        }
    }

    public void RemoveUnitFromParty(UnitData unit)
    {
        if (unit != null)
        {
            playerParty.Remove(unit);
        }
    }

    public void ClearParty()
    {
        playerParty.Clear();
    }

    public void SetCustomUnit(UnitData customUnit)
    {
        playerCustomUnit = customUnit;
    }

    public override string ToString()
    {
        return $"{playerCustomUnit.unitName} save:\nSeed: {runSeed}\nKerfuffles Won{kerfufflesWon}\nParty Size: {playerParty.Count}";
    }
}