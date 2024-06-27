using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticle_Controller : MonoBehaviour
{
    private float emissionRate = 0;

    // Components
    [SerializeField] private Car_Controller car_controller;
    [SerializeField] private Car_Surface_Detector car_surface_detector;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem.EmissionModule _emissionModule;

    private void Awake()
    {
        car_controller = GetComponentInParent<Car_Controller>();
        car_surface_detector = GetComponentInParent<Car_Surface_Detector>();
        _particleSystem = GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
        _emissionModule.rateOverTime = 0;
    }

    public Surface_Controller.SurfaceTypes GetSurface()
    {
        return car_surface_detector.GetCurrentSurface();
    }

    private void Update()
    {
        emissionRate = Mathf.Lerp(emissionRate, 0, Time.deltaTime * 5);
        _emissionModule.rateOverTime = emissionRate;

        switch (GetSurface())
        {
            case Surface_Controller.SurfaceTypes.Asphalt:
                break;
            case Surface_Controller.SurfaceTypes.Dirt:
                emissionRate = 30;
                break;
            case Surface_Controller.SurfaceTypes.Sand:
                emissionRate = 30;
                break;
            case Surface_Controller.SurfaceTypes.Grass:
                break;
            case Surface_Controller.SurfaceTypes.Urban:
                break;
            case Surface_Controller.SurfaceTypes.Snow:
                emissionRate = 30;
                break;
        }

        if (car_controller.isTireSlipping(out float lateralVelo, out bool isBraking))
        {
            if (isBraking)
            {
                emissionRate = 30;
            }
            else
            {
                emissionRate = Mathf.Abs(lateralVelo) * 2;
            }
        }
    }
}
