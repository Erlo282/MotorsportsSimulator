using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result_Handler : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] private Car_Controller car_controller;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        GameManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    public void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title Screen");
    }

    public IEnumerator ShowFinishMenu_co()
    {
        yield return new WaitForSeconds(1.0f);
        canvas.enabled = true;
    }

    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameState() == GameStates.raceOver && !car_controller.isCarRetire)
        {
            StartCoroutine(ShowFinishMenu_co());
        }
    }
    private void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
