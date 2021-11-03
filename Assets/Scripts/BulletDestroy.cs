using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private int timeout;

    public void destroyAfterSeconds(int sec)
    {
        timeout = sec;
        StartCoroutine("wait");
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeout);
        Destroy(gameObject);
    }
}
