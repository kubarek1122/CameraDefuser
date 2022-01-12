using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    public float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    [SerializeField] float _deathTimer = 1.5f;

    public Transform checkpoint;
    bool visible = true;
    public GameObject player;
    public bool safeZone = false;

    public Collider camera_col;

    public bool fadeing = false;

    Ray ray;
    RaycastHit hit;

    public CanvasGroup FadeImage;

    void Start()
    {
        //FadeOut();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(-_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;

        ray = new Ray(transform.position + new Vector3(0, 0, 0.1f), transform.forward);
        Physics.Raycast(ray, out hit);

        /*  if (hit.transform.gameObject == _target.gameObject)
          {
              FadeImage.alpha = 0;
              if(!fadeing)
              {
                  fadeing = true;

                  FadeOut();
              }

              Debug.Log("Visible");
              visible = true;
          }
          else
          {
              if(!fadeing)
              {
                  fadeing = true;
                  FadeIn();
              }
              Debug.Log("Not visible");
              Debug.Log(fadeing);
              visible = false;
          }

          if(visible==false && safeZone==false)
          {


              StartCoroutine("ExampleCoroutine");
              //player.transform.position = checkpoint.position;
          } else
          {
              StopCoroutine("ExampleCoroutine");
          }
        */
    }

    public IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.

        yield return new WaitForSeconds(_deathTimer);
        if (visible == false)
        {
            player.transform.position = checkpoint.position;
        }

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SafeZone")
        {
            safeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SafeZone")
        {
            safeZone = false;
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(FadeImage, FadeImage.alpha, 1, 1.5f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(FadeImage, FadeImage.alpha, 0, 0.2f));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while(true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            if (!safeZone)
                cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        fadeing = false;

    }


}