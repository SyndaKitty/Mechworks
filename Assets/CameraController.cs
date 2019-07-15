using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {
        // Calculate anchor
        targetAnchor += (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward) * Speed * Time.deltaTime;
        currentAnchor = Vector3.Lerp(currentAnchor, targetAnchor, 1f - Mathf.Exp(-Sharpness * Time.deltaTime));

        // Calculate zoom factor
        targetZoomFactor = Mathf.Clamp01(targetZoomFactor + Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed);
        currentZoomFactor = Mathf.Lerp(currentZoomFactor, targetZoomFactor, 1f - Mathf.Exp(-ZoomSharpness * Time.deltaTime));
        print(currentZoomFactor);

        // Evaluate curves for zoom
        Quaternion rotation = Quaternion.Euler(AngleCurve.Evaluate(currentZoomFactor), 0, 0);
        float distance = DistanceCurve.Evaluate(currentZoomFactor);

        // Calculate camera position
        transform.rotation = rotation;
        transform.position = currentAnchor + rotation * Vector3.back * distance;

    }
}
