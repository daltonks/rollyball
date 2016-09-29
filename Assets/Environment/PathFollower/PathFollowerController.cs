using UnityEngine;
using System.Collections;

public class PathFollowerController : MonoBehaviour
{
    public Transform MovingObject;
    public Transform PathHolder;
    public float Speed;

    private Transform[] Points;
    private int PointIndex = 0;
    private Transform NextPoint;

	void Start ()
    {
        Points = new Transform[PathHolder.childCount];
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i] = PathHolder.GetChild(i);
        }
        MovingObject.position = Points[0].position;
        IncreasePathPoint();
    }
	
	void FixedUpdate ()
    {
        Vector3 toNext = NextPoint.position - MovingObject.position;
        Vector3 toNextWithSpeed = toNext.normalized * Speed * Time.fixedDeltaTime;
        if(toNextWithSpeed.magnitude > toNext.magnitude)
        {
            MovingObject.position += toNext;
            IncreasePathPoint();
        }
        else
        {
            MovingObject.position += toNextWithSpeed;
        }
	}

    void IncreasePathPoint()
    {
        PointIndex++;
        if (PointIndex == Points.Length)
        {
            PointIndex = 0;
        }
        NextPoint = Points[PointIndex];
    }
}
