using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    private SnakeController sc; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("snake"))
        {
            //Destroy(other.gameObject);
            sc = other.GetComponent<SnakeController>();
            sc.freeze();
        }
    }
}
