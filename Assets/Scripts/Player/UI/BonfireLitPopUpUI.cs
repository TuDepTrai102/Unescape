using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class BonfireLitPopUpUI : MonoBehaviour
    {
        CanvasGroup canvas;

        private void Awake()
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void DisplayBonfireLitPopUp()
        {
            StartCoroutine(FadeInPopUp());
        }

        IEnumerator FadeInPopUp()
        {
            gameObject.SetActive(true);

            for (float fade = 0.05f; fade < 1; fade = fade + 0.05f)
            {
                canvas.alpha = fade;

                if (fade > 0.9f)
                {
                    StartCoroutine(FadeOutPopUp());
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator FadeOutPopUp()
        {
            //WAIT 2 SECOND BEFORE WE BEGIN TO FADE OUT THE POP UP
            yield return new WaitForSeconds(2);

            for (float fade = 1f; fade > 0; fade = fade - 0.05f)
            {
                canvas.alpha = fade;

                if (fade <= 0.05f)
                {
                    gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}