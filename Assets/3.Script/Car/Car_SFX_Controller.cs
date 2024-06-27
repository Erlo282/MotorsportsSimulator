using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_SFX_Controller : MonoBehaviour
{
    public AudioSource tireScreech;
    public AudioSource engine;
    public AudioSource contact;
    
    private float enginePitch = 0.5f;
    private float tireScreechPitch = 0.5f;

    [SerializeField] private Car_Controller car_controller;


    private void Awake()
    {
        car_controller = GetComponentInParent<Car_Controller>();
    }

    private void Update()
    {
        UpdateEngineSFX();
        UpdateTireScreechSFX();
    }

    public void UpdateEngineSFX()
    {
        float velo = car_controller.GetVelo();
        // Increase when accelerating
        float volume = velo * 0.05f;
        // Minimum setting for when car is idle
        volume = Mathf.Clamp(volume, 0.2f, 1.0f);
        engine.volume = Mathf.Lerp(engine.volume, volume, Time.deltaTime * 10);
        // Variable pitch
        enginePitch = velo * 0.2f;
        enginePitch = Mathf.Clamp(enginePitch, 0.5f, 2f);
        engine.pitch = Mathf.Lerp(engine.pitch, enginePitch, Time.deltaTime * 1.5f);
    }

    public void UpdateTireScreechSFX()
    {
        if (car_controller.isTireSlipping(out float lateralVelo, out bool isBraking))
        {
            if (isBraking)
            {
                tireScreech.volume = Mathf.Lerp(tireScreech.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
                //tireScreech.pitch = Mathf.Lerp(tireScreech.pitch, tireScreechPitch, Time.deltaTime * 0.5f);
            }
            else
            {
                tireScreech.volume = Mathf.Abs(lateralVelo) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelo) * 0.1f;
                //tireScreech.pitch = Mathf.Lerp(tireScreech.pitch, tireScreechPitch, Time.deltaTime * 0.5f);
            }
        }
        else
        {
            tireScreech.volume = Mathf.Lerp(tireScreech.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelo = collision.relativeVelocity.magnitude;

        float volume = relativeVelo * 0.1f;

        contact.volume = volume;
        contact.pitch = Random.Range(0.95f, 1.05f);

        if (!contact.isPlaying)
        {
            contact.Play();
        }
    }
}
