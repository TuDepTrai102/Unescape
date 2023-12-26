using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AttackStateHumanoid : State
    {
        IdleStateHumanoid idleState;
        RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
        CombatStanceStateHumanoid combatStanceState;
        PursueTargetStateHumanoid pursueTargetState;
        public ItemBaseAttackAction currentAttack;

        public bool willDoComboNextAttack = false;
        public bool hasPerformAttack = false;

        private void Awake()
        {
            idleState = GetComponent<IdleStateHumanoid>();
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetStateHumanoid>();
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();
            pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }

        public override State Tick(AICharacterManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if (enemy.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
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

            ResetStateFlags();

            return rotateTowardsTargetState;
        }

        private State ProcessArcherCombatStyle(AICharacterManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.isInteracting)
                return this;

            if (!enemy.isHoldingArrow)
            {
                ResetStateFlags();
                return combatStanceState;
            }

            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return idleState;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            if (!hasPerformAttack && enemy.isHoldingArrow)
            {
                //FIRE AMMO
                FireAmmo(enemy);
            }

            ResetStateFlags();

            return rotateTowardsTargetState;
        }

        private void AttackTarget(AICharacterManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformAttack = true;
        }

        private void AttackTargetWithCombo(AICharacterManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            willDoComboNextAttack = false;
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
                if (currentAttack.actionCanCombo)
                {
                    willDoComboNextAttack = true;
                }
                else
                {
                    willDoComboNextAttack = false;
                    currentAttack = null;
                }
            }
        }

        private void ResetStateFlags()
        {
            willDoComboNextAttack = false;
            hasPerformAttack = false;
        }

        private void FireAmmo(AICharacterManager enemy)
        {
            if (enemy.isHoldingArrow)
            {
                hasPerformAttack = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_tap_E_Action.PerformAction(enemy);
            }
        }
    }
}