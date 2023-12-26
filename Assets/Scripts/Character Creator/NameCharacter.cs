using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class NameCharacter : MonoBehaviour
    {
        public CharacterManager character;
        public InputField inputField;
        public Text nameButtonText;

        private void Awake()
        {
            character = FindObjectOfType<CharacterManager>();
        }

        public void NameMyCharacter()
        {
            character.characterStatsManager.characterName = inputField.text;
            
            if (character.characterStatsManager.characterName == "")
            {
                character.characterStatsManager.characterName = "Nameless";
            }

            nameButtonText.text = character.characterStatsManager.characterName;
        }
    }
}