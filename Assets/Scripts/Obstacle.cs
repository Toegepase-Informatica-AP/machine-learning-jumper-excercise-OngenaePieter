using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    private Rigidbody rb;

    [SerializeField] private float minSpeed = 2;
    [SerializeField] private float maxSpeed = 15;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.left * Random.Range(minSpeed, maxSpeed);
    }
}
