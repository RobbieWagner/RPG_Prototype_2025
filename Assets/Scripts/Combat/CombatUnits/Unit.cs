using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class UnitData
    {
        public string unitName;
        public Sprite unitIcon;
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<StatType, int> baseStats = new SerializedDictionary<StatType, int>();
        public List<CombatMove> combatMoves = new List<CombatMove>();

        // sprite path for the unit in the resources folder
        public string unitSpriteFilePath;
    }

    public class UnitInstance : MonoBehaviour
    {
        private UnitData unitData = null;
        public UnitData UnitData
        {
            get
            {
                return unitData;
            }
            set
            {
                unitData = value;
                UpdateUnitData();
            }
        }

        public SpriteRenderer unitSpriteRenderer;

        private void UpdateUnitData()
        {
            Sprite sprite = Resources.Load<Sprite>(unitData.unitSpriteFilePath);
            unitSpriteRenderer.sprite = sprite;
        }
    }
}