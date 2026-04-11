using UnityEngine;

public class SimpleMovingTarget : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    Vector3 targetPos;

    void Start()
    {
        targetPos = pointB.position;
    }

    void Update()
    {
        if (!PracticeSessionManager.Instance.sessionActive) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            if (targetPos == pointA.position)
                targetPos = pointB.position;
            else
                targetPos = pointA.position;
        }

        PracticeSessionManager.Instance.AddTrackingAliveTime(Time.deltaTime);
    }
}
