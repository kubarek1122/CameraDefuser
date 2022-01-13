using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletOrbDoor : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private Material on;
    public List<GameObject> orbs = new List<GameObject>();
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float count = 0;
        foreach (GameObject item in orbs)
        {
            if (!item.activeSelf)
            {
                count += 1;
            }
        }
        if (count == orbs.Count)
        {
            disableWall();
        }
    }

    void disableWall()
    {
        wall.SetActive(false);
        renderer.material = on;
        foreach (GameObject item in orbs)
        {
            item.GetComponent<VioletOrb>().off = true;
        }
    }
}
