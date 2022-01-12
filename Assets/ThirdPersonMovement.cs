using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private VisualEffect _explosion;
    [SerializeField] private float _respawnTime = 2f;
    [SerializeField] private float _jumpHeight = 5f;
    [SerializeField] private Transform spawnPoint;

    CharacterController controller;
    Transform cam;

    public float movementSpeed = 6f;
    public float turnSmooth = 0.1f;
    public float jumpSmooth = 0.1f;
    public float gravity = 9.81f;
    float turnSmoothVelocity;
    Vector3 jumpSmoothVelocity;

    private bool isDead = false;
    private bool respawning = false;

    private Rigidbody _rb;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameObject.Find("Main Camera").transform;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDead)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
            }
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
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.angularVelocity = new Vector3(0, 0, 0);
        respawning = false;
    }
}
