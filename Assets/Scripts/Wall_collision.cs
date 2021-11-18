using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_collision : MonoBehaviour
{
    public GameObject player;

    public Transform checkpoint;

    public CanvasGroup FadeImage;


    public Animation anim;

    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            anim.Play();
            player.transform.position = checkpoint.position;
        }
    }

   

}
