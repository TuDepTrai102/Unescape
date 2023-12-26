using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class PatrolStateHumanoid : State
    {
        PursueTargetStateHumanoid pursueTargetState;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public bool patrolComplete;
        public bool repeatPatrol;

        //HOW LONG BEFORE NEXT PATROL
        [Header("PATROL REST TIME")]
        public float endOfPatrolRestTime;
        public float endOfPatrolTimer;

        [Header("PATROL POSITION")]
        public int patrolDestinationIndex;
        public bool hasPatrolDestination;
        public Transform currentPatrolDestination;
        public float distanceFromCurrentPatrolPoint;
        public List<Transform> listOfPatrolDestinations = new List<Transform>();

        public override State Tick(AICharacterManager aiCharacter)
        {
            SearchForTargetWhilstPatroling(aiCharacter);

            //IF THE A.I IS PERFORMING SOME ACTION, HAULT ALL MOVEMENT AND RETURN
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }

            //IF WE'VE COMPLETED OUR PATROL AND WE DO WANT TO REPEAT IT, WE DO SO
            if (patrolComplete && repeatPatrol)
            {
                //WE COUNT DOWN OUR REST TIME, AND RESET ALL OF OUR PATROL FLAGS
                if (endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                    endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                    return this;
                }
                else if (endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if (patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return this;
            }

            if (hasPatrolDestination)
            {
                if (currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance
                        (aiCharacter.transform.position, currentPatrolDestination.transform.position);

                    if (distanceFromCurrentPatrolPoint > 1)
                    {
                        aiCharacter.navMeshAgent.enabled = true;
                        aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Lerp
                            (aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                        aiCharacter.characterSoundFXManager.PlayRandomFootStep_Walk();
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }

            if (!hasPatrolDestination)
            {
                patrolDestinationIndex = patrolDestinationIndex + 1;

                if (patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
                hasPatrolDestination = true;
            }

            return this;
        }

        private void SearchForTargetWhilstPatroling(AICharacterManager aiCharacter)
        {
            //SEARCHES FOR A POTENTIAL TARGET WITHIN THE DETECTION RADIUS
            Collider[] colliders = Physics.OverlapSphere
                (transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //IF A POTENTIAL TARGET IS FOUND, THAT IS NOT THE SAME TEAM AS THE A.I WE PROCEED TO THE NEXT STEP
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.aiCharacterStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                        float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

                        //IF A POTENTIAL TARGET IS FOUND, IT HAS TO BE STANDING INFRONT OF THE A.I'S FIELD OF VIEW
                        if (viewAbleAngle > aiCharacter.minimumDetectionAngle &&
                            viewAbleAngle < aiCharacter.maximumDetectionAngle)
                        {
                            //IF THE A.I'S POTENTIAL TARGET HAS AN OBSTRUCTION IN BETWEEN ITSELF AND THE A.I
                            //WE DO NOT ADD IT AS OUR CURRENT TARGET
                            if (Physics.Linecast
                                (aiCharacter.lockOnTransform.position,
                                targetCharacter.lockOnTransform.position,
                                layersThatBlockLineOfSight))
                            {
                                return;
                            }
                            else
                            {
                                aiCharacter.currentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }

            if (aiCharacter.currentTarget != null)
            {
                return;
            }
            else
            {
                return;
            }
        }
    }
}