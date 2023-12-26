using NT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public AICharacterAttackAction[] enemyAttacks;
        PursueTargetState pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        protected virtual void Awake()
        {
            attackState = GetComponent<AttackState>();
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager enemy)
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformAttack = false;

            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }
                
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(AICharacterManager enemy)
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

        protected void DecideCirclingAction(AICharacterManager enemyManager)
        {
            WalkAroundTarget(enemyManager);
        }

        protected void WalkAroundTarget(AICharacterManager enemyManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }

            enemyManager.characterSoundFXManager.PlayRandomFootStep_Walk();
        }

        protected virtual void GetNewAttack(AICharacterManager enemy)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                AICharacterAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                AICharacterAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}