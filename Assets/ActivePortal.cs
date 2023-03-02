using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivePortal : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        while (collision.gameObject.name == "entradaLuthadel")
        {
            gameObject.SetActive(true);
        }
     }
}
