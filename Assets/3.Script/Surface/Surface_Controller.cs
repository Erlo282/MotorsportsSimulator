using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Controller : MonoBehaviour
{
    public enum SurfaceTypes
    { 
        Asphalt,
        Dirt,
        Sand,
        Grass,
        Urban,
        Snow
    };
    public SurfaceTypes surfaceType;
}
