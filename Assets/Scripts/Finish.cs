using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Finish : MonoBehaviour
{
    [SerializeField] private VisualEffect _fireworks;

    [SerializeField] private List<GameObject> _walls;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _fireworks.Play();
            foreach (var item in _walls)
            {
                item.SetActive(false);
            }
        }
    }
}
