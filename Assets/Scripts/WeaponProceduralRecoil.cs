using UnityEngine;

public class WeaponProceduralRecoil : MonoBehaviour
{
    [Header("Recoil Kick")]
    [SerializeField] private float kickBackZ = -0.05f;
    [SerializeField] private float kickUp = 2.5f;
    [SerializeField] private float sideKick = 0.6f;

    [Header("Accumulation")]
    [SerializeField] private float recoilStep = 1.0f;
    [SerializeField] private float maxRecoil = 10f;

    [Header("Recovery")]
    [SerializeField] private float returnSpeed = 8f;
    [SerializeField] private float snappiness = 14f;

    private Vector3 initialLocalPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private float accumulatedRecoil;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;

    }

    private void Update()
    {
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);

        targetRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        accumulatedRecoil = Mathf.Lerp(accumulatedRecoil, 0f, returnSpeed * 0.5f * Time.deltaTime);

        transform.localPosition = initialLocalPosition + currentPosition;
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void ApplyRecoil()
    {
        accumulatedRecoil += recoilStep;
        accumulatedRecoil = Mathf.Clamp(accumulatedRecoil, 0f, maxRecoil);

        targetPosition += new Vector3(0f, 0.01f, kickBackZ);


        targetRotation += new Vector3(-(kickUp + accumulatedRecoil),
            Random.Range(-sideKick, sideKick),
            Random.Range(-sideKick * 0.3f, sideKick * 0.3f));
    }
}
