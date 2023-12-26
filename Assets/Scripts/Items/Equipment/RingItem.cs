using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Ring")]
    public class RingItem : Item
    {
        [SerializeField] StaticCharacterEffect effect;
        private StaticCharacterEffect effectClone;

        //FOR USER UI
        [Header("ITEM EFFECT DESCRIPTION")]
        [TextArea] public string itemEffectInformation;

        //CALLED WHEN EQUIPPING A RING, ADDS THE RING'S EFFECT TO OUR CHARACTER
        public void EquipRing(CharacterManager character)
        {
            //WE CREATE A CLONE SO THE BASE SCRIPTABLE OBJECT IS NOT EFFECTED IF IN THE FUTURE WE CHANGE ANY OF ITS VARIABLES
            effectClone = Instantiate(effect);

            character.characterEffectsManager.AddStaticEffect(effectClone);
        }

        //CALLED WHEN UNEQUIPPING A RING, REMOVES EFFECT FROM OUR CHARACTER
        public void UnEquipRing(CharacterManager character)
        {
            character.characterEffectsManager.RemoveStaticEffect(effect.effectID);
        }
    }
}