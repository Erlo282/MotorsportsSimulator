using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Controller : MonoBehaviour
{
    [SerializeField] private AudioSource BGM;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        BGM = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (BGM.isPlaying)
        {
            return;
        }
        BGM.Play();
    }
    public void StopMusic()
    {
        BGM.Stop();
    }
}
