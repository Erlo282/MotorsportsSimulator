using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    Text lapText;

    private void Awake()
    {
        lapText = GetComponent<Text>();
    }

    public void SetLapText(string text)
    {
        lapText.text = text;
    }
}
