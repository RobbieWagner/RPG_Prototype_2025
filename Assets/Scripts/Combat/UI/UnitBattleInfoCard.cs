using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.RPG
{
    public class UnitBattleInfoCard : MonoBehaviour
    {
        [SerializeField] private Image bg;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private Image unitIcon;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private List<Image> staminaBits = new List<Image>();
        [SerializeField] private Color staminaBitInactiveColor;
        [SerializeField] private Color staminaBitActiveColor;

        private Unit unit;
        public Unit Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if (unit != null && unit.gameObject == value.gameObject)
                    return;

                if (unit != null)
                    UnsubscribeFromUnit(unit);

                unit = value;
                UpdateUnitUI();

                if (unit != null)
                    SubscribeToUnit(unit);
            }
        }

        private void SubscribeToUnit(Unit unit)
        {
            unit.OnUpdateRuntimeStat += OnUpdateUnitStat;
        }

        private void UnsubscribeFromUnit(Unit unit)
        {
            unit.OnUpdateRuntimeStat -= OnUpdateUnitStat;
        }

        private void OnUpdateUnitStat(ComputedStatType type, int value)
        {
            UpdateUnitUI();
        }

        private void UpdateUnitUI()
        {
            UnitData unitData = unit.UnitData;

            unitNameText.text = unitData.unitName;
            unitIcon.sprite = unitData.unitIcon;
            bg.color = unitData.unitColor;

            hpSlider.minValue = 0;
            hpSlider.maxValue = unit.GetComputedStatDefaultValue(ComputedStatType.HP);
            hpSlider.value = unit.RuntimeStats[ComputedStatType.HP];

            ResetStaminaBar();
        }

        public void ResetStaminaBar()
        {
            for(int i = 0; i < staminaBits.Count; i++)
            {
                staminaBits[i].color = unit.RuntimeStats[ComputedStatType.STAMINA] > i ? staminaBitActiveColor : staminaBitInactiveColor;
            }
        }
    }
}