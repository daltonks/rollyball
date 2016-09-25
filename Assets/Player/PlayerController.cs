using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public float MinTimeBetweenJumps;
    public float Speed;
    public float JumpSpeed;
    public float JumpInAirMaxTime;
    private float FallingTime = 0;
    private bool IsFalling = false;
    private bool IsInJumpMode = false;
    private float Radius;
    private Rigidbody RigidBody;
    private Vector3[] GroundRaycastVectors, WallRaycastVectors;
    private float JumpAccum;
    public CameraController Camera { get; set; }
    public MasterController Master { get; set; }

    void Start ()
    {
        JumpAccum = MinTimeBetweenJumps;
        Radius = GetComponent<MeshRenderer>().bounds.extents.x;

        RigidBody = GetComponent<Rigidbody>();
        //Coordinates created by making a UV sphere in Blender
        GroundRaycastVectors = new Vector3[]
        {
            new Vector3(0, -1, 0),
            new Vector3(0.11126f, -0.86603f, 0.48746f),
            new Vector3(0.45048f, -0.86603f, 0.21694f),
            new Vector3(0.45048f, -0.86603f, -0.21694f),
            new Vector3(0.11126f, -0.86603f, -0.48746f),
            new Vector3(-0.31174f, -0.86603f, -0.39092f),
            new Vector3(-0.5f, -0.86603f, 0.0f),
            new Vector3(-0.31175f, -0.86603f, 0.39092f),
            new Vector3(0.19271f, -0.5f, 0.84431f),
            new Vector3(0.78026f, -0.5f, 0.37575f),
            new Vector3(0.78026f, -0.5f, -0.37575f),
            new Vector3(0.19271f, -0.5f, -0.84431f),
            new Vector3(-0.53996f, -0.5f, -0.67709f),
            new Vector3(-0.86603f, -0.5f, -0.0f),
            new Vector3(-0.53996f, -0.5f, 0.67709f)
        };
        WallRaycastVectors = new Vector3[]
        {
            new Vector3(0.22252f, -0.0f, 0.97493f),
            new Vector3(0.90097f, -0.0f, 0.43388f),
            new Vector3(0.90097f, -0.0f, -0.43388f),
            new Vector3(0.22252f, -0.0f, -0.97493f),
            new Vector3(-0.62349f, -0.0f, -0.78183f),
            new Vector3(-1.0f, -0.0f, -0.0f),
            new Vector3(-0.62349f, -0.0f, 0.78183f)
        };
    }
	
	void FixedUpdate()
    {
        if(transform.position.y <= Master.YDeath)
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
        if(fromCamera.magnitude < .5f)
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
            AddMovementForces(Quaternion.identity, fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            AddMovementForces(Quaternion.Euler(0, 180, 0), fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            AddMovementForces(Quaternion.Euler(0, -90, 0), fromCamera, tickSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            AddMovementForces(Quaternion.Euler(0, 90, 0), fromCamera, tickSpeed);
        }

        JumpAccum += Time.fixedDeltaTime;
        if(IsFalling)
        {
            if(IsInJumpMode)
            {
                FallingTime += Time.fixedDeltaTime;
                if (Input.GetKey(KeyCode.Space))
                {
                    if(FallingTime >= JumpInAirMaxTime)
                    {
                        IsInJumpMode = false;
                    }
                    else
                    {
                        AddUpwardForce();
                    }
                }
                else
                {
                    IsInJumpMode = false;
                }
            }
        }
        else if (Input.GetKey(KeyCode.Space) && JumpAccum >= MinTimeBetweenJumps)
        {
            foreach(Vector3 vec in GroundRaycastVectors)
            {
                if(Physics.Raycast(transform.position, vec, Radius + .05f))
                {
                    AddUpwardForce();
                    JumpAccum = 0;
                    FallingTime = 0;
                    IsFalling = true;
                    IsInJumpMode = true;
                    break;
                }
            }
        }
    }

    void AddUpwardForce()
    {
        RigidBody.AddForce(Vector3.up * JumpSpeed, ForceMode.Acceleration);
    }

    void AddMovementForces(Quaternion rotation, Vector3 vector, float speed)
    {
        RigidBody.AddForce(rotation * vector * speed, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contactPoint in collision.contacts)
        {
            if(contactPoint.normal.y > -.1)
            {
                IsFalling = false;
            }
        }
    }
}
