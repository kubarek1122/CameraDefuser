using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletOrb : MonoBehaviour
{
    public bool off = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "MainCamera")
        {
            gameObject.SetActive(false);
            Invoke("turnOn", 1.5f);
        }
    }

    void turnOn()
    {
        if (!off)
            gameObject.SetActive(true);
    }
}
