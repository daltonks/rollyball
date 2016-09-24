using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour
{
    public GameObject PlayerPrefab, CameraPrefab;

    void Start ()
    {
        CameraController Camera = ((GameObject) Instantiate(CameraPrefab, transform.position, transform.rotation, transform)).GetComponent<CameraController>();
        PlayerController Player = ((GameObject) Instantiate(PlayerPrefab, transform.position, transform.rotation, transform)).GetComponent<PlayerController>();
        Player.GetComponent<PlayerController>().Camera = Camera;
        Camera.GetComponent<CameraController>().Player = Player;
    }
}
