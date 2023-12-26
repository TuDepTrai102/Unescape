using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Attempt Critical Attack Action")]
    public class CriticalAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.isInteracting)
                return;

            if (!player._playerSkills._combat_BackstabOrRiposteSkill._combat_CriticalAttackUnlocked)
            {
                Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
            }
            else
            {
                character.characterCombatManager.AttemptBackStabOrRiposte();
            }
        }
    }
}