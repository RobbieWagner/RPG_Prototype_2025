using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [CreateAssetMenu(fileName = "Combat Move", menuName = "RobbieWagnerGames/Combat/Combat Move")]
    public class CombatMove : ScriptableObject
    {
        public string moveName = "New Move";
        public Sprite moveIcon = null;
        public int moveCost = 0;
        [HideInInspector] public List<GameAction> moveActions = new();
        [TextArea] public string description;

        public bool targetsAllOpposition;
        public bool targetsAllAllies;

        public bool canTargetSelf;
        public bool canTargetOpposition;
        public bool canTargetAllies;

        [SerializeReference] public List<MoveEffect> effects;

        [ContextMenu(nameof(AddAttackEffect))] void AddAttackEffect() { effects.Add(new Attack()); }
        [ContextMenu(nameof(AddHealEffect))] void AddHealEffect() { effects.Add(new Heal()); }
        [ContextMenu(nameof(AddBuffEffect))] void AddBuffEffect() { effects.Add(new Buff()); }
        [ContextMenu(nameof(AddDebuffEffect))] void AddDebuffEffect() { effects.Add(new Debuff()); }
        [ContextMenu(nameof(Clear))] void Clear() { effects.Clear(); }

        public void BuildMoveActions()
        {
            moveActions.Clear();
            foreach (MoveEffect effect in effects)
            {
                switch (effect)
                {
                    case Attack attack:
                        moveActions.Add(new MakeAttackCA(attack));
                        break;
                    case Heal heal:
                        moveActions.Add(new HealUnitCA(heal));
                        break;
                    case Buff buff:
                        moveActions.Add(new BuffUnitCA(buff));
                        break;
                    case Debuff debuff:
                        moveActions.Add(new DebuffUnitCA(debuff));
                        break;
                }
            }
        }
        
        public virtual bool isAvailable(Unit unit)
        {
            return moveCost <= unit.runtimeStats[ComputedStatType.STAMINA];
        }
    }
}