using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT
{
    public class WorldSaveBonfire : MonoBehaviour
    {
        public static WorldSaveBonfire instance;
        public WorldSaveGameManager gameManager;

        public CharacterBonfireSave bonfire;

        [Header("STATUS CANVAS")]
        public UIManager playerUI;

        [Header("SAVE DATA WRITER")]
        [SerializeField] public SaveGameDataWriter saveGameDataWriter;

        [Header("CURRENT BONFIRE DATA")]
        public BonfireActivatedData currentBonfireSaveData;
        [SerializeField] private string fileName;

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

            bonfire = FindObjectOfType<CharacterBonfireSave>();
            gameManager = FindObjectOfType<WorldSaveGameManager>();
            playerUI = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            //LOAD ALL POSSIBLE CHARACTER PROFILES

            //LoadBonfire();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveBonfire();
            }
            else if (loadGame)
            {
                loadGame = false;
                //LOAD SAVE DATA
                LoadBonfire();
            }
        }

        //NEW GAME 

        //SAVE GAME
        public void SaveBonfire()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;

            //PASS ALONG OUR CHARACTERS DATA TO THE CURRENT SAVE FILE
            bonfire.SaveBonfireDataToCurrentBonfireSaveData(ref currentBonfireSaveData);

            //WRITE THE CURRENT CHARACTER DATA TO A JSON FILE AND SAVE IT ON THIS DEVICE
            saveGameDataWriter.WriteCharacterDataToSaveFile_Bonfire(currentBonfireSaveData);

            Debug.Log("FILE SAVED AS: " + fileName);
        }

        //LOAD GAME
        public void LoadBonfire()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            bonfire.LoadBonfireDataToCurrentBonfireSaveData(ref currentBonfireSaveData);
        }

        //LOAD BONFIRE 1
        public void LoadBonfire_01()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            StartCoroutine(LoadWorldSceneBonfire01());
        }

        private IEnumerator LoadWorldSceneBonfire01()
        {
            gameManager.loadingSlider.value = 0;
            gameManager.loadingScreen.SetActive(true);
            playerUI.gameObject.SetActive(false);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (gameManager.listOfImageBgLoading != null)
            {
                if (gameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, gameManager.listOfImageBgLoading.Length);
                    gameManager.imageBackgroundLoading.sprite = gameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                gameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    gameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    gameManager.loadingScreen.SetActive(false);
                    playerUI.gameObject.SetActive(true);
                }
                yield return null;
            }

            bonfire.LoadBonfire_01_Data(ref currentBonfireSaveData);
        }

        //LOAD BONFIRE 2
        public void LoadBonfire_02()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            StartCoroutine(LoadWorldSceneBonfire02());
        }

        private IEnumerator LoadWorldSceneBonfire02()
        {
            gameManager.loadingSlider.value = 0;
            gameManager.loadingScreen.SetActive(true);
            playerUI.gameObject.SetActive(false);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(2);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (gameManager.listOfImageBgLoading != null)
            {
                if (gameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, gameManager.listOfImageBgLoading.Length);
                    gameManager.imageBackgroundLoading.sprite = gameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                gameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    gameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    gameManager.loadingScreen.SetActive(false);
                    playerUI.gameObject.SetActive(true);
                }
                yield return null;
            }

            bonfire.LoadBonfire_02_Data(ref currentBonfireSaveData);
        }

        //LOAD BONFIRE 3
        public void LoadBonfire_03()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            StartCoroutine(LoadWorldSceneBonfire03());
        }

        private IEnumerator LoadWorldSceneBonfire03()
        {
            gameManager.loadingSlider.value = 0;
            gameManager.loadingScreen.SetActive(true);
            playerUI.gameObject.SetActive(false);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(3);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (gameManager.listOfImageBgLoading != null)
            {
                if (gameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, gameManager.listOfImageBgLoading.Length);
                    gameManager.imageBackgroundLoading.sprite = gameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                gameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    gameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    gameManager.loadingScreen.SetActive(false);
                    playerUI.gameObject.SetActive(true);
                }
                yield return null;
            }

            bonfire.LoadBonfire_03_Data(ref currentBonfireSaveData);
        }

        //LOAD BONFIRE 4
        public void LoadBonfire_04()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            StartCoroutine(LoadWorldSceneBonfire04());
        }

        private IEnumerator LoadWorldSceneBonfire04()
        {
            gameManager.loadingSlider.value = 0;
            gameManager.loadingScreen.SetActive(true);
            playerUI.gameObject.SetActive(false);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(4);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (gameManager.listOfImageBgLoading != null)
            {
                if (gameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, gameManager.listOfImageBgLoading.Length);
                    gameManager.imageBackgroundLoading.sprite = gameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                gameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    gameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    gameManager.loadingScreen.SetActive(false);
                    playerUI.gameObject.SetActive(true);
                }
                yield return null;
            }

            bonfire.LoadBonfire_04_Data(ref currentBonfireSaveData);
        }

        //LOAD BONFIRE 5
        public void LoadBonfire_05()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_bonfire = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_bonfire = fileName;
            currentBonfireSaveData = saveGameDataWriter.LoadCharacterDataFromJson_Bonfire();

            StartCoroutine(LoadWorldSceneBonfire05());
        }

        private IEnumerator LoadWorldSceneBonfire05()
        {
            gameManager.loadingSlider.value = 0;
            gameManager.loadingScreen.SetActive(true);
            playerUI.gameObject.SetActive(false);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(5);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (gameManager.listOfImageBgLoading != null)
            {
                if (gameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, gameManager.listOfImageBgLoading.Length);
                    gameManager.imageBackgroundLoading.sprite = gameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                gameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    gameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    gameManager.loadingScreen.SetActive(false);
                    playerUI.gameObject.SetActive(true);
                }
                yield return null;
            }

            bonfire.LoadBonfire_05_Data(ref currentBonfireSaveData);
        }
    }
}