using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class Unit : MonoBehaviour
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