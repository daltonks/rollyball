using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MasterController : MonoBehaviour
{
    public static MasterController Instance { get; private set; }
    private static int _points;
    public static int Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            Instance.PointText.text = _points.ToString();
        }
    }

    public float YDeath;
    public GameObject PlayerPrefab, CameraPrefab;
    public Transform SpawnPoint;
    public Text PointText;

    void Start ()
    {
        Instance = this;
        CameraController camera = ((GameObject) Instantiate(CameraPrefab, SpawnPoint.position, SpawnPoint.rotation)).GetComponent<CameraController>();
        PlayerController player = ((GameObject) Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation)).GetComponent<PlayerController>();
        player.Camera = camera;
        Cursor.lockState = CursorLockMode.Locked;
        PointText.text = Points.ToString();
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
