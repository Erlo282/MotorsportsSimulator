using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Position_Handler : MonoBehaviour
{
    // Components
    [SerializeField] private Leaderboard_Controller leaderboard_controller;

    // List to keep track of track position
    public List<Car_LapCounter> car_LapCounters = new List<Car_LapCounter>();

    private void Start()
    {
        Car_LapCounter[] car_LapCounter_Array = FindObjectsOfType<Car_LapCounter>();
        car_LapCounters = car_LapCounter_Array.ToList();

        foreach (Car_LapCounter lapCounters in car_LapCounters)
        {
            lapCounters.OnPassCheckPoint += OnPassingCP;
        }

        leaderboard_controller = FindObjectOfType<Leaderboard_Controller>();

        if (leaderboard_controller != null)
        {
              leaderboard_controller.UpdateList(car_LapCounters);
        }
    }

    void OnPassingCP(Car_LapCounter carlapcounter)
    {
        // Sort track position by : 1. (Descending) Number of CPs passed 2. (Increasing) Time since last CP passed
        car_LapCounters = car_LapCounters.OrderByDescending(s => s.Num_CP_Passed()).ThenBy(s => s.TimeAtLastCP()).ToList();

        // Get track position
        int carPosition = car_LapCounters.IndexOf(carlapcounter) + 1;

        carlapcounter.SetCarPosition(carPosition);

        if (carlapcounter.isRaceFinish)
        {
            int raceResult_Pos = carPosition;
        }

        if (leaderboard_controller != null)
        {
            leaderboard_controller.UpdateList(car_LapCounters);
        }
    }
}
