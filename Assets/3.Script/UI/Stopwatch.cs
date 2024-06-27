using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    Text timeText;
    float lastRaceTimeUpdate = 0;

    private void Awake()
    {
        timeText = GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(UpdateTime_co());
    }

    public IEnumerator UpdateTime_co()
    {
        while (true)
        {
            float raceTime = GameManager.instance.GetRaceTime();
            if (lastRaceTimeUpdate != raceTime)
            {
                int raceTimeMinutes = (int)Mathf.Floor(raceTime / 60);
                int raceTimeSeconds = (int)Mathf.Floor(raceTime % 60);

                timeText.text = $"{raceTimeMinutes.ToString("00")}:{raceTimeSeconds.ToString("00")}";

                lastRaceTimeUpdate = raceTime;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
