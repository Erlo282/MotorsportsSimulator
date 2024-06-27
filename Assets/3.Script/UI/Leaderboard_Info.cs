using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard_Info : MonoBehaviour
{
    public Text positionText;
    public Text driverNameText;

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }
    public void SetDriverNameText(string newDriverName)
    {
        driverNameText.text = newDriverName;
    }
}
