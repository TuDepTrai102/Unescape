using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("WEAPON BUFF DAMAGE")]
        public float physicalBuffDamage;
        public float fireBuffDamage;
        public float _lightningBuffDamage;
        public float _bleedBuffDamage;
        public float _darkBuffDamage;
        public float _magicBuffDamage;
        public float poiseBuffDamage;

        protected override void DealDamage(CharacterManager enemyManager)
        {
            float finalPhysicalDamage = physicalDamage + physicalBuffDamage;
            float finalFireDamage = fireDamage + fireBuffDamage;
            float _finalLightningDamage = _lightningDamage + _lightningBuffDamage;
            float _finalBleedDamage = _bleedDamage + _bleedBuffDamage;
            float _finalDarkDamage = _darkDamage + _darkBuffDamage;
            float _finalMagicDamage = _magicDamage + _magicBuffDamage;
            float finalDamage = 0;

            //GET ATTACK TYPE FROM ATTACKING CHARACTER

            //APPLY DAMAGE MULTIPLIERS
            if (characterManager.isUsingRightHand) //IF WE ARE USING RIGHT HAND, WE COMPARE THE RIGHT WEAPON MODIFIERS
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalDamage += finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += finalFireDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += _finalLightningDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += _finalDarkDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += _finalBleedDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += _finalMagicDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalDamage += finalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalDamage += finalPhysicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += finalFireDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalLightningDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalDarkDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalBleedDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalMagicDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalDamage += finalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                }
            }
            else if (characterManager.isUsingLeftHand) //IF WE ARE USING LEFT HAND, WE COMPARE THE LEFT WEAPON MODIFIERS
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalDamage += finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += finalFireDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += _finalLightningDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += _finalDarkDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += _finalBleedDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += _finalMagicDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalDamage += finalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalDamage += finalPhysicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += finalFireDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalLightningDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalDarkDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalBleedDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += _finalMagicDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalDamage += finalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                }
            }

            TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            takeDamageEffect.physicalDamage = finalPhysicalDamage;
            takeDamageEffect.fireDamage = finalFireDamage;
            takeDamageEffect._lightningDamage = _finalLightningDamage;
            takeDamageEffect._darkDamage = _finalDarkDamage;
            takeDamageEffect._bleedDamage = _finalBleedDamage;
            takeDamageEffect._magicDamage = _finalMagicDamage;
            takeDamageEffect.poiseDamage = poiseDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.angleHitFrom = angleHitFrom;
            enemyManager.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
        }
    }
}