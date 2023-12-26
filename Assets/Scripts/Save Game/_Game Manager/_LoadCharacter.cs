using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _LoadCharacter : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            player.enabled = true;
            player.playerAnimatorManager.enabled = true;
            player.playerCombatManager.enabled = true;
            player.playerEffectsManager.enabled = true;
            player.playerLocomotionManager.enabled = true;
            player.playerInventoryManager.enabled = true;
            player.playerWeaponSlotManager.enabled = true;
            player.playerStatsManager.enabled = true;
            player.inputHandler.enabled = true;
            player._playerSkills.enabled = true;

            StartCoroutine(_skillTreeIssueError_());
        }

        private IEnumerator _skillTreeIssueError_()
        {
            player.uiManager._skillTreeWindow.SetActive(true);

            yield return new WaitForSeconds(0.55f);

            player.uiManager._skillTreeWindow.SetActive(false);
        }
    }
}