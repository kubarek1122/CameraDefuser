using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private float respawnTime = 2f;
    [SerializeField] private float jumpHeight = 5f;
    
    private Transform spawnPoint;

    Transform cam;

    public float baseMovementSpeed = 6f;
    public float sprintSpeed = 10f;
    public float turnSmooth = 0.1f;
    float turnSmoothVelocity;

    public bool isDead = false;
    private bool isGrounded;
    private float movementSpeed;

    Rigidbody rb;
    Vector3 direction;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;

        rb = GetComponent<Rigidbody>();

        movementSpeed = baseMovementSpeed;

        spawnPoint = transform;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        processInputs();
        
    }

    private void FixedUpdate()
    {
        move(direction);
    }

    void move(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rb.MovePosition(transform.position + moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }

    void processInputs()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = baseMovementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void Kill()
    {
        isDead = true;

        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        rb.velocity = Vector3.zero;

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
