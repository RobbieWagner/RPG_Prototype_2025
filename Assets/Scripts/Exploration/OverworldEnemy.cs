using System;
using UnityEngine;
using UnityEngine.AI;

namespace RobbieWagnerGames.RPG
{
    public enum OverworldEnemyState
    {
        NONE = -1,
        IDLE,
        WANDERING,
        CHASING,
        ATTACKING
    }

    public class OverworldEnemy : MonoBehaviour
    {
        [SerializeField] private UnitAnimator animator;

        [Header("Navigation Settings")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Vector2 idleTimeRange = new Vector2(1f, 3f);
        [SerializeField] private Vector2 wanderTimeRange = new Vector2(3f, 7f);
        public CombatDetails enemyCombatEncounterDetails;

        private void Awake()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
}