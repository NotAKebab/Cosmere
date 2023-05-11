using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWorld : MonoBehaviour
{
   public string World1; 
   
       private void OnTriggerEnter(Collider other)
       {
           if (other.CompareTag("MainCamera"))
           {
               SceneManager.LoadScene(World1); 
           }
       }
}
