using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(AICharacterManager aiCharacter);
    }
}