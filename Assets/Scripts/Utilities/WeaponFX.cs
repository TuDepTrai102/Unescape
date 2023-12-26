using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("WEAPON FX")]
        public ParticleSystem normalWeaponTrail;
        // fire weapon trail
        // lightning weapon trail
        // dark weapon trail

        public void PlayWeaponFX()
        {
            normalWeaponTrail.Stop();

            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
            }
        }
    }
}