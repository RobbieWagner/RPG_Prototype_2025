using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public enum BaseStatType
    {
        // STAT EFFECTS WIP
        NONE = -1,
        CAT = 0, // Determines Accuracy, Stamina, and Magical Defense
        BOY = 1, // Determines Power, Defense, and Vitality
        ISEKAI = 2 // Determines Crit Chance, Magic Power, and Initiative
    }

    public enum ComputedStatType
    {
        // STAT EFFECTS WIP
        NONE = -1,

        STAMINA,
        ACCURACY,
        MAGIC_DEFENSE,

        POWER,
        DEFENSE,
        HP,

        MAGIC_POWER,
        CRIT_CHANCE,
        INITIATIVE
    }
}