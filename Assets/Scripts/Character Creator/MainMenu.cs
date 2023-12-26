using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NT
{
    public class MainMenu : MonoBehaviour
    {
        //public void OnNewGameCliked()
        //{
        //    //CREATE A NEW GAME - WHICH WILL INITIALIZE OUR GAME DATA
        //    WorldSaveGameManager.instance.NewGame();
        //    //LOAD THE GAMEPLAY SCENE - WHICH WILL IN TURN SAVE THE GAME BECAUSE OF
        //    //ONSCENEUNLOADED() IN THE WORLD SAVE GAME MANAGER SCRIPT
        //    SceneManager.LoadSceneAsync("Demo02");
        //}

        public void OnContinueGameClicked()
        {
            //LOAD THE NEXT SCENE - WHICH WILL IN TURN SAVE THE GAME BECAUSE OF
            //ONSCENEUNLOADED() IN THE WORLD SAVE GAME MANAGER SCRIPT
            SceneManager.LoadSceneAsync("Demo02");
        }
    }
}