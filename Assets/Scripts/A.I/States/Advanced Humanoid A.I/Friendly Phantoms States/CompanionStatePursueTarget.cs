using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class CompanionStatePursueTarget : State
    {
        CompanionStateCombatStance combatStanceState;
        CompanionStateFollowHost followHostState;

        private void Awake()
        {
            combatStanceState = GetComponent<CompanionStateCombatStance>();
            followHostState = GetComponent<CompanionStateFollowHost>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

            if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
        {
            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private State ProcessArcherCombatStyle(AICharacterManager aiCharacter)
        {
            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                if (!aiCharacter.isStationaryArcher)
                {
                    aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
            }

            if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            //ROTATE MANUALLY
            if (aiCharacter.isPerformingAction)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp
                    (transform.rotation, targetRotation, aiCharacter.rotationSpeed);
            }
            //ROTATE WITH PATHFINDING (NAVMESH AGENT)
            else
            {
                Vector3 relativeRotation = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = aiCharacter.characterController.velocity;

                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
                aiCharacter.characterController.Move(targetVelocity);
                aiCharacter.transform.rotation = Quaternion.Slerp
                    (aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed);
            }
        }
    }
}