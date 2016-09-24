using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float Speed;

    void Start ()
    {
	
	}
	
	void FixedUpdate()
    {
        Vector3 fromCamera = transform.position - MasterController.Camera.transform.position;
        fromCamera.y = 0;
        fromCamera.Normalize();
        

        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody>().AddTorque(Quaternion.Euler(0, 90, 0) * fromCamera * Speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody>().AddTorque(Quaternion.Euler(0, -90, 0) * fromCamera * Speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().AddTorque(fromCamera * Speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().AddTorque(Quaternion.Euler(0, 180, 0) * fromCamera * Speed, ForceMode.Acceleration);
        }
    }
}
