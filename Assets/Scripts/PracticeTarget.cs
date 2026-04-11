using UnityEngine;

public class PracticeTarget : MonoBehaviour
{
    public bool isTrackingTarget = true;
    public int hitsReceived = 0;

    public void ReceiveHit()
    {
        hitsReceived++;

        if (isTrackingTarget)
            PracticeSessionManager.Instance.AddTrackingHit();
    }
}
