using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT 
{
    public class _QuitGame : MonoBehaviour
    {
        public PlayerManager playerManager;
        public _WorldSaveStartScreen _worldSaveStartScreen;
        public WorldSaveGameManager _worldSaveGameManager;
        public WorldSaveBonfire worldSaveBonfire;

        public GameObject player;
        public GameObject uiManager;
        public GameObject cameraHandler;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            _worldSaveStartScreen = FindObjectOfType<_WorldSaveStartScreen>();
            _worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
            worldSaveBonfire = FindObjectOfType<WorldSaveBonfire>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);    
        }

        public void _BackToMainMenu()
        {
            StartCoroutine(loadingScreenBackToMainMenu());
        }

        IEnumerator loadingScreenBackToMainMenu()
        {
            _worldSaveGameManager.loadingSlider.value = 0;
            _worldSaveGameManager.loadingScreen.SetActive(true);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(0);
            loadOperation.allowSceneActivation = false;
            float loadingProgress = 0;

            if (_worldSaveGameManager.listOfImageBgLoading != null)
            {
                if (_worldSaveGameManager.listOfImageBgLoading.Length > 0)
                {
                    int randomIndex = Random.Range(0, _worldSaveGameManager.listOfImageBgLoading.Length);
                    _worldSaveGameManager.imageBackgroundLoading.sprite = _worldSaveGameManager.listOfImageBgLoading[randomIndex];
                }
            }

            while (!loadOperation.isDone)
            {
                loadingProgress = Mathf.MoveTowards(loadingProgress, loadOperation.progress, Time.deltaTime);
                //ENABLE LOADING SCREEN & PASS THE LOADING PROGRESS TO A SLIDER/LOADING EFFECT
                _worldSaveGameManager.loadingSlider.value = loadingProgress;
                if (loadingProgress >= 0.9f)
                {
                    _worldSaveGameManager.loadingSlider.value = 1;
                    loadOperation.allowSceneActivation = true;
                    _worldSaveGameManager.loadingScreen.SetActive(false);
                    StartCoroutine(_LoadGEAR_MainScene());
                }
                yield return null;
            }
        }

        IEnumerator _LoadGEAR_MainScene()
        {
            Destroy(player);
            Destroy(uiManager);
            Destroy(cameraHandler);

            yield return new WaitForSeconds(0.55f);

            _worldSaveStartScreen.player_Start = FindObjectOfType<_CharacterSceneMenu>();
            _worldSaveGameManager.creationCanvas = FindObjectOfType<CreationCanvas>();
            _worldSaveGameManager.musicThemeMainMenu = FindObjectOfType<ThemeMusic>();
            worldSaveBonfire.bonfire = FindObjectOfType<CharacterBonfireSave>();
            worldSaveBonfire.playerUI = FindObjectOfType<UIManager>();
            playerManager = FindObjectOfType<PlayerManager>();

            StartCoroutine(CreationIssueError());

            _worldSaveStartScreen.LoadGame();
        }

        private IEnumerator CreationIssueError()
        {
            _worldSaveGameManager.creationCanvas.gameObject.SetActive(false);

            yield return null;

            _worldSaveGameManager.creationCanvas.gameObject.SetActive(true);
        }

        public void _QuitGameButton()
        {
            Application.Quit();
        }
    }
}