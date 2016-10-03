using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public delegate void JumpDelegate(PlayerController player, bool wallJump);
    public static event JumpDelegate JumpEvent;

    public float Speed;
    public float JumpSpeed;
    public float TimeBetweenJumps;
    public float JumpAfterCollideLingerDuration;

    private float FallingTime = 0;
    private float JumpAfterCollideCountdown = 0;
    private float WallJumpAfterCollideCountdown = 0;
    private bool IsInJumpMode = false;
    private bool HasLetGoOfJump = true;
    private bool HasWallJumped = false;
    private float TimeBetweenJumpsAccum;

    public Rigidbody RigidBody { get; private set; }
    public CameraController Camera { get; set; }

    void Start ()
    {
        Instance = this;
        TimeBetweenJumpsAccum = TimeBetweenJumps;
        RigidBody = GetComponent<Rigidbody>();
    }
	
	void FixedUpdate()
    {
        if (RigidBody.IsSleeping())
            RigidBody.WakeUp();

        JumpAfterCollideCountdown -= Time.fixedDeltaTime;
        WallJumpAfterCollideCountdown -= Time.fixedDeltaTime;

        if (transform.position.y <= MasterController.Instance.YDeath)
        {
            RigidBody.velocity = Vector3.zero;
            RigidBody.angularVelocity = Vector3.zero;
            transform.position = MasterController.Instance.SpawnPoint.transform.position;
            transform.rotation = MasterController.Instance.SpawnPoint.transform.rotation;
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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            HasLetGoOfJump = true;
        }

        if (IsInJumpMode)
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
        else if(HasLetGoOfJump && Input.GetKey(KeyCode.Space) && TimeBetweenJumpsAccum >= TimeBetweenJumps)
        {
            bool jumped = false;
            if (JumpAfterCollideCountdown > 0)
            {
                HasWallJumped = false;
                JumpAfterCollideCountdown = 0;
                if (JumpEvent != null)
                    JumpEvent.Invoke(this, false);
                jumped = true;
                Debug.Log(DateTime.Now + ": Jump!");
            }
            else if (!HasWallJumped && WallJumpAfterCollideCountdown > 0)
            {
                HasWallJumped = true;
                WallJumpAfterCollideCountdown = 0;
                if (JumpEvent != null)
                    JumpEvent.Invoke(this, true);
                jumped = true;
                Debug.Log(DateTime.Now + ": Wall Jump!");
            }
            if(jumped)
            {
                AddJumpForce();
                TimeBetweenJumpsAccum = 0;
                FallingTime = 0;
                IsInJumpMode = true;
                HasLetGoOfJump = false;
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
            }
            else if(contactPoint.normal.y > -.2)
            {
                WallJumpAfterCollideCountdown = JumpAfterCollideLingerDuration;
            }
        }
    }
}
