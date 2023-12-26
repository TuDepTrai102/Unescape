using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCashSpell(CharacterManager character)
        {
            base.AttemptToCashSpell(character);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterAnimatorManager.transform);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            Destroy(instantiatedWarmUpSpellFX, 1.5f);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCashSpell(CharacterManager character)
        {
            base.SuccessfullyCashSpell(character);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, character.characterAnimatorManager.transform);
            character.characterStatsManager.HealCharacter(healAmount);
            Destroy(instantiatedSpellFX, 1.5f);
            Debug.Log("Spell cast succesful");
        }
    }
}