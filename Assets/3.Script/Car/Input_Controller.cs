using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Controller : MonoBehaviour
{
    // Components
    Car_Controller car_controller;

    private void Awake()
    {
        car_controller = GetComponent<Car_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input_V = Vector2.zero;

        input_V.x = Input.GetAxis("Horizontal");
        input_V.y = Input.GetAxis("Vertical");

        car_controller.SetInput(input_V);
    }
}
