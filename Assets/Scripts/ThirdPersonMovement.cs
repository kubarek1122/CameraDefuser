using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private float _respawnTime = 2f;
    [SerializeField] private float _jumpHeight = 5f;
    
    private Transform spawnPoint;

    Transform cam;

    public float movementSpeed = 6f;
    public float turnSmooth = 0.1f;
    public float gravity = 9.81f;
    float turnSmoothVelocity;

    public bool isDead = false;
    private bool isGrounded;

    private Rigidbody _rb;

    Vector3 jump = Vector3.zero;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        _rb = GetComponent<Rigidbody>();

        spawnPoint = transform;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            transform.position += moveDir.normalized * movementSpeed * Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void Kill()
    {
        isDead = true;

        Invoke("Respawn", _respawnTime);
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        isDead = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Projectile")
        {
            if (!isDead)
            {
                _explosion.Play();
                Kill();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            spawnPoint = other.transform.GetChild(0).transform;
            other.GetComponent<Checkpoint>().changeMaterial();
        }
    }
}
