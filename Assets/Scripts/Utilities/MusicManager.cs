using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public Button[] songButtons;
        public AudioClip[] musicTracks;

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
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();
        }

        public void StopMusic()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}