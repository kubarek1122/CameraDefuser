using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TurretController : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _rayPosition;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Material _deactivated;
    [SerializeField] private float _fireDelay;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _bulletLifetime;

    [SerializeField] private int _rotationSpeed;

    [SerializeField] private VisualEffect _explosion;

    private ThirdPersonMovement player;

    private bool _targetInRange = false;
    private bool _visible;
    private bool _shot = false;
    private bool _alive = true;
    private Quaternion _initialRotation;

    void Start()
    {
        _initialRotation = transform.rotation;
        _target = GameObject.Find("Player").transform;
        player = _target.gameObject.GetComponent<ThirdPersonMovement>();
    }

    void Update()
    {
        if (_alive)
        {
            if (_targetInRange)
            {
                RaycastHit hit;
                var ray = new Ray(_rayPosition.position, _target.position - _rayPosition.position);
                Physics.Raycast(ray, out hit);
                if (hit.transform.gameObject == _target.gameObject)
                {
                    _visible = true;
                    Debug.Log("Player Visible");
                }
                else
                {
                    _visible = false;
                    Debug.Log("Player Not Visible");
                }
            }

            if (_targetInRange && _visible)
            {
                //transform.LookAt(_target);
                Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f);
            }

            if (!_visible || !_targetInRange)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, 0.3f);
            }

            if (!_shot && _visible && _targetInRange)
            {
                _shot = true;
                StartCoroutine("ShotDelay");
            }
        }

        if (Vector3.Dot(transform.up, Vector3.up) < 0.5f)
        {
            _alive = false;
        }


    }

    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(_fireDelay);

        if (_visible && !player.isDead)
        {
            Rigidbody clone;
            clone = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
            clone.velocity = _firePoint.TransformDirection(Vector3.forward * _bulletSpeed);
            clone.gameObject.GetComponent<BulletDestroy>().destroyAfterSeconds(_bulletLifetime);
        }
        _shot = false;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _target = other.transform;
            _targetInRange = true;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _targetInRange = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = _deactivated;
            _explosion.Play();
            _alive = false;
        }
    }
}
