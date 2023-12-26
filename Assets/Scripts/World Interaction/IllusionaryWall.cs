using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Material illusionaryWallMaterial;
        public float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;

        private void Update()
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        public void FadeIllusionaryWall()
        {
            alpha = illusionaryWallMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            illusionaryWallMaterial.color = fadedWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusionaryWallSound);
            }

            if (alpha <= 0)
            {
                Destroy(this);
            }
        }
    }
}