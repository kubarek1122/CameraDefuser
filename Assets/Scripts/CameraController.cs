using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement player;
    [SerializeField] private Animation anim;

    [SerializeField] float topDetection = 1.0f,
                            bottomDetection = 1.0f,
                            backDetection = 1.0f,
                            diagonalDetection = 1.0f;

    [SerializeField] private CanvasGroup top, bottom, back;

    [SerializeField] LayerMask wallsLayer;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        top.alpha = 0;
        bottom.alpha = 0;
        back.alpha = 0;
    }

    void Update()
    {
        bool top_check = Physics.Raycast(transform.position, transform.up, topDetection, wallsLayer);
        bool bottom_check = Physics.Raycast(transform.position, -transform.up, bottomDetection, wallsLayer);
        bool back_check = Physics.Raycast(transform.position, -transform.forward, backDetection, wallsLayer);
        bool topback_check = Physics.Raycast(transform.position, (transform.up - transform.forward).normalized, diagonalDetection, wallsLayer);
        bool bottomback_check = Physics.Raycast(transform.position, (-transform.up - transform.forward).normalized, diagonalDetection, wallsLayer);

        top.alpha = 0;
        bottom.alpha = 0;
        back.alpha = 0;

        if (top_check)
        {
            top.alpha = 1;
        }

        if (bottom_check)
        {
            bottom.alpha = 1;
        }

        if(back_check)
        {
            back.alpha = 1;
        }

        if (topback_check)
        {
            top.alpha = 1;
            back.alpha = 1;
        }

        if (bottomback_check)
        {
            bottom.alpha = 1;
            back.alpha = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!player.isDead)
            {
                anim.Play();
                player.Kill();
            }
        }
    }
}
