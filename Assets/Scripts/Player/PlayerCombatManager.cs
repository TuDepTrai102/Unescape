using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void DrainStaminaBaseOnAttackType()
        {
            if (player.isUsingRightHand)
            {
                if (currentAttackType == AttackType.light)
                {
                    player.playerStatsManager.DeductStamina
                        (player.playerInventoryManager.rightWeapon.baseStaminaCost *
                        player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.heavy)
                {
                    player.playerStatsManager.DeductStamina
                        (player.playerInventoryManager.rightWeapon.baseStaminaCost *
                        player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
            }
            else if (player.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.light)
                {
                    player.playerStatsManager.DeductStamina
                        (player.playerInventoryManager.leftWeapon.baseStaminaCost *
                        player.playerInventoryManager.leftWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.heavy)
                {
                    player.playerStatsManager.DeductStamina
                        (player.playerInventoryManager.leftWeapon.baseStaminaCost *
                        player.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);
                }
            }
        }
    }
}