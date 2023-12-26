using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT {
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public AICharacterBossManager boss;

        [Header("BOSS INFORMATION")]
        public int bossID;
        public bool hasBeenDefeatedBOSS;

        public bool bossFightIsActive;
        public bool bossHasBeenAwakened;
        public bool bossHasBeenDefeated;

        public AudioSource audioSource;
        public AudioClip bossMusic;
        public float fadeDuration = 3.0f;
        public float maxVolume = 1.0f;

        public BoxCollider boxColliderBlockedEscape;
        public GameObject flagNextMap;
        public GameObject bonfireofMap;
        public GameObject fireKeeper;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            boxColliderBlockedEscape = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = bossMusic;
            boxColliderBlockedEscape.isTrigger = true;
        }

        private void Update()
        {

        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            boxColliderBlockedEscape.isTrigger = false;

            foreach (var fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;
            boxColliderBlockedEscape.isTrigger = true;
            flagNextMap.SetActive(true);
            fireKeeper.SetActive(true);
            bonfireofMap.SetActive(true);
            bossHealthBar.SetHealthBarInactive();
            StopBossMusic();

            foreach (var fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }

        public void StartBossMusic()
        {
            StartCoroutine(Fade_In_Sound());
        }

        private void StopBossMusic()
        {
            StartCoroutine(Fade_Out_Sound());
        }

        private IEnumerator Fade_In_Sound()
        {
            audioSource.volume = 0.0f;
            audioSource.Play();

            float currentTime = 0.0f;

            while (currentTime < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(0.0f, maxVolume, currentTime / fadeDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator Fade_Out_Sound()
        {
            audioSource.volume = maxVolume;

            float currentTime = 0.0f;

            while (currentTime < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(maxVolume, 0.0f, currentTime / fadeDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }

            if (currentTime > fadeDuration)
            {
                audioSource.mute = true;
            }
        }
    }
}
