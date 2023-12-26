using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Cast Magic Spell Action")]
    public class MagicSpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.characterInventoryManager.currentSpell != null && 
                character.characterInventoryManager.currentSpell.isMagicSpell)
            {
                if (character.characterStatsManager.currentFocusPoints >= 
                    character.characterInventoryManager.currentSpell.focusPointCost)
                {
                    character.characterInventoryManager.currentSpell.AttemptToCashSpell(character);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterAnimatorManager.animation_cannot_cast, true);
                }
            }
        }
    }
}