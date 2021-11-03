using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Finish : MonoBehaviour
{
    [SerializeField] private VisualEffect _fireworks;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _fireworks.Play();
        }
    }
}
