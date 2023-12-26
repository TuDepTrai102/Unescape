using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spells Cost")]
        public int focusPointCost;

        [Header("Spells Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spells Description")]
        [TextArea] public string spellDescription;

        public virtual void AttemptToCashSpell(CharacterManager character)
        {
            Debug.Log("You attemp to cash spell");
        }

        public virtual void SuccessfullyCashSpell(CharacterManager character)
        {
            Debug.Log("You successfully cash a spell");
            PlayerManager player = character as PlayerManager;

            if (player != null)
            {
                player.playerStatsManager.DeductFocusPoints(focusPointCost);
            }
        }
    }
}