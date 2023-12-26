using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _BleedSurace : MonoBehaviour
    {
        public List<CharacterManager> characterInsideBleedSurface;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsideBleedSurface.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsideBleedSurface.Remove(character);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach (CharacterManager character in characterInsideBleedSurface)
            {
                if (character.characterStatsManager._isBleeded)
                    return;

                _BleedBuildUpEffect bleedBuildUp = Instantiate(WorldCharacterEffectsManager.instance._bleedBuildUpEffect);

                foreach (var effect in character.characterEffectsManager.timedEffects)
                {
                    if (effect.effectID == bleedBuildUp.effectID)
                    {
                        return;
                    }
                }

                character.characterEffectsManager.timedEffects.Add(bleedBuildUp);
            }
        }
    }
}