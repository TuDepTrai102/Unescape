using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class TalentTree : MonoBehaviour
    {
        [SerializeField] private Talent[] talents;
        [SerializeField] private Talent[] unlockedByDefault;

        [SerializeField] public Text talentPointText;
        public int points = 10;

        public GameObject _description;
        public GameObject _noteDescription;
        public Text _descriptionText;
        public Text _noteDescriptionText;

        public Text _skillNameText;
        public Image _skillIconImage;

        private void Start()
        {
            gameObject.SetActive(false);

            ResetTalents();
        }

        public int MyPoints
        {
            get
            {
                return points;
            }

            set
            {
                points = value;
                UpdateTalentPointText();
            }
        }

        public void TryUseTalent(Talent talent)
        {
            if (MyPoints > 0 && talent.Click())
            {
                MyPoints--;
            }

            if (MyPoints == 0)
            {
                foreach (Talent t in talents)
                {
                    if (t.MyCurrentCount == 0)
                    {
                        t.Lock();
                    }
                }
            }
        }

        private void ResetTalents()
        {
            UpdateTalentPointText();

            foreach (Talent talent in talents)
            {
                talent.Lock();
            }

            foreach (Talent talent in unlockedByDefault)
            {
                talent.Unlock();
            }
        }

        public void UpdateTalentPointText()
        {
            talentPointText.text = points.ToString();
        }

        public void _UpdateTalentDescription(Talent talent)
        {
            _descriptionText.gameObject.SetActive(false);
            _noteDescriptionText.gameObject.SetActive(false);

            if (talent != null)
            {
                if (talent._skillName != null)
                {
                    _skillNameText.gameObject.SetActive(true);
                    _skillNameText.text = talent._skillName;
                }
                else
                {
                    _skillNameText.gameObject.SetActive(false);
                    _skillNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (talent._skillIcon != null)
                {
                    _skillIconImage.gameObject.SetActive(true);
                    _skillIconImage.enabled = true;
                    _skillIconImage.sprite = talent._skillIcon;
                }
                else
                {
                    _skillIconImage.gameObject.SetActive(false);
                    _skillIconImage.enabled = false;
                    _skillIconImage.sprite = null;
                }

                _descriptionText.text = talent._descriptionOfSkill.ToString();
                _noteDescriptionText.text = talent._noteDescriptionOfSkill.ToString();

                _descriptionText.gameObject.SetActive(true);
                _noteDescriptionText.gameObject.SetActive(true);
            }
            else
            {
                _skillNameText.text = "";
                _skillIconImage.gameObject.SetActive(false);
                _skillIconImage.enabled = false;
                _skillIconImage.sprite = null;
                _descriptionText.gameObject.SetActive(false);
                _noteDescriptionText.gameObject.SetActive(false);
            }
        }

        public void _UpdateSpellDescription(SpellItem spell)
        {
            _descriptionText.gameObject.SetActive(false);
            _noteDescriptionText.gameObject.SetActive(false);

            if (spell != null)
            {
                if (spell.itemName != null)
                {
                    _skillNameText.gameObject.SetActive(true);
                    _skillNameText.text = spell.itemName;
                }
                else
                {
                    _skillNameText.gameObject.SetActive(false);
                    _skillNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (spell.itemIcon != null)
                {
                    _skillIconImage.gameObject.SetActive(true);
                    _skillIconImage.enabled = true;
                    _skillIconImage.sprite = spell.itemIcon;
                }
                else
                {
                    _skillIconImage.gameObject.SetActive(false);
                    _skillIconImage.enabled = false;
                    _skillIconImage.sprite = null;
                }

                _descriptionText.text = spell.itemDescription.ToString();
                _noteDescriptionText.text = spell.spellDescription.ToString();

                _descriptionText.gameObject.SetActive(true);
                _noteDescriptionText.gameObject.SetActive(true);
            }
            else
            {
                _skillNameText.text = "";
                _skillIconImage.gameObject.SetActive(false);
                _skillIconImage.enabled = false;
                _skillIconImage.sprite = null;
                _descriptionText.gameObject.SetActive(false);
                _noteDescriptionText.gameObject.SetActive(false);
            }
        }
    }
}