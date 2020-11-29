using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle2 : MonoBehaviour
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
        rb.velocity = Vector3.right * Random.Range(minSpeed, maxSpeed);
    }
}
