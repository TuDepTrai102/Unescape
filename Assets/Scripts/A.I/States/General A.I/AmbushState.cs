using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AmbushState : State
    {
        AICharacterManager ai;
        PursueTargetState pursueTargetState;
        _AcceptFightingBoss _acceptFighting;

        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetState>();
            _acceptFighting = GetComponentInChildren<_AcceptFightingBoss>();
            ai = GetComponentInParent<AICharacterManager>();
        }

        private void Update()
        {

        }

        public override State Tick(AICharacterManager enemy)
        {
            if (isSleeping && enemy.isInteracting == false &&
                !_acceptFighting._acceptFightingBool)
            {
                enemy.aiCharacterAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
                enemy.animator.SetBool("isInvulnerable", true);
            }

            Collider[] colliders = Physics.OverlapSphere
                (enemy.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager potentialTarget = colliders[i].transform.GetComponent<CharacterManager>();

                if (potentialTarget != null)
                {
                    Vector3 targetDirection = potentialTarget.transform.position - enemy.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                    if (viewableAngle > enemy.minimumDetectionAngle &&
                        viewableAngle < enemy.maximumDetectionAngle)
                    {
                        if (_acceptFighting._acceptFightingBool)
                        {
                            enemy.currentTarget = potentialTarget;
                            isSleeping = false;
                            enemy.aiCharacterAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                        }
                    }
                }
            }

            if (enemy.currentTarget != null)
            {
                _acceptFighting._acceptFightingBool = true;
                _acceptFighting.worldEventManager.bossHasBeenAwakened = true;
                isSleeping = false;
                enemy.aiCharacterAnimatorManager.PlayTargetAnimation(wakeAnimation, true);

                _acceptFighting.worldEventManager.StartBossMusic();

                if (_acceptFighting.worldEventManager.bossHasBeenAwakened)
                {
                    _acceptFighting.worldEventManager.bossHealthBar.SetUIHealthBarToActive();
                }

                Destroy(_acceptFighting);

                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}