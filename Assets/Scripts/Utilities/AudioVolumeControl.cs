using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AudioVolumeControl : MonoBehaviour
    {
        public PlayerManager player;
        public AudioSource audioSource;
        public float maxDistance = 10f;
        public float minVolume = 0.0f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            player = FindObjectOfType<PlayerManager>();
        }
        private void Update()
        {
            if (player != null && audioSource != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                float volume = 1f - Mathf.Clamp01(distance / maxDistance);

                volume = Mathf.Max(volume, minVolume);

                audioSource.volume = volume;
            }
        }
    }
}