using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown_Controller : MonoBehaviour
{
    public Text countDownText;
    [SerializeField] private AudioSource countdown_audio;

    private void Awake()
    {
        countDownText.text = "";
        countdown_audio = transform.GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Countdown_co());
    }

    public IEnumerator Countdown_co()
    {
        yield return new WaitForSeconds(0.3f);

        countdown_audio = transform.GetComponent<AudioSource>();
        countdown_audio.Play();

        int counter = 3;
        while (true)
        {
            if (counter != 0)
            {
                countDownText.text = counter.ToString();
            }
            else
            {
                countDownText.text = "GO!";
                GameManager.instance.OnRaceStart();
                break;
            }
            counter--;
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
