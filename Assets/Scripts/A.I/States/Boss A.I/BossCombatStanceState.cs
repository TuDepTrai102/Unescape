using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class BossCombatStanceState : CombatStanceState
    {
        [Header("SECOND PHASE ATTACKS")]
        public bool hasPhaseShifted;
        public AICharacterAttackAction[] secondPhaseAttacks;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void GetNewAttack(AICharacterManager enemy)
        {
            if (hasPhaseShifted)
            {
                int maxScore = 0;

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    AICharacterAttackAction enemyAttackAction = secondPhaseAttacks[i];

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

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    AICharacterAttackAction enemyAttackAction = secondPhaseAttacks[i];

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
            else
            {
                base.GetNewAttack(enemy);
            }
        }
    }
}