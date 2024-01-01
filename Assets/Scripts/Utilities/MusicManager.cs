using NT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public Button[] songButtons;
        public AudioClip[] musicTracks;
        public BonfireInteractable bonfireMusic;

        void Start()
        {
            for (int i = 0; i < songButtons.Length; i++)
            {
                int index = i;
                songButtons[i].onClick.AddListener(() => PlayMusic(index));
            }
        }

        void PlayMusic(int trackIndex)
        {
            bonfireMusic = FindObjectOfType<BonfireInteractable>();

            if (bonfireMusic != null)
            {
                if (bonfireMusic.audioSource.isPlaying)
                {
                    bonfireMusic.audioSource.Stop();
                }
            }

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();
        }

        public void StopMusic()
        {
            bonfireMusic = FindObjectOfType<BonfireInteractable>();

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (bonfireMusic != null)
            {
                bonfireMusic.audioSource.PlayOneShot(bonfireMusic.bonfireActivationSoundFX);
            }
        }
    }
}