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

    RaycastHit slopeHit;

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

        ProcessInputs();
        OnGround();
        if (OnSlope())
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        Move(direction);
    }

    bool OnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.2f))
        {
            isGrounded = true;
            rb.drag = 1.0f;
        }
        else
        {
            isGrounded = false;
            rb.drag = 0.1f;
        }
        return isGrounded;
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.1f))
        {
            if (slopeHit.normal != Vector3.down)
            {
                return true;
            }
        }
        return false;
    }

    void Move(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (OnSlope())
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
            }

            rb.MovePosition(transform.position + moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }

    void ProcessInputs()
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(slopeHit.point, 0.1f);
    }
}
