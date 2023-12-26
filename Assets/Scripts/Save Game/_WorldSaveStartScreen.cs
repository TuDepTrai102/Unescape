using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT
{
    public class _WorldSaveStartScreen : MonoBehaviour
    {
        public static _WorldSaveStartScreen instance;

        public _CharacterSceneMenu player_Start;

        [Header("SAVE DATA WRITER")]
        [SerializeField] public SaveGameDataWriter saveGameDataWriter;

        [Header("CURRENT CHARACTER DATA")]
        //CHARACTER SLOT #
        public _CharacterStartScreenData currentCharacterSceneData;
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

            player_Start = FindObjectOfType<_CharacterSceneMenu>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            //LOAD ALL POSSIBLE CHARACTER PROFILES

            //LOAD GAME
            LoadGame();
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
            saveGameDataWriter.saveDataDirectoryPath_02 = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_02 = fileName;

            //PASS ALONG OUR CHARACTERS DATA TO THE CURRENT SAVE FILE
            player_Start.SaveCharacterDataToCurrentSaveData_MENU(ref currentCharacterSceneData);

            //WRITE THE CURRENT CHARACTER DATA TO A JSON FILE AND SAVE IT ON THIS DEVICE
            saveGameDataWriter.WriteCharacterDataToSaveFile_START(currentCharacterSceneData);

            Debug.Log("FILE SAVED AS: " + fileName);
        }

        //LOAD GAME
        public void LoadGame()
        {
            //DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath_02 = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName_02 = fileName;
            currentCharacterSceneData = saveGameDataWriter.LoadCharacterDataFromJson_START();

            player_Start.LoadCharacterDataFromCurrentCharacterSaveData_MENU(ref currentCharacterSceneData);
        }
    }
}