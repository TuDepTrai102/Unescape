using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT
{
    public class _StartGame : MonoBehaviour
    {
        PlayerManager player;
        public WorldSaveBonfire worldSaveBonfire;
        public WorldSaveGameManager worldSaveGameManager;
        public _WorldSaveStartScreen worldSaveStartScreen;
        public GameObject createCharacterUI;
        public GameObject _plane;
        public GameObject _noteBoard;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            worldSaveBonfire = FindObjectOfType<WorldSaveBonfire>();
            worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
            worldSaveStartScreen = FindObjectOfType<_WorldSaveStartScreen>();
            StartCoroutine(_MenuSceneIssue());
        }

        public void _StartGameButtonClicked()
        {
            //player._worldSaveGameManager.LoadGame();
            worldSaveGameManager.LoadGame();
            worldSaveBonfire.LoadBonfire();
            _plane.SetActive(false);
            _noteBoard.SetActive(true);
        }

        public void _CreateCharatcer()
        {
            //player._worldSaveGameManager.SaveGame();
            //player._worldSaveStartScreen.SaveGame();

            worldSaveGameManager.SaveGame();
            worldSaveStartScreen.SaveGame();
        }

        public void _ContinueGame()
        {
            //player._worldSaveGameManager.LoadGame();
            worldSaveGameManager.LoadGame();
            worldSaveBonfire.LoadBonfire();
            _plane.SetActive(false);
            _noteBoard.SetActive(false);
        }

        private IEnumerator _MenuSceneIssue() //ERROR WHEN PLAYMODE, THIS PLAYER UI ON CREATOR UI...
        {
            createCharacterUI.SetActive(false);

            yield return null;

            createCharacterUI.SetActive(true);
        }
    }
}