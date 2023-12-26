using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NT
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("LOADING CANVAS")]
        public CreationCanvas creationCanvas;
        public GameObject loadingScreen;
        public ThemeMusic musicThemeMainMenu;
        public Image imageBackgroundLoading;
        public Sprite[] listOfImageBgLoading;
        public Slider loadingSlider;

        [Header("SAVE DATA WRITER")]
        [SerializeField] SaveGameDataWriter saveGameDataWriter;

        [Header("CURRENT CHARACTER DATA")]
        //CHARACTER SLOT #
        public CharacterSaveData currentCharacterSaveData;
        [SerializeField] private string fileName = "nguyentu_test_01.json";

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            player = FindObjectOfType<PlayerManager>();
            creationCanvas = FindObjectOfType<CreationCanvas>();
            musicThemeMainMenu = FindObjectOfType<ThemeMusic>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loadingScreen);
            //LOAD ALL POSSIBLE CHARACTER PROFILES

            //LOAD GAME
            //LoadGame();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                //SAVE GAME
                SaveGame();
            }
            else if (loadGame)
            {
                loadGame = false;
                //LOAD SAVE DATA
                LoadGame();
            }
        }

        //NEW GAME 

        //SAVE GAME
        public void SaveGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;

            //PASS ALONG OUR CHARACTERS DATA TO THE CURRENT SAVE FILE
            player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

            //WRITE THE CURRENT CHARACTER DATA TO A JSON FILE AND SAVE IT ON THIS DEVICE
            saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

            Debug.Log("FILE SAVED AS: " + fileName);
        }

        //LOAD GAME
        public void LoadGame()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldSceneAsynchronously());
        }

        private IEnumerator LoadWorldSceneAsynchronously()
        {
            if (creationCanvas == null)
            {
                creationCanvas = FindObjectOfType<CreationCanvas>();
            }

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            creationCanvas.gameObject.SetActive(false);
            player.uiManager.gameObject.SetActive(false);
            Destroy(musicThemeMainMenu.gameObject);

            if (currentCharacterSaveData.sceneSaved == 0)
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
                loadOperation.allowSceneActivation = false;
                float loadingProgress = 0;

                if (listOfImageBgLoading != null)
                {
                    if (listOfImageBgLoading.Length > 0)
                    {
                        int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                        imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                    }
                }

                while (!loadOperation.isDone)
                {
                    loadingProgress = Mathf.MoveTowards(loadingProgress,loadOperation.progress, Time.deltaTime);
                    //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                    loadingSlider.value = loadingProgress;
                    if (loadingProgress >= 0.9f)
                    {
                        loadingSlider.value = 1;
                        loadOperation.allowSceneActivation = true;
                        loadingScreen.SetActive(false);
                        player.uiManager.gameObject.SetActive(true);
                    }
                    yield return null;
                }
            }
            else
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterSaveData.sceneSaved);
                loadOperation.allowSceneActivation = false;
                float loadingProgress = 0;

                if (listOfImageBgLoading != null)
                {
                    if (listOfImageBgLoading.Length > 0)
                    {
                        int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                        imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                    }
                }

                while (!loadOperation.isDone)
                {
                    loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                    //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                    loadingSlider.value = loadingProgress;
                    if (loadingProgress >= 0.9f)
                    {
                        loadingSlider.value = 1;
                        loadOperation.allowSceneActivation = true;
                        loadingScreen.SetActive(false);
                        player.uiManager.gameObject.SetActive(true);
                    }
                    yield return null;
                }
            }

            player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
        }

        //LOAD MAP 1 (RESPAWN)
        public void LoadMap1()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_01());
        }

        private IEnumerator LoadWorldScene_01()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap1(ref currentCharacterSaveData);
        }


        //LOAD MAP 2
        public void LoadMap2()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_02());
        }

        private IEnumerator LoadWorldScene_02()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(2);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap2(ref currentCharacterSaveData);
        }

        //MAP 2 BACK
        public void LoadMap2_back()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_02_back());
        }

        private IEnumerator LoadWorldScene_02_back()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap2_Back(ref currentCharacterSaveData);
        }


        //LOAD MAP 3
        public void LoadMap3()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_03());
        }

        private IEnumerator LoadWorldScene_03()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(3);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap3(ref currentCharacterSaveData);
        }

        //MAP 3 BACK
        public void LoadMap3_back()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_03_back());
        }

        private IEnumerator LoadWorldScene_03_back()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(2);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap3_Back(ref currentCharacterSaveData);
        }


        //LOAD MAP 4
        public void LoadMap4()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_04());
        }

        private IEnumerator LoadWorldScene_04()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(4);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap4(ref currentCharacterSaveData);
        }

        //MAP 4 BACK
        public void LoadMap4_back()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_04_back());
        }

        private IEnumerator LoadWorldScene_04_back()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(3);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap3_Back(ref currentCharacterSaveData);
        }


        //LOAD MAP 5
        public void LoadMap5()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_05());
        }

        private IEnumerator LoadWorldScene_05()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(5);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap5(ref currentCharacterSaveData);
        }

        //MAP 5 BACK
        public void LoadMap5_back()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_05_back());
        }

        private IEnumerator LoadWorldScene_05_back()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(4);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadMap5_Back(ref currentCharacterSaveData);
        }

        //ENDING MAP
        public void LoadEndingMap()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldScene_Ending());
        }

        private IEnumerator LoadWorldScene_Ending()
        {
            loadingSlider.value = 0;
            loadingScreen.SetActive(true);
            player.uiManager.gameObject.SetActive(false);

            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(6);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (listOfImageBgLoading != null)
            {
                if (listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, listOfImageBgLoading.Length);
                    imageBackgroundLoading.sprite = listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                    player.uiManager.gameObject.SetActive(true);
                }
                yield return null;
            }

            player.LoadEndScene(ref currentCharacterSaveData);
        }
    }
}