using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    private Rigidbody rb;
    private bool alreadyResolved = false;
    private Coroutine deactivateCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void OnEnable()
    {
        alreadyResolved = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        deactivateCoroutine = StartCoroutine(DeactivateAfterTime());
    }
    public void SetDirection(Vector3 dir, float speed)
    {
        rb.linearVelocity = dir * speed;
    }

    IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (alreadyResolved && PracticeSessionManager.Instance != null)
        { PracticeSessionManager.Instance.AddShotMiss(); }
        Deactivate();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (alreadyResolved) return;

        TargetBase target = collision.collider.GetComponent<TargetBase>();

        if (target == null)
            target = collision.collider.GetComponentInParent<TargetBase>();

        if (target == null)
            target = collision.collider.GetComponentInChildren<TargetBase>();

        alreadyResolved = true;

        if (target != null)
        {

            target.OnHit(collision.contacts[0].point);

        }
        else
        {


            PracticeSessionManager.Instance.AddShotMiss();

        }
        Deactivate();
    }

    void Deactivate()
    {
        if (deactivateCoroutine != null) StopCoroutine(deactivateCoroutine);
        gameObject.SetActive(false);
    }
}