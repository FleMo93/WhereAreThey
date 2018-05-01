using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanColor : MonoBehaviour, IHumanColor
{
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private int minColorsToUse = 1;
    [SerializeField]
    private int maxColorsToUse = 4;


    private int amountOfColorsToUse = 0;
    private static List<Color> colorsToUse;

    void Start()
    {
        colorsToUse = new List<Color>();

        if(maxColorsToUse > colors.Length)
        {
            maxColorsToUse = colors.Length;
        }

        if(minColorsToUse <= 0)
        {
            minColorsToUse = 1;
        }

        if(minColorsToUse > maxColorsToUse)
        {
            minColorsToUse = maxColorsToUse;
        }

        MixColors();
    }

    public void MixColors()
    {
        colorsToUse.Clear();
        amountOfColorsToUse = UnityEngine.Random.Range(minColorsToUse, maxColorsToUse);
        
        int colorsFound = 0;

        while (colorsFound < amountOfColorsToUse)
        {
            Color color = colors[UnityEngine.Random.Range(0, colors.Length)];
        
            if (!colorsToUse.Contains(color))
            {
                colorsToUse.Add(color);
                colorsFound++;
            }
        }
    }

    public Color GetColor()
    {
        return colorsToUse[UnityEngine.Random.Range(0, colorsToUse.Count - 1)];
    }
}