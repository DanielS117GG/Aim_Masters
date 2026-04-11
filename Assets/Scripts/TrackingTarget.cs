using UnityEngine;

public class TrackingTarget : TargetBase
{
    [Header("Stats")]
    public int hitsReceived = 0;

    [Header("Movimiento")]
    public bool enableMovement = true;
    public float moveRange = 2f;
    public float moveSpeed = 2f;
    public bool useLocalAxis = false;
    public bool startMovingForward = true;

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
        direction = startMovingForward ? 1 : -1;
    }

    void Update()
    {
        if (!enableMovement) return;
        if (PracticeSessionManager.Instance == null) return;
        if (!PracticeSessionManager.Instance.sessionActive) return;

        Vector3 axis = useLocalAxis ? transform.forward : Vector3.forward;
        transform.position += axis * direction * moveSpeed * Time.deltaTime;

        float offset = Vector3.Dot(transform.position - startPosition, axis);

        if (offset >= moveRange)
        {
            transform.position = startPosition + axis * moveRange;
            direction = -1;
        }
        else if (offset <= -moveRange)
        {
            transform.position = startPosition - axis * moveRange;
            direction = 1;
        }

        PracticeSessionManager.Instance.AddTrackingAliveTime(Time.deltaTime);
    }

    public override void OnHit(Vector3 hitPoint)
    {
        hitsReceived++;

        if (PracticeSessionManager.Instance != null)
        {
            PracticeSessionManager.Instance.AddShotHit();
            PracticeSessionManager.Instance.AddTrackingHit();
        }

        HandleHitLogic();
    }

    protected override void HandleHitLogic()
    {
    }
}