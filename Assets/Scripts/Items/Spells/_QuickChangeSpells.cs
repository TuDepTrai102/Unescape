using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _QuickChangeSpells : _SpellsSelected
    {
        public _SpellsSelected _spellsSelected;

        public override void Awake()
        {
            base.Awake();
        }

        public override void _SelectThisSpell(SpellItem spell)
        {
            if (spell.isFaithSpell)
            {
                player.playerInventoryManager.currentSpell = spell;

                healingSpellSelectImage.enabled = true;
                healingSpellFrameImage.enabled = false;

                projectileSpellSelectImage.enabled = false;
                projectileSpellFrameImage.enabled = true;

                _spellsSelected.healingSpellSelectImage.enabled = true;
                _spellsSelected.healingSpellFrameImage.enabled = false;

                _spellsSelected.projectileSpellSelectImage.enabled = false;
                _spellsSelected.projectileSpellFrameImage.enabled = true;

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

                _spellsSelected.projectileSpellSelectImage.enabled = true;
                _spellsSelected.projectileSpellFrameImage.enabled = false;

                _spellsSelected.healingSpellSelectImage.enabled = false;
                _spellsSelected.healingSpellFrameImage.enabled = true;

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
    }
}