using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{
    public void Restart()
    {
        var temp = FindObjectsOfType<RestartReplacer>();

        foreach (var item in temp)
        {
            item.Restart();
        }

        var tempBlackHoleGenerator = FindObjectOfType<BlackHoleGenerator>();

        if (tempBlackHoleGenerator != null)
        {
            tempBlackHoleGenerator.ReturnToPlayer();
        }
    }
}
