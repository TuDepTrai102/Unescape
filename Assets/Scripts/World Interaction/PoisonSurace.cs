using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PoisonSurace : MonoBehaviour
    {
        public List<CharacterManager> characterInsidePoisonSurface;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsidePoisonSurface.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                characterInsidePoisonSurface.Remove(character);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach (CharacterManager character in characterInsidePoisonSurface)
            {
                if (character.characterStatsManager.isPoisoned)
                    return;

                PoisonBuildUpEffect poisonBuildUp = Instantiate(WorldCharacterEffectsManager.instance.poisonBuildUpEffect);

                foreach (var effect in character.characterEffectsManager.timedEffects)
                {
                    if (effect.effectID == poisonBuildUp.effectID)
                    {
                        return;
                    }
                }

                character.characterEffectsManager.timedEffects.Add(poisonBuildUp);
            }
        }
    }
}