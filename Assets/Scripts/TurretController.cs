using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _rayPosition;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private int _fireRate;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _bulletLifetime;

    [SerializeField] private int _rotationSpeed;

    private bool _targetInRange = false;
    private bool _visible;
    private bool _shot = false;
    private bool _rotation = true;
    private Quaternion _initialRotation;

    void Start()
    {
        _initialRotation = transform.rotation;
    }

    void Update()
    {
        var ray = new Ray(_rayPosition.position, _rayPosition.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        if (hit.transform.gameObject == _target.gameObject)
        {
            _visible = true;
            _rotation = false;
            Debug.Log("Player Visible");
        }
        else
        {
            _visible = false;
            _rotation = true;
            Debug.Log("Player Not Visible");
        }

        if (_targetInRange && _visible)
        {
            transform.LookAt(_target);
            _rayPosition.rotation = transform.rotation;
        } 

        if(_rotation)
        {
            _rayPosition.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, 0.3f);
        }

        if (!_shot && _visible && _targetInRange)
        {
            _shot = true;
            StartCoroutine("ShotDelay");
        }
    }

    IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(_fireRate);

        if (_visible)
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
}
