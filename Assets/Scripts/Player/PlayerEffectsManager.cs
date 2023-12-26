using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager player;

        PoisonBuildUpBar poisonBuildUpBar;
        public PoisonAmountBar poisonAmountBar;

        _BleedBuildUpBar _bleedBuildUpBar;
        public _BleedAmountBar _bleedAmountBar;

        _CurseBuildUpBar _curseBuildUpBar;
        public _CurseAmountBar _curseAmountBar;

        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel; //THE PARTICLES THAT WILL PLAY OF THE CURRENT EFFECT THAT IS EFFECTING THE PLAYER (DRINKING ESTUS, POISON ETC)
        public int amountToBeHealed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();

            _bleedBuildUpBar = FindObjectOfType<_BleedBuildUpBar>();
            _bleedAmountBar = FindObjectOfType<_BleedAmountBar>();

            _curseBuildUpBar = FindObjectOfType<_CurseBuildUpBar>();
            _curseAmountBar = FindObjectOfType<_CurseAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            player.playerStatsManager.HealCharacter(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
            Destroy(healParticles, 1.5f);
            Destroy(instantiatedFXModel.gameObject);
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();
        }

        protected override void ProcessBuildUpDecay()
        {
            if (player.characterStatsManager.poisonBuildup >= 0)
            {
                player.characterStatsManager.poisonBuildup -= 1;

                poisonBuildUpBar.gameObject.SetActive(true);
                poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager.poisonBuildup));
            }

            if (player.characterStatsManager._bleedBuildup >= 0)
            {
                player.characterStatsManager._bleedBuildup -= 1;

                _bleedBuildUpBar.gameObject.SetActive(true);
                _bleedBuildUpBar._SetBleedBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager._bleedBuildup));
            }

            if (player.characterStatsManager._curseBuildup >= 0)
            {
                player.characterStatsManager._curseBuildup -= 1;

                _curseBuildUpBar.gameObject.SetActive(true);
                _curseBuildUpBar._SetCurseBuildUpAmount(Mathf.RoundToInt(player.characterStatsManager._curseBuildup));
            }
        }
    }
}