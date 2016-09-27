using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Vector3 StartingLoc;
    private Quaternion StartingRot;
    public float CameraSpeed;
    public float DistanceToPlayer, LookOffsetY;
    public PlayerController Player { get; set; }
    private Vector3 LastMousePos;

    void Start()
    {
        StartingLoc = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        StartingRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Reset();
        LastMousePos = Input.mousePosition;
    }

    public void Reset()
    {
        transform.position = StartingLoc - StartingRot * Vector3.forward;
        //LerpToPlayer(1);
    }

    void FixedUpdate()
    {
        if(Player == null)
        {
            return;
        }
        //LerpToPlayer(1);
        /*
        Quaternion quat = new Quaternion();
        quat.SetLookRotation(Player.transform.position + Vector3.up * LookOffsetY + Player.RigidBody.velocity * LookPlayerVelocityMult - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.fixedDeltaTime * LookLerpSpeed);*/
    }

    void LateUpdate()
    {
        Vector3 mouseDif = Input.mousePosition - LastMousePos;
        transform.RotateAround(Player.transform.position, Vector3.up, mouseDif.x);
        transform.RotateAround(Player.transform.position, Vector3.left, mouseDif.y);
        LastMousePos = Input.mousePosition;
        transform.position = Player.transform.position + transform.rotation * Vector3.back * DistanceToPlayer;
    }

    /*
    void LerpToPlayer(float lerpValue)
    {
        Vector3 newLocation = Player.transform.position + (transform.position - Player.transform.position).normalized * DistanceToPlayer;
        newLocation.y = Player.transform.position.y + OffsetY;
        Vector3 dif = newLocation - Player.transform.position;
        RaycastHit[] raycastHits = Physics.RaycastAll(Player.transform.position, dif, dif.magnitude);
        if(raycastHits.Length > 0)
        {
            float minDistance = float.MaxValue;
            Vector3 minPoint = newLocation;
            foreach (RaycastHit hit in raycastHits)
            {
                if (hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                    minPoint = hit.point;
                }
            }
            newLocation = minPoint;
        }
        transform.position = Vector3.Lerp(transform.position, newLocation, lerpValue);
    }*/
}
