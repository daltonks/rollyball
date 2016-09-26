using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    public float TimeBetweenJumps;
    public float JumpAfterCollideLingerDuration;

    private float FallingTime = 0;
    private float JumpAfterCollideCountdown = 0;
    private float WallJumpAfterCollideCountdown = 0;
    private bool IsInJumpMode = false;
    private bool HasWallJumped = false;
    private Rigidbody RigidBody;
    private float TimeBetweenJumpsAccum;

    public CameraController Camera { get; set; }
    public MasterController Master { get; set; }

    void Start ()
    {
        TimeBetweenJumpsAccum = TimeBetweenJumps;
        RigidBody = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate()
    {
        if (RigidBody.IsSleeping())
            RigidBody.WakeUp();

        JumpAfterCollideCountdown -= Time.fixedDeltaTime;
        WallJumpAfterCollideCountdown -= Time.fixedDeltaTime;

        if (transform.position.y <= Master.YDeath)
        {
            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            transform.position = Master.transform.position;
            transform.rotation = Master.transform.rotation;
            Camera.Reset();
            return;
        }

        //Rotation
        Vector3 fromCamera = transform.position - Camera.transform.position;
        fromCamera.y = 0;
        fromCamera.Normalize();
        if(fromCamera.magnitude < .1f)
        {
            fromCamera = Vector3.forward;
        }
        float tickSpeed = Speed;
        if(  (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
          && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            tickSpeed *= .5f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            AddMovementForce(Quaternion.identity, fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            AddMovementForce(Quaternion.Euler(0, 180, 0), fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            AddMovementForce(Quaternion.Euler(0, -90, 0), fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            AddMovementForce(Quaternion.Euler(0, 90, 0), fromCamera, tickSpeed);
        }
        
        TimeBetweenJumpsAccum += Time.fixedDeltaTime;

        if(IsInJumpMode)
        {
            FallingTime += Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                if (FallingTime >= TimeBetweenJumps)
                {
                    IsInJumpMode = false;
                }
                else
                {
                    AddJumpForce();
                }
            }
            else
            {
                IsInJumpMode = false;
            }
        }
        else if(Input.GetKey(KeyCode.Space) && TimeBetweenJumpsAccum >= TimeBetweenJumps)
        {
            if (JumpAfterCollideCountdown > 0)
            {
                AddJumpForce();
                TimeBetweenJumpsAccum = 0;
                FallingTime = 0;
                IsInJumpMode = true;
                HasWallJumped = false;
                JumpAfterCollideCountdown = 0;
                Debug.Log(DateTime.Now + ": Jump!");
            }
            else if(!HasWallJumped && WallJumpAfterCollideCountdown > 0)
            {
                AddJumpForce();
                TimeBetweenJumpsAccum = 0;
                FallingTime = 0;
                IsInJumpMode = true;
                HasWallJumped = true;
                WallJumpAfterCollideCountdown = 0;
                Debug.Log(DateTime.Now + ": Wall Jump!");
            }
        }
    }

    void AddJumpForce()
    {
        RigidBody.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Acceleration);
    }

    void AddMovementForce(Quaternion rotation, Vector3 vector, float speed)
    {
        RigidBody.AddForce(rotation * vector * speed, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckCollisionForJumpEligibility(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        CheckCollisionForJumpEligibility(collision);
    }

    void CheckCollisionForJumpEligibility(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.normal.y > .1)
            {
                JumpAfterCollideCountdown = JumpAfterCollideLingerDuration;
                break;
            }
            else if(contactPoint.normal.y > -.2)
            {
                WallJumpAfterCollideCountdown = JumpAfterCollideLingerDuration;
            }
        }
    }
}
