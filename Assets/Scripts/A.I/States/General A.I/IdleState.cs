using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class IdleState : State
    {
        PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersToIgnoreForLineOfSight;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
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
                            if (Physics.Linecast
                                (aiCharacter.lockOnTransform.position,
                                targetCharacter.lockOnTransform.position,
                                layersToIgnoreForLineOfSight))
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