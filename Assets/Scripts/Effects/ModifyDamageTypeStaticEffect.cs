using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Modify Damage Type")]
    public class ModifyDamageTypeStaticEffect : StaticCharacterEffect
    {
        [Header("DAMAGE TYPE EFFECTED")]
        [SerializeField] DamageType damageType;
        [SerializeField] int modifedValue = 22;

        //WHEN ADDING THE EFFECT, WE ADD THE MODIFIED VALUE AMOUNT TO OUR RESPECTIVE DAMAGE TYPE MODIFIED
        public override void AddStaticEffect(CharacterManager character)
        {
            base.AddStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical: 
                    character.characterStatsManager.physicalDamagePercentageModifier += modifedValue;
                    break;

                case DamageType.Fire: 
                    character.characterStatsManager.fireDamagePercentageModifier += modifedValue;
                    break;

                case DamageType._Lightning:
                    character.characterStatsManager._lightningDamagePercentageModifier += modifedValue;
                    break;

                case DamageType._Dark:
                    character.characterStatsManager._darkDamagePercentageModifier += modifedValue;
                    break;

                case DamageType._Magic:
                    character.characterStatsManager._magicDamagePercentageModifier += modifedValue;
                    break;

                case DamageType._Bleed:
                    character.characterStatsManager._bleedDamagePercentageModifier += modifedValue;
                    break;

                default: 
                    break;
            }
        }

        //WHEN REMOVE THE EFFECT, WE SUBTRACT THE AMOUNT WE ADDED
        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical:
                    character.characterStatsManager.physicalDamagePercentageModifier -= modifedValue;
                    break;

                case DamageType.Fire:
                    character.characterStatsManager.fireDamagePercentageModifier -= modifedValue;
                    break;

                case DamageType._Lightning:
                    character.characterStatsManager._lightningDamagePercentageModifier -= modifedValue;
                    break;

                case DamageType._Dark:
                    character.characterStatsManager._darkDamagePercentageModifier -= modifedValue;
                    break;

                case DamageType._Magic:
                    character.characterStatsManager._magicDamagePercentageModifier -= modifedValue;
                    break;

                case DamageType._Bleed:
                    character.characterStatsManager._bleedDamagePercentageModifier -= modifedValue;
                    break;

                default:
                    break;
            }
        }
    }
}