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
    private int maxColorsToUser = 4;


    private int amountOfColorsToUse = 0;
    private static List<Color> colorsToUse;

    void Start()
    {
        colorsToUse = new List<Color>();
        amountOfColorsToUse = Random.Range(minColorsToUse, maxColorsToUser);

        int colorsFound = 0;

        while(colorsFound < amountOfColorsToUse)
        {
            Color color = colors[Random.Range(0, colors.Length - 1)];

            if(!colorsToUse.Contains(color))
            {
                colorsToUse.Add(color);
                colorsFound++;
            }
        }
    }

    public Color GetColor()
    {
        return colorsToUse[Random.Range(0, colorsToUse.Count - 1)];
    }
}