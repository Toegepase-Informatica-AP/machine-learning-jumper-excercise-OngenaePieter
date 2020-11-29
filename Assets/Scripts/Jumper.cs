using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using TMPro;

public class Jumper : Agent
{
    [SerializeField] private float jumpForce;
    [SerializeField] private KeyCode jumpKey;
    
    private bool jumpIsReady = true;
    private Rigidbody rBody;
    private Vector3 startingPosition;
    public TextMeshPro score;
    public Jumper j;

    public event Action OnReset;
    
    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (jumpIsReady)
        {
            RequestDecision();
        }

        score.text = j.GetCumulativeReward().ToString();

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            Jump();
        }
            
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        
        if (Input.GetKey(jumpKey))
        {
            actionsOut[0] = 1;
        }
            
    }

    private void Jump()
    {
        if (jumpIsReady)
        {
            rBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            jumpIsReady = false;
            AddReward(-0.1f);
        }
    }
    
    private void Reset()
    {
        jumpIsReady = true;
        transform.position = startingPosition;
        rBody.velocity = Vector3.zero;
        
        OnReset?.Invoke();
    }

    private void OnCollisionEnter(Collision collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Street"))
        {
            jumpIsReady = true;
        }
    }


    void OnTriggerEnter(Collider collidedObj)
    { 
        if (collidedObj.gameObject.tag == "JumpedOver" || collidedObj.gameObject.tag == "JumpedOver2")
        {
            AddReward(0.5f);
        }

        if (collidedObj.gameObject.tag == "Obstacle")
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

}
