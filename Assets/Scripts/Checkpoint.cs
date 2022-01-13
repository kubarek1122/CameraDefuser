using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material newMaterial;

    public void changeMaterial()
    {
        GetComponent<Renderer>().material = newMaterial;
    }
}
