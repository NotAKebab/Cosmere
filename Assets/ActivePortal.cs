using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePortal : MonoBehaviour
{
    [SerializeField]public GameObject luthadelActive;
    void Start()
    {
        luthadelActive.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("forPortal"))
        {
            luthadelActive.SetActive(true);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        luthadelActive.SetActive(false);
    }
}
