using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Car_LapCounter : MonoBehaviour
{
    private int checkpoint_Num = 0;
    private int checkpointCount = 0;
    private int lapCount = 0;
    public bool isRaceFinish = false;
    private int carPosition = 0;
    float timeAtLastCheckpoint = 0;
    [SerializeField] private int lapsToFinish = 0;

    // Component
    [SerializeField] private LapCounter lapCounter;

    // Event
    public event Action<Car_LapCounter> OnPassCheckPoint;

    private void Start()
    {
        if (CompareTag("Player"))
        {
            lapCounter = FindObjectOfType<LapCounter>();
            lapCounter.SetLapText($"Lap {lapCount + 1} / {lapsToFinish}");
        }
    }

    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int Num_CP_Passed()
    {
        return checkpointCount;
    }

    public float TimeAtLastCP()
    {
        return timeAtLastCheckpoint;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if(isRaceFinish)
            {
                return;
            }

            Checkpoint checkpoint = collision.GetComponent<Checkpoint>();

            // Car must pass checkpoints in order
            if (checkpoint_Num + 1 == checkpoint.checkpointNumber)
            {
                checkpoint_Num = checkpoint.checkpointNumber;
                checkpointCount++;
                //Debug.Log($"Checkpoint : {checkpoint_Num}");
                // Store time at last CP
                timeAtLastCheckpoint = Time.time;

                // Passing finish line => Reset CP number & add lap count
                if (checkpoint.isFinishLine)
                {
                    checkpoint_Num = 0;
                    lapCount++;
                    Debug.Log($"Lap : {lapCount}");
                    if (lapCount >= lapsToFinish)
                    {
                        isRaceFinish = true;
                    }
                    if (!isRaceFinish && lapCounter != null)
                    {
                        lapCounter.SetLapText($"Lap {lapCount + 1} / {lapsToFinish}");
                    }
                }

                // Invoke passed CP event
                OnPassCheckPoint?.Invoke(this);

                if (isRaceFinish)
                {
                    if (CompareTag("Player"))
                    {
                        GameManager.instance.OnRaceOver();
                    }
                    else
                    {
                        // Lose
                    }
                    //SceneManager.LoadScene("Result");
                }
            }
        }
    }
}
