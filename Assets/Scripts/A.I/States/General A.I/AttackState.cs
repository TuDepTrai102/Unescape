using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AttackState : State
    {
        RotateTowardsTargetState rotateTowardsTargetState;
        CombatStanceState combatStanceState;
        PursueTargetState pursueTargetState;
        public AICharacterAttackAction currentAttack;

        bool willDoComboNextAttack = false;
        public bool hasPerformAttack = false;

        private void Awake()
        {
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetState>();
            combatStanceState = GetComponent<CombatStanceState>();
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboNextAttack && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }

            if (!hasPerformAttack)
            {
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }

            if (willDoComboNextAttack && hasPerformAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(AICharacterManager enemy)
        {
            enemy.isUsingRightHand = currentAttack.isRightHandedAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
            enemy.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformAttack = true;
        }

        private void AttackTargetWithCombo(AICharacterManager enemy)
        {
            enemy.isUsingRightHand = currentAttack.isRightHandedAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
            willDoComboNextAttack = false;
            enemy.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(AICharacterManager enemy)
        {
            //ROTATE MANUALLY
            if (enemy.canRotate && enemy.isInteracting)
            {
                Vector3 direction = enemy.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp
                    (transform.rotation, targetRotation, enemy.rotationSpeed);
            }
        }

        private void RollForComboChance(AICharacterManager enemy)
        {
            float comboChance = Random.Range(0, 100);

            if (enemy.allowAIToPerformCombos && comboChance <= enemy.comboLikelyHood)
            {
                if (currentAttack.comboAction != null)
                {
                    willDoComboNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}