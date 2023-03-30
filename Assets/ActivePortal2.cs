using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePortal2 : MonoBehaviour
{
    [SerializeField]public GameObject nalthisActive;
    void Start()
    {
        nalthisActive.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("forPortal"))
        {
            nalthisActive.SetActive(true);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        nalthisActive.SetActive(false);
    }
}
