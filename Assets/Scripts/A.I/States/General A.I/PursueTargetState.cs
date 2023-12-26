using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PursueTargetState : State
    {
        CombatStanceState combatStanceState;
        RotateTowardsTargetState rotateTowardsTargetState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceState>();
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetState>();
        }

        public override State Tick(AICharacterManager enemy)
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
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
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

        private void HandleRotateTowardsTarget(AICharacterManager enemy)
        {
            //ROTATE MANUALLY
            if (enemy.isPerformingAction)
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
            //ROTATE WITH PATHFINDING (NAVMESH AGENT)
            else
            {
                Vector3 relativeRotation = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemy.aiRigidbody.velocity;

                enemy.navMeshAgent.enabled = true;
                enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
                enemy.aiRigidbody.velocity = targetVelocity;
                enemy.transform.rotation = Quaternion.Slerp
                    (enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, enemy.rotationSpeed);
            }
        }
    }
}