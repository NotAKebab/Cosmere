using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForPortal1 : MonoBehaviour
{
    
    public string targetTag;
    public int newLayer;
    public GameObject[] insidePublicGameObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Vector3 targetVelocity = other.GetComponent<VelocityEstimator>().GetVelocityEstimate();
            
            float angle = Vector3.Angle(transform.forward, targetVelocity);

            if (angle < 90)
            {
                foreach (var item in insidePublicGameObject)
                {
                    item.layer = newLayer;
                }
            }
        }
    }
}
