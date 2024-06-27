using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard_Controller : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    [SerializeField] private Leaderboard_Info[] leaderboard_Info;

    private void Start()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        Car_LapCounter[] car_LapCounterArray = FindObjectsOfType<Car_LapCounter>();
        leaderboard_Info = new Leaderboard_Info[car_LapCounterArray.Length];

        for (int i = 0; i < car_LapCounterArray.Length; i++)
        {
            GameObject leaderboardInfo = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            leaderboard_Info[i] = leaderboardInfo.GetComponent<Leaderboard_Info>();
            leaderboard_Info[i].SetPositionText($"P{i + 1} : ");
        }
    }

    public void UpdateList(List<Car_LapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            leaderboard_Info[i].SetDriverNameText(lapCounters[i].gameObject.name);
        }
    }
}