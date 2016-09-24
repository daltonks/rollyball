using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float CameraSpeed;
    public float DistanceToPlayer, OffsetY;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(MasterController.Player == null)
        {
            return;
        }
        Vector3 newLocation = MasterController.Player.transform.position + (transform.position - MasterController.Player.transform.position).normalized * DistanceToPlayer;
        newLocation.y = MasterController.Player.transform.position.y + OffsetY;
        transform.position = Vector3.Lerp(transform.position, newLocation, Time.deltaTime * CameraSpeed);
    }

    void LateUpdate ()
    {
        if (MasterController.Player == null)
        {
            return;
        }
        Quaternion quat = new Quaternion();
        quat.SetLookRotation(MasterController.Player.transform.position - transform.position + Vector3.up * .6f);
        transform.rotation = quat;
	}
}
