using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Bleed Build Up")]
    public class _BleedBuildUpEffect : CharacterEffect
    {
        //THE AMOUNT OF BLEED BUILD UP GIVEN BEFORE RESISTANCES ARE CALCULATED, PER GAME TICK
        [SerializeField] float _baseBleedBuildUpAmount = 7;

        //THE AMOUNT OF BLEED TIME THE CHARACTER RECEIVES IF THEY ARE BLEEDED
        [SerializeField] float _bleedAmount = 100;

        //THE AMOUNT OF DAMAGE TAKEN FROM THE BLEED PER TICK IF IT IS BUILT UP TO 100%
        [SerializeField] int _bleedDamagePerTick = 5;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            //BLEED BUILD UP AFTER WE FACTOR IN OUR PLAYERS RESISTANCES
            float finalBleedBuildUp = 0;

            if (character.characterStatsManager._bleedResistance > 0)
            {
                //IF OUR CHARACTER HAS 100 OR MORE BLEED RESIS THEY ARE IMMUNUE
                if (character.characterStatsManager._bleedResistance >= 100)
                {
                    finalBleedBuildUp = 0;
                }
                else
                {
                    float resistencePercentage = character.characterStatsManager._bleedResistance / 100;

                    finalBleedBuildUp = _baseBleedBuildUpAmount - (_baseBleedBuildUpAmount * resistencePercentage);
                }
            }
            else
            {
                finalBleedBuildUp = _baseBleedBuildUpAmount;
            }

            //EACH TICK WE ADD THE BUILD UP AMOUNT TO THE CHARACTERS OVERALL BUILD UP
            character.characterStatsManager._bleedBuildup += finalBleedBuildUp;

            //IF THE CHARACTER IS ALREADY BLEEDED, REMOVE ALL BLEED BUILD UP EFFECTS
            if (character.characterStatsManager._isBleeded)
            {
                character.characterEffectsManager.timedEffects.Remove(this);
            }

            //IF OUR BUILD UP IS 100 OR MORE, BLEED THE CHARACTER
            if (character.characterStatsManager._bleedBuildup >= 100)
            {
                character.characterStatsManager._isBleeded = true;
                character.characterStatsManager._bleedAmount = _bleedAmount;
                character.characterStatsManager._bleedBuildup = 0;

                if (player != null)
                {
                    player.playerEffectsManager._bleedAmountBar._SetBleedAmount(Mathf.RoundToInt(_bleedAmount));
                }

                //WE ALWAYS WANT TO INSTANTIATE A COPY OF A SCRIPTABLE, SO THE ORIGINAL IS NEVER EDITED
                //IF THE ORIGINAL IS EDITED, AN EVERY CHARACTER USES AN ORIGINAL, THEY WILL ALL SHARE THE SAME VALUES
                // (IF ONE IS POISONED FOR 5 SECONDS ALL POISONED SCRIPTABLES WILL READ 5 SECONDS)
                _BleededEffect bleededEffect = Instantiate(WorldCharacterEffectsManager.instance._bleededEffect);
                bleededEffect._bleedDamage = _bleedDamagePerTick;
                character.characterEffectsManager.timedEffects.Add(bleededEffect);
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterSoundFXManager.PlaySoundFX(WorldCharacterEffectsManager.instance._bleedSFX);

                character.characterEffectsManager.AddTimedEffectParticle
                    (Instantiate(WorldCharacterEffectsManager.instance._bleedFX));
            }

            character.characterEffectsManager.timedEffects.Remove(this);
        }
    }
}