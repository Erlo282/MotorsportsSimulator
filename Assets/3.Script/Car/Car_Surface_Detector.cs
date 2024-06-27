using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Surface_Detector : MonoBehaviour
{
    public LayerMask surfaceLayer;

    // Surface contact check
    Collider2D[] surfaceCollider = new Collider2D[10];
    Vector3 lastSurfacePosition = Vector3.one * 10000;

    // Surface Type
    Surface_Controller.SurfaceTypes drivingSurface = Surface_Controller.SurfaceTypes.Asphalt;

    Collider2D carCollider;

    private void Awake()
    {
        carCollider = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        if ((transform.position - lastSurfacePosition).sqrMagnitude < 0.75f)
        {
            return;
        }

        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.layerMask = surfaceLayer;
        contactFilter2D.useLayerMask = true;
        contactFilter2D.useTriggers = true;

        int surface_HitCount = Physics2D.OverlapCollider(carCollider, contactFilter2D, surfaceCollider);

        float lastSurfaceValue = -1000;

        for (int i = 0; i < surface_HitCount; i++)
        {
            Surface_Controller surface_controller = surfaceCollider[i].GetComponent<Surface_Controller>();
            
            if (surface_controller.transform.position.z > lastSurfaceValue)
            {
                drivingSurface = surface_controller.surfaceType;
                lastSurfaceValue = surface_controller.transform.position.z;
            }
        }
        if (surface_HitCount == 0)
        {
            drivingSurface = Surface_Controller.SurfaceTypes.Asphalt;
        }
        lastSurfacePosition = transform.position;
        //Debug.Log($"Driving on {drivingSurface}");
    }
    public Surface_Controller.SurfaceTypes GetCurrentSurface()
    {
        return drivingSurface;
    }
}