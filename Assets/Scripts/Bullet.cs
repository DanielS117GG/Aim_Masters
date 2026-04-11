using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private Rigidbody rb;
    private bool alreadyResolved = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector3 dir, float speed)
    {
        rb.linearVelocity = dir * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (alreadyResolved) return;

        TargetBase target = collision.collider.GetComponent<TargetBase>();

        if (target == null)
            target = collision.collider.GetComponentInParent<TargetBase>();

        if (target == null)
            target = collision.collider.GetComponentInChildren<TargetBase>();

        if (target != null)
        {
            alreadyResolved = true;
            target.OnHit(collision.contacts[0].point);
            Destroy(gameObject);
            return;
        }

        alreadyResolved = true;
        PracticeSessionManager.Instance.AddShotMiss();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!alreadyResolved &&
            PracticeSessionManager.Instance != null &&
            PracticeSessionManager.Instance.sessionActive)
        {
            PracticeSessionManager.Instance.AddShotMiss();
        }
    }
}