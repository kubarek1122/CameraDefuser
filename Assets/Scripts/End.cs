using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [SerializeField] private GameObject hide;
    [SerializeField] private GameObject show;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<ThirdPersonMovement>().isDead = true;
            hide.SetActive(false);
            show.SetActive(true);
            Invoke("Menu", 5f);
        }
    }

    void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
