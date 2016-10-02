using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Vector3 StartingLoc;
    private Quaternion StartingRot;
    public float CameraSpeed;
    public float DistanceToPlayer, LookOffsetY;
    private float RotX, RotY;

    void Start()
    {
        StartingLoc = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        StartingRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Reset();
    }

    public void Reset()
    {
        transform.position = StartingLoc - StartingRot * Vector3.forward;
        transform.rotation = StartingRot;
        Vector3 rot = StartingRot.eulerAngles;
        RotX = rot.x;
        RotY = rot.y;
    }

    private const float ClampAngle = 80.0f;
    void LateUpdate()
    {
        if(Cursor.lockState != CursorLockMode.None)
        {
            RotX += -Input.GetAxis("Mouse Y") * CameraSpeed * Time.deltaTime;
            RotY += Input.GetAxis("Mouse X") * CameraSpeed * Time.deltaTime;
            if (RotY > 180)
            {
                RotY -= 360;
            }
            if (RotY < -180)
            {
                RotY += 360;
            }

            RotX = Mathf.Clamp(RotX, -ClampAngle, ClampAngle);

            transform.rotation = Quaternion.Euler(RotX, RotY, 0.0f);
        }

        Vector3 newPosition = PlayerController.Instance.transform.position + transform.rotation * Vector3.back * DistanceToPlayer + Vector3.up * LookOffsetY;
        RaycastHit hit;
        if(Physics.Linecast(PlayerController.Instance.transform.position, newPosition, out hit))
        {
            newPosition = hit.point;
        }
        transform.position = newPosition;
    }
}
