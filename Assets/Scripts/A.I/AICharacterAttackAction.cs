using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "A.I/A.I Actions/Attack Action")]
    public class AICharacterAttackAction : AICharacterAction
    {
        public bool canCombo;
        public AICharacterAttackAction comboAction;

        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}