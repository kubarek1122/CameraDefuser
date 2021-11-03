using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Movement : MonoBehaviour
{
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private int _respawnTime;

    public float Speed;

    private bool isDead = false;
    private bool respawning = false;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (!isDead)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            Vector3 playerMovement = new Vector3(hor, 0f, ver) * Speed * Time.deltaTime;
            transform.Translate(playerMovement, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            isDead = true;
            _explosion.Play();
            if (isDead && !respawning)
            {
                respawning = true;
                StartCoroutine("respawnTimer");
            }
        }
    }

    IEnumerator respawnTimer()
    {
        yield return new WaitForSeconds(_respawnTime);
        Respawn();
    }

    void Respawn()
    {
        isDead = false;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.angularVelocity = new Vector3(0, 0, 0);
        respawning = false;
    }



}
