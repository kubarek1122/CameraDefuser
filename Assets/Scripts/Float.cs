using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Float : MonoBehaviour
{
    private float height, offset;
    private Vector3 pos;

    private void Start()
    {
        height = Random.Range(-10, 10);
        offset = Random.Range(-5, 5);
        pos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(pos, pos + new Vector3(0, height, 0), (Mathf.Sin((Time.time + offset) * 0.5f) + 1) * 0.5f);
    }
}
