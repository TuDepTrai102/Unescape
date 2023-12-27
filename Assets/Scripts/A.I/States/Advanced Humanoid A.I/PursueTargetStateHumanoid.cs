using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PursueTargetStateHumanoid : State
    {
        CombatStanceStateHumanoid combatStanceState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceStateHumanoid>();
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
            HandleRotateTowardsTarget(enemy);

            if (enemy.isInteracting)
                return this;

            if (enemy.isPerformingAction)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                combatStanceState._rushedInAttackTimer += Time.deltaTime;

                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                enemy.characterSoundFXManager.PlayRandomFootStep_Run();
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private State ProcessArcherCombatStyle(AICharacterManager enemy)
        {
            HandleRotateTowardsTarget(enemy);

            if (enemy.isInteracting)
                return this;

            if (enemy.isPerformingAction)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                if (!enemy.isStationaryArcher)
                {
                    enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                    enemy.characterSoundFXManager.PlayRandomFootStep_Run();
                }
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(AICharacterManager enemyManager)
        {
            //ROTATE MANUALLY
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp
                    (transform.rotation, targetRotation, enemyManager.rotationSpeed);
            }
            //ROTATE WITH PATHFINDING (NAVMESH AGENT)
            else
            {
                Vector3 targetVelocity = enemyManager.aiRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.aiRigidbody.velocity = targetVelocity;
                enemyManager.aiRigidbody.velocity = enemyManager.navMeshAgent.desiredVelocity;

                Vector3 directionToTarget = (enemyManager.currentTarget.transform.position - enemyManager.transform.position).normalized;

                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                    enemyManager.transform.rotation = Quaternion.Slerp
                        (enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed);
                }

                //enemyManager.transform.rotation = Quaternion.Slerp
                //    (enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed);
            }
        }
    }
}