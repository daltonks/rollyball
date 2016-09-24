using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float CameraSpeed;
    public float DistanceToPlayer, OffsetY;
    public PlayerController Player { get; set; }

    void Start()
    {
        transform.position = transform.position - transform.rotation * Vector3.forward;
        LerpToPlayer(1);
    }

    void FixedUpdate()
    {
        if(Player == null)
        {
            return;
        }
        LerpToPlayer(Time.deltaTime * CameraSpeed);
    }

    void LerpToPlayer(float lerpValue)
    {
        Vector3 newLocation = Player.transform.position + (transform.position - Player.transform.position).normalized * DistanceToPlayer;
        newLocation.y = Player.transform.position.y + OffsetY;
        transform.position = Vector3.Lerp(transform.position, newLocation, lerpValue);
    }

    void LateUpdate ()
    {
        if (Player == null)
        {
            return;
        }
        Quaternion quat = new Quaternion();
        quat.SetLookRotation(Player.transform.position - transform.position + Vector3.up * .6f);
        transform.rotation = quat;
	}
}
