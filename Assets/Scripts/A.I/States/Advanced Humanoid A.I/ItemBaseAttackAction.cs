using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
    public class ItemBaseAttackAction : ScriptableObject
    {
        [Header("ATTACK TYPE")]
        public AIAttackActionType attackActionType = AIAttackActionType.meleeAttackAction;
        public AttackType attackType = AttackType.light;

        [Header("ACTION COMBO SETTINGS")]
        public bool actionCanCombo = false;

        [Header("LIGHT HAND OR LEFT HAND ACTION")]
        public bool isRightHandedAction = true;

        [Header("ACTION SETTINGS")]
        public int attackScore = 3;
        public float recoveryTime = 2;
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;
        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;

        public void PerformAttackAction(AICharacterManager enemy)
        {
            if (isRightHandedAction)
            {
                enemy.UpdateWhichHandCharacterIsUsing(true);
                PerformRightHandItemActionBasedOnAttackType(enemy);
            }
            else
            {
                enemy.UpdateWhichHandCharacterIsUsing(false);
                PerformLeftHandItemActionBasedOnAttackType(enemy);
            }
        }

        //DECIDE WHICH HAND PERFORMS ACTION
        private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager enemy)
        {
            if (attackActionType == AIAttackActionType.meleeAttackAction)
            {
                //PERFORM RIGHT HAND MELEE ACTION
                PerformRightHandMeleeAction(enemy);
            }
            else if (attackActionType == AIAttackActionType.rangedAttackAction)
            {
                //PERFORM RIGHT HAND RANGED ACTION
                PerformRightHandRangedAction(enemy);
            }
        }

        private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager enemy)
        {
            if (attackActionType == AIAttackActionType.meleeAttackAction)
            {
                //PERFORM LEFT HAND MELEE ACTION
            }
            else if (attackActionType == AIAttackActionType.rangedAttackAction)
            {
                //PERFORM LEFT HAND RANGED ACTION
            }
        }

        //RIGHT HAND ACTIONS
        private void PerformRightHandMeleeAction(AICharacterManager enemy)
        {
            if (enemy.isTwoHandingWeapon)
            {
                if (attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_E_Action.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_T_Action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_E_Action.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_T_Action.PerformAction(enemy);
                }
            }
        }

        private void PerformRightHandRangedAction(AICharacterManager enemy)
        {
            if (enemy.isTwoHandingWeapon)
            {
                if (attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_E_Action.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_T_Action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.light)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_E_Action.PerformAction(enemy);
                }
                else if (attackType == AttackType.heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.oh_tap_T_Action.PerformAction(enemy);
                }
            }
        }

        //LEFT HAND ACTIONS
        private void PerformLeftHandMeleeAction(AICharacterManager enemy)
        {

        }
    }
}