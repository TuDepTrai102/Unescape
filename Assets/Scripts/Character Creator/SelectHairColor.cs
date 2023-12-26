using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHairColor : MonoBehaviour
{
    [Header("COLOR VALUES")]
    public float redAmount;
    public float greenAmount;
    public float blueAmount;
    public float alphaAmount;

    [Header("COLOR SLIDERS")]
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    private Color currentHairColor;

    //WE GRAB THE MATERIAL FROM THE SKIN MESH RENDERER, AND CHANGE THE COLOR PROPERTIES OF THE MATERIAL
    public List<SkinnedMeshRenderer> rendererList = new List<SkinnedMeshRenderer>();

    public void UpdateSliders()
    {
        redAmount = redSlider.value;
        greenAmount = greenSlider.value;
        blueAmount = blueSlider.value;
        SetHairColor();
    }

    public void SetHairColor()
    {
        currentHairColor = new Color(redAmount, greenAmount, blueAmount, alphaAmount);

        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererList[i].material.SetColor("_Color_Hair", currentHairColor);
        }
    }
}
