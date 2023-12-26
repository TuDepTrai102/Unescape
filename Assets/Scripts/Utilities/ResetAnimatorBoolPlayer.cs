using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT {
    public class ResetAnimatorBoolPlayer : StateMachineBehaviour
    {
        public string isInvulnerableBool = "isInvulnerable";
        public bool isInvulnerableStatus = false;

        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringSpellStatus = false;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string canDoComboBool = "canDoCombo";
        public bool canDoComboStatus = false;

        public string isMirroredBool = "isMirrored";
        public bool isMirroredStatus = false;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();

            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;
            character.isAttacking = false;
            character.isBeingBackstabbed = false;
            character.isBeingRiposted = false;
            character.isPerformingBackstab = false;
            character.isPerformingRiposte = false;
            character.canBeParried = false;
            character.canBeRiposted = false;
            character.canRoll = true;
            character.isJumping = false;
            character._isRushIn = false;
            character._isChargingAttack = false;

            //AFTER THE DAMAGE ANIMATION ENDS, RESET OUR PREVIOUS POISE DAMAGE TO ZERO
            character.characterCombatManager.previousPoiseDamageTaken = 0;

            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(canDoComboBool, canDoComboStatus);
            animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
            animator.SetBool(isMirroredBool, isMirroredStatus);
        }
    }
}