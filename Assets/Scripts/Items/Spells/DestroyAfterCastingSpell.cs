using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        CharacterManager characterCastingSpell;

        private void Awake()
        {
            characterCastingSpell = GetComponentInParent<CharacterManager>();
        }

        private void Update()
        {
            if (characterCastingSpell.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }
}