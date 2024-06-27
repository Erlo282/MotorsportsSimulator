using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Car_Controller : MonoBehaviour
{
    // Variables
    [SerializeField] private float acceleration;
    [SerializeField] private float braking;
    [SerializeField] private float steering = 3f;
    [SerializeField] private float drift;
    [SerializeField] private float maxSpeed;
    public bool isCarRetire = false;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotation = 0;
    private float velocityVSup = 0;
    private int contactCount = 0;

    // Components
    [SerializeField] private Rigidbody2D carRigidBody2D;
    [SerializeField] private Collider2D carCollider;
    [SerializeField] private Car_Surface_Detector car_surface_detector;

    private void Awake()
    {
        carRigidBody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponent<Collider2D>();
        car_surface_detector = GetComponent<Car_Surface_Detector>();
    }
    public void SetInput(Vector2 input)
    {
        accelerationInput = input.y;
        if (velocityVSup < 0)
        {
            steeringInput = -input.x;
        }
        else
        {
            steeringInput = input.x;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameStates.countDown)
        {
            return;
        }

        Engine();
        Steer();
        DriftControl();
    }

    public void Engine()
    {
        // Max Velocity Limit
        velocityVSup = Vector2.Dot(transform.up, carRigidBody2D.velocity);
        if (velocityVSup > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        if (velocityVSup < -maxSpeed && accelerationInput < 0)
        {
            return;
        }
        if (carRigidBody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        // Slow down if no input
        if (accelerationInput == 0)
        {
            carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag, 1.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            carRigidBody2D.drag = 0;
        }

        // Braking & Acceleration
        Vector2 engineVector;
        if (accelerationInput < 0 && velocityVSup > 0)
        {
            engineVector = transform.up * accelerationInput * braking;
        }
        else if (accelerationInput > 0 && velocityVSup < 0)
        {
            engineVector = transform.up * accelerationInput * braking;
        }
        else
        {
            engineVector = transform.up * accelerationInput * acceleration;
        }

        carRigidBody2D.AddForce(engineVector, ForceMode2D.Force);

        // Variable physics based on surface
        switch (GetSurface())
        {
            case Surface_Controller.SurfaceTypes.Asphalt:
                break;
            case Surface_Controller.SurfaceTypes.Dirt:
                carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
            case Surface_Controller.SurfaceTypes.Sand:
                carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag, 9.0f, Time.fixedDeltaTime * 3);
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
            case Surface_Controller.SurfaceTypes.Grass:
                carRigidBody2D.drag = 0;
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
            case Surface_Controller.SurfaceTypes.Snow:
                carRigidBody2D.drag = 0.5f;
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
            case Surface_Controller.SurfaceTypes.Urban:
                carRigidBody2D.drag = Mathf.Lerp(carRigidBody2D.drag, 2.0f, Time.fixedDeltaTime * 3);
                break;
        }
    }
    public void Steer()
    {
        float minSpeed = (carRigidBody2D.velocity.magnitude / 8);
        minSpeed = Mathf.Clamp01(minSpeed);
        
        rotation -= steeringInput * steering * minSpeed;

        carRigidBody2D.MoveRotation(rotation);
    }

    public Surface_Controller.SurfaceTypes GetSurface()
    {
        return car_surface_detector.GetCurrentSurface();
    }

    public void DriftControl()
    {
        Vector2 forwardVelo = transform.up * Vector2.Dot(carRigidBody2D.velocity, transform.up);
        Vector2 rightVelo = transform.right * Vector2.Dot(carRigidBody2D.velocity, transform.right);

        float currentDrift = drift;
        // Variable physics based on surface
        switch (GetSurface())
        {
            case Surface_Controller.SurfaceTypes.Asphalt:
                break;
            case Surface_Controller.SurfaceTypes.Dirt:
                currentDrift *= 1.37f;
                break;
            case Surface_Controller.SurfaceTypes.Sand:
                currentDrift *= 1.35f;
                break;
            case Surface_Controller.SurfaceTypes.Grass:
                currentDrift *= 1.4f;
                break;
            case Surface_Controller.SurfaceTypes.Urban:
                break;
            case Surface_Controller.SurfaceTypes.Snow:
                currentDrift *= 1.35f;
                break;
        }

        carRigidBody2D.velocity = forwardVelo + rightVelo * currentDrift;
    }

    public float GetVelo()
    {
        return Mathf.Abs(Vector2.Dot(transform.up, carRigidBody2D.velocity));
    }

    public float GetLateralVelo()
    {
        return Vector2.Dot(transform.right, carRigidBody2D.velocity);
    }

    public bool isTireSlipping(out float lateralVelo, out bool isBraking)
    {
        lateralVelo = GetLateralVelo();
        isBraking = false;

        if (accelerationInput < 0 && velocityVSup > 0)
        {
            isBraking = true;
            return true;
        }
        if (Mathf.Abs(GetLateralVelo()) > 4.0f)
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelo = collision.relativeVelocity.magnitude;

        if (relativeVelo > maxSpeed * 0.2f && relativeVelo < maxSpeed * 0.4f)
        {
            contactCount += 1;
            Debug.Log($"Contact : {contactCount}");
        }
        if (relativeVelo >= maxSpeed * 0.4f && relativeVelo < maxSpeed * 0.6f)
        {
            contactCount += 3;
            Debug.Log($"Contact : {contactCount}");
        }
        if (relativeVelo >= maxSpeed * 0.8f)
        {
            contactCount += 5;
            Debug.Log($"Contact : {contactCount}");
        }

        if (GameManager.instance.GetGameState() != GameStates.raceOver)
        {
            if (contactCount > 19)
            {
                isCarRetire = true;
                Debug.Log("Car retired!");
                GameManager.instance.ChangeGameState(GameStates.raceOver);
                gameObject.SetActive(false);
                // Game Over Event
                SceneManager.LoadScene("Retired");
            }
        }
    }
}
