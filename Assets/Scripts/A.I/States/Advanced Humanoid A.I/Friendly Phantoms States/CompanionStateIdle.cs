using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CompanionStateIdle : State
    {
        CompanionStatePursueTarget pursueTargetState;
        CompanionStateFollowHost followHostState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        private void Awake()
        {
            pursueTargetState = GetComponent<CompanionStatePursueTarget>();
            followHostState = GetComponent<CompanionStateFollowHost>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

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
                                return this;
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
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}