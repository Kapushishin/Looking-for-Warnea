using System.Collections.Generic;
using UnityEngine;

public class DefaultInitIcon : IInitIcon
{
    private Dictionary<int, Color> layerColor = new Dictionary<int, Color>()
    {
        {8, Color.red },
        {9, Color.green },
    };

    public void IconInit(int unitLayer, GameObject icon)
    {
        foreach (var i in layerColor)
        {
            if (i.Key == unitLayer)
            {
                icon.GetComponent<Renderer>().material.SetColor("_BaseColor", layerColor[i.Key]);
            }
        }
    }
}
