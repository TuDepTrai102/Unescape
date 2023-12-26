using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Curse Build Up")]
    public class _CurseBuildUpEffect : CharacterEffect
    {
        //THE AMOUNT OF CURSE BUILD UP GIVEN BEFORE RESISTANCES ARE CALCULATED, PER GAME TICK
        [SerializeField] float _baseCurseBuildUpAmount = 7;

        //THE AMOUNT OF CURSE TIME THE CHARACTER RECEIVES IF THEY ARE CURSED
        [SerializeField] float _curseAmount = 100;

        //THE AMOUNT OF DAMAGE TAKEN FROM THE CURSE PER TICK IF IT IS BUILT UP TO 100%
        [SerializeField] int _curseDamagePerTick = 5;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            //CURSE BUILD UP AFTER WE FACTOR IN OUR PLAYERS RESISTANCES
            float finalBleedBuildUp = 0;

            if (character.characterStatsManager._curseResistance > 0)
            {
                //IF OUR CHARACTER HAS 100 OR MORE CURSE RESIS THEY ARE IMMUNUE
                if (character.characterStatsManager._curseResistance >= 100)
                {
                    finalBleedBuildUp = 0;
                }
                else
                {
                    float resistencePercentage = character.characterStatsManager._curseResistance / 100;

                    finalBleedBuildUp = _baseCurseBuildUpAmount - (_baseCurseBuildUpAmount * resistencePercentage);
                }
            }
            else
            {
                finalBleedBuildUp = _baseCurseBuildUpAmount;
            }

            //EACH TICK WE ADD THE BUILD UP AMOUNT TO THE CHARACTERS OVERALL BUILD UP
            character.characterStatsManager._curseBuildup += finalBleedBuildUp;

            //IF THE CHARACTER IS ALREADY CURSED, REMOVE ALL CURSE BUILD UP EFFECTS
            if (character.characterStatsManager._isCursed)
            {
                character.characterEffectsManager.timedEffects.Remove(this);
            }

            //IF OUR BUILD UP IS 100 OR MORE, CURSE THE CHARACTER
            if (character.characterStatsManager._curseBuildup >= 100)
            {
                character.characterStatsManager._isCursed = true;
                character.characterStatsManager._curseAmount = _curseAmount;
                character.characterStatsManager._curseBuildup = 0;

                if (player != null)
                {
                    player.playerEffectsManager._curseAmountBar._SetCurseAmount(Mathf.RoundToInt(_curseAmount));
                }

                //WE ALWAYS WANT TO INSTANTIATE A COPY OF A SCRIPTABLE, SO THE ORIGINAL IS NEVER EDITED
                //IF THE ORIGINAL IS EDITED, AN EVERY CHARACTER USES AN ORIGINAL, THEY WILL ALL SHARE THE SAME VALUES
                // (IF ONE IS POISONED FOR 5 SECONDS ALL POISONED SCRIPTABLES WILL READ 5 SECONDS)
                _CursedEffect cursedEffect = Instantiate(WorldCharacterEffectsManager.instance._cursedEffect);
                cursedEffect._curseDamage = _curseDamagePerTick;
                character.characterEffectsManager.timedEffects.Add(cursedEffect);
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterSoundFXManager.PlaySoundFX(WorldCharacterEffectsManager.instance._curseSFX);

                character.characterEffectsManager.AddTimedEffectParticle
                    (Instantiate(WorldCharacterEffectsManager.instance._curseFX));
            }

            character.characterEffectsManager.timedEffects.Remove(this);
        }
    }
}