using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public Text UIText;
    public int curScore = 0;

    public void UpdateText()
    {
        curScore = curScore + 100;
        UIText.text = "Score: " + curScore.ToString();
    }
}