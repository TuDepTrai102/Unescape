using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Curse")]
    public class _CursedEffect : CharacterEffect
    {
        public int _curseDamage = 1;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.characterStatsManager._isCursed)
            {
                if (character.characterStatsManager._curseAmount > 0)
                {
                    character.characterStatsManager._curseAmount -= 1;

                    //DAMAGE PLAYER
                    Debug.Log("CURSE DAMAGE");

                    if (player != null)
                    {
                        player.playerEffectsManager._curseAmountBar._SetCurseAmount(Mathf.RoundToInt(character.characterStatsManager._curseAmount));
                    }
                }
                else
                {
                    character.characterStatsManager._isCursed = false;
                    character.characterStatsManager._curseAmount = 0;
                    player.playerEffectsManager._curseAmountBar._SetCurseAmount(0);
                }
            }
            else
            {
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterEffectsManager.RemoveTimedEffectParticle(EffectParticleType._Curse);
            }
        }
    }
}