using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed;
    public float Sharpness;
    public AnimationCurve DistanceCurve;
    public AnimationCurve AngleCurve;
    public float ZoomSharpness;
    public float ZoomSpeed = .1f;

    float targetZoomFactor = 1;
    float currentZoomFactor = 1;

    Vector3 targetAnchor;
    Vector3 currentAnchor;

    float maxZoomFactor;
    void Awake()
    {
        foreach (var zf in DistanceCurve.keys)
        {
            if (zf.time > maxZoomFactor) maxZoomFactor = zf.time;
        }
    }

    void Update()
    {
        // Calculate anchor
        targetAnchor += (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward) * Speed * Time.deltaTime;
        currentAnchor = Vector3.Lerp(currentAnchor, targetAnchor, 1f - Mathf.Exp(-Sharpness * Time.deltaTime));

        // Calculate zoom factor
        targetZoomFactor = Mathf.Clamp(targetZoomFactor + Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed, 0, maxZoomFactor);
        currentZoomFactor = Mathf.Lerp(currentZoomFactor, targetZoomFactor, 1f - Mathf.Exp(-ZoomSharpness * Time.deltaTime));

        // Evaluate curves for zoom
        Quaternion rotation = Quaternion.Euler(AngleCurve.Evaluate(currentZoomFactor), 0, 0);
        float distance = DistanceCurve.Evaluate(currentZoomFactor);

        // Calculate camera position
        transform.rotation = rotation;
        transform.position = currentAnchor + rotation * Vector3.back * distance;

    }
}
