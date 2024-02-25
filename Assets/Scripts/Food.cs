using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    private SnakeController sc;
    public float cubeSize = 20f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("snake"))
        {
            float halfSize = cubeSize / 2f;
            
            float randomX = Random.Range(-halfSize, halfSize);
            float randomZ = Random.Range(-halfSize, halfSize);
            
            //Destroy(gameObject);
            
            transform.position = new Vector3(randomX, 1f, randomZ);
            sc = other.GetComponent<SnakeController>();
            sc.GrowSnake();
            sc.points();
        }
    }
}
