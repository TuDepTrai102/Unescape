using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [System.Serializable]
    public class ClassStats
    {
        public string playerClass;
        public int classLevel;

        [TextArea]
        public string classDescription;

        [Header("CLASS STATS")]
        public int strengthLevel;

        //STAMINA LEVEL
        public int staminaLevel;

        //HEALTH LEVEL
        public int healthLevel;

        //FOCUS POINT LEVEL
        public int focusPointLevel;
    }
}