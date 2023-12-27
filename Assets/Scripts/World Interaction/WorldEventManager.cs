using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT {
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        public UIBossHealthBar bossHealthBar;
        public AICharacterBossManager boss;
        public _AcceptFightingBoss acceptFightingBoss;

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
        public BoxCollider[] boxHiddenColliderBlocked;
        public GameObject flagNextMap;
        public GameObject bonfireofMap;
        public GameObject fireKeeper;
        public GameObject theBOSS_prebab;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            acceptFightingBoss = FindObjectOfType<_AcceptFightingBoss>();
            boxColliderBlockedEscape = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            //IF THE SAVE DATA DOES NOT CONTAIN THIS ITEM, WE MUST HAVE NEVER LOOTED IT,
            //SO WE ADD IT TO THE LIST AND LIST IT AS NOT LOOTED
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Add(bossID, false);
            }

            hasBeenDefeatedBOSS = WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld[bossID];

            if (hasBeenDefeatedBOSS)
            {
                theBOSS_prebab.SetActive(false);
                BossHasBeenDefeated();
            }

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = bossMusic;
            boxColliderBlockedEscape.isTrigger = true;
        }

        private void Update()
        {

        }

        public void ActivateBossFight()
        {
            if (!bossHasBeenDefeated)
            {
                bossFightIsActive = true;
                boxColliderBlockedEscape.isTrigger = false;

                foreach (var fogWall in fogWalls)
                {
                    fogWall.ActivateFogWall();
                }
            }
            else
            {
                BossHasBeenDefeated();
            }
        }

        public void BossHasBeenDefeated()
        {
            //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
            if (WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Remove(bossID);
            }

            //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
            WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Add(bossID, true);

            bossHasBeenDefeated = true;
            bossFightIsActive = false;
            Destroy(boxColliderBlockedEscape);

            foreach (var collider in boxHiddenColliderBlocked)
            {
                Destroy(collider);
            }

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
