using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _CurseSurace : MonoBehaviour
    {
        public List<CharacterManager> characterInsideCurseSurface;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsideCurseSurface.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsideCurseSurface.Remove(character);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach (CharacterManager character in characterInsideCurseSurface)
            {
                if (character.characterStatsManager._isCursed)
                    return;

                _CurseBuildUpEffect bleedBuildUp = Instantiate(WorldCharacterEffectsManager.instance._curseBuildUpEffect);

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