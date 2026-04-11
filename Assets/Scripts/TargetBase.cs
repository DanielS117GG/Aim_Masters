using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    public bool disableOnHit = true;

    public virtual void OnHit(Vector3 hitPoint)
    {
        HandleHitLogic();

        if (disableOnHit)
        {
            gameObject.SetActive(false);
        }
    }

    protected abstract void HandleHitLogic();
}