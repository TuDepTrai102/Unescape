using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class RotateTowardsTargetStateHumanoid : State
    {
        CombatStanceStateHumanoid combatStanceState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();    
        }

        public override State Tick(AICharacterManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            if (enemy.isInteracting)
                return this;

            if (enemy.viewableAngle >= 100 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
            {
                enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(enemy.aiCharacterAnimatorManager.animation_turn_behind_left, true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(enemy.aiCharacterAnimatorManager.animation_turn_behind_right, true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle >= 45 && enemy.viewableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(enemy.aiCharacterAnimatorManager.animation_turn_left, true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation(enemy.aiCharacterAnimatorManager.animation_turn_right, true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}