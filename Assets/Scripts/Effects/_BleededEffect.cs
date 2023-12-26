using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Bleed")]
    public class _BleededEffect : CharacterEffect
    {
        public int _bleedDamage = 1;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.characterStatsManager._isBleeded)
            {
                if (character.characterStatsManager._bleedAmount > 0)
                {
                    character.characterStatsManager._bleedAmount -= 1;

                    //DAMAGE PLAYER
                    Debug.Log("BLEED DAMAGE");

                    if (player != null)
                    {
                        player.playerEffectsManager._bleedAmountBar._SetBleedAmount(Mathf.RoundToInt(character.characterStatsManager._bleedAmount));
                    }
                }
                else
                {
                    character.characterStatsManager._isBleeded = false;
                    character.characterStatsManager._bleedAmount = 0;
                    player.playerEffectsManager._bleedAmountBar._SetBleedAmount(0);
                }
            }
            else
            {
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterEffectsManager.RemoveTimedEffectParticle(EffectParticleType._Bleed);
            }
        }
    }
}