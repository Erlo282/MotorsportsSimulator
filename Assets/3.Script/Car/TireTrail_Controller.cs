using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrail_Controller : MonoBehaviour
{
    // Components
    [SerializeField] private Car_Controller car_controller;
    [SerializeField] private TrailRenderer trailRenderer;

    private void Awake()
    {
        car_controller = GetComponentInParent<Car_Controller>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (car_controller.isTireSlipping(out float lateralVelo, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
