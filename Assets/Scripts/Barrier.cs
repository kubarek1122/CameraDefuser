using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private BoxCollider _door;
    
    [SerializeField] private Material _material;

    void Start()
    {
        _material.SetVector("Color_d9dfc07cd944497c94d6401352b43c55", new Vector4(3.5f, 3.5f, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _material.SetVector("Color_d9dfc07cd944497c94d6401352b43c55", new Vector4(0f, 3.5f, 0, 0));
            _door.enabled = false;
        }

        if (other.tag == "MainCamera")
        {
            _material.SetVector("Color_d9dfc07cd944497c94d6401352b43c55", new Vector4(3.5f, 0, 0, 0));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _material.SetVector("Color_d9dfc07cd944497c94d6401352b43c55", new Vector4(3.5f, 3.5f, 0, 0));
        _door.enabled = true;
    }
}
