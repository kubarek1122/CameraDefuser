using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Movement : MonoBehaviour
{
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private float _respawnTime;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private float _jumpHeight = 5;
    [SerializeField] private Rigidbody _rb;

    public float Speed;

    private bool isDead = false;
    private bool respawning = false;
    private bool isGrounded = true;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;


    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }


    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (!isDead)
        {
            float hor = Input.GetAxis("Vertical");
            float ver = Input.GetAxis("Horizontal");
            Vector3 playerMovement = new Vector3(hor, 0.0f, -1*ver) * Speed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpHeight);
                isGrounded = false;
            }

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

        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
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
        _cameraMove.safeZone = false;
        respawning = false;
    }



}
