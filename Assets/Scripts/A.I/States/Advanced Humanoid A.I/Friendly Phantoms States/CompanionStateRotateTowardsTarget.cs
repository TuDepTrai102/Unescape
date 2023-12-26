using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CompanionStateRotateTowardsTarget : State
    {
        CompanionStateCombatStance combatStanceState;

        private void Awake()
        {
            combatStanceState = GetComponent<CompanionStateCombatStance>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.viewableAngle >= 100 && aiCharacter.viewableAngle <= 180 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(aiCharacter.aiCharacterAnimatorManager.animation_turn_behind_left, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle <= -101 && aiCharacter.viewableAngle >= -180 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(aiCharacter.aiCharacterAnimatorManager.animation_turn_behind_right, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle >= 45 && aiCharacter.viewableAngle <= 100 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(aiCharacter.aiCharacterAnimatorManager.animation_turn_left, true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle <= -45 && aiCharacter.viewableAngle >= -100 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(aiCharacter.aiCharacterAnimatorManager.animation_turn_right, true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}