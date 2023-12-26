using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [System.Serializable]
    public class _CharacterStartScreenData
    {
        public string characterName;

        [Header("EQUIPMENT")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;

        public int currentHeadGearItemID;
        public int currentChestGearItemID;
        public int currentLegGearItemID;
        public int currentHandGearItemID;
    }
}