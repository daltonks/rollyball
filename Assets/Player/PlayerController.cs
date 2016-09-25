using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float MaxVelocitySquared;
    public float JumpSpeed;
    public float JumpInAirMaxTime;
    public float MinTimeBetweenJumps;

    private float FallingTime = 0;
    private bool IsFalling = false;
    private bool IsInJumpMode = false;
    private bool HasWallJumped = false;
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
                        AddJumpForce();
                    }
                }
                else
                {
                    IsInJumpMode = false;
                }
            }
            else
            {
                if (!HasWallJumped && Input.GetKey(KeyCode.Space) && JumpAccum >= MinTimeBetweenJumps)
                {
                    Debug.Log("Try wall jump");
                    foreach (Vector3 vec in WallRaycastVectors)
                    {
                        if (Physics.Raycast(transform.position, vec, Radius + .05f))
                        {
                            Debug.Log("Wall jump!");
                            AddJumpForce();
                            JumpAccum = 0;
                            FallingTime = 0;
                            IsFalling = true;
                            IsInJumpMode = true;
                            HasWallJumped = true;
                            break;
                        }
                    }
                }
            }
        }
        else if (Input.GetKey(KeyCode.Space) && JumpAccum >= MinTimeBetweenJumps)
        {
            foreach(Vector3 vec in GroundRaycastVectors)
            {
                if(Physics.Raycast(transform.position, vec, Radius + .05f))
                {
                    AddJumpForce();
                    JumpAccum = 0;
                    FallingTime = 0;
                    IsFalling = true;
                    IsInJumpMode = true;
                    HasWallJumped = false;
                    break;
                }
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
        foreach(ContactPoint contactPoint in collision.contacts)
        {
            if(contactPoint.normal.y > .1)
            {
                IsFalling = false;
                Debug.Log("Not falling: " + contactPoint.normal.y);
            }
        }
    }
}
