using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour
{
    public float YDeath;
    public GameObject PlayerPrefab, CameraPrefab;

    void Start ()
    {
        CameraController Camera = ((GameObject) Instantiate(CameraPrefab, transform.position, transform.rotation, transform)).GetComponent<CameraController>();
        PlayerController Player = ((GameObject) Instantiate(PlayerPrefab, transform.position, transform.rotation, transform)).GetComponent<PlayerController>();
        Player.GetComponent<PlayerController>().Camera = Camera;
        Player.Master = this;
        Camera.GetComponent<CameraController>().Player = Player;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }
}
