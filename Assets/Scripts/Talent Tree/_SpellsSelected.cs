using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _SpellsSelected : MonoBehaviour
    {
        public PlayerManager player;
        public _QuickChangeSpells _quickChangeSpell;

        public Image healingSpellFrameImage;
        public Image healingSpellSelectImage;

        public Image projectileSpellFrameImage;
        public Image projectileSpellSelectImage;

        public virtual void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public virtual void _SelectThisSpell(SpellItem spell)
        {
            if (spell.isFaithSpell)
            {
                player.playerInventoryManager.currentSpell = spell;

                healingSpellSelectImage.enabled = true;
                healingSpellFrameImage.enabled = false;

                projectileSpellSelectImage.enabled = false;
                projectileSpellFrameImage.enabled = true;

                _quickChangeSpell.healingSpellSelectImage.enabled = true;
                _quickChangeSpell.healingSpellFrameImage.enabled = false;

                _quickChangeSpell.projectileSpellSelectImage.enabled = false;
                _quickChangeSpell.projectileSpellFrameImage.enabled = true;

                player.uiManager.quickSlotsUI.UpdateCurrentSpellIcon(spell);
                player.uiManager.equipmentWindowUI._LoadSpellOnEquipmentScreen(player.playerInventoryManager);
            }
            else if (spell.isPyroSpell)
            {
                player.playerInventoryManager.currentSpell = spell;

                projectileSpellSelectImage.enabled = true;
                projectileSpellFrameImage.enabled = false;

                healingSpellSelectImage.enabled = false;
                healingSpellFrameImage.enabled = true;

                _quickChangeSpell.projectileSpellSelectImage.enabled = true;
                _quickChangeSpell.projectileSpellFrameImage.enabled = false;

                _quickChangeSpell.healingSpellSelectImage.enabled = false;
                _quickChangeSpell.healingSpellFrameImage.enabled = true;

                player.uiManager.quickSlotsUI.UpdateCurrentSpellIcon(spell);
                player.uiManager.equipmentWindowUI._LoadSpellOnEquipmentScreen(player.playerInventoryManager);
            }
            else
            {
                player.playerInventoryManager.currentSpell = null;

                healingSpellSelectImage.enabled = false;
                healingSpellFrameImage.enabled = true;

                projectileSpellSelectImage.enabled = false;
                projectileSpellFrameImage.enabled = true;

                player.uiManager.quickSlotsUI.UpdateCurrentSpellIcon(spell);
                player.uiManager.equipmentWindowUI._LoadSpellOnEquipmentScreen(player.playerInventoryManager);
            }
        }

        public void _UpdateUISpell(SpellItem spell)
        {
            if (spell.isFaithSpell)
            {
                healingSpellSelectImage.enabled = true;
                healingSpellFrameImage.enabled = false;

                _quickChangeSpell.healingSpellSelectImage.enabled = true;
                _quickChangeSpell.healingSpellFrameImage.enabled = false;
            }
            else if (spell.isPyroSpell)
            {
                projectileSpellSelectImage.enabled = true;
                projectileSpellFrameImage.enabled = false;

                _quickChangeSpell.projectileSpellSelectImage.enabled = true;
                _quickChangeSpell.projectileSpellFrameImage.enabled = false;
            }
        }
    }
}