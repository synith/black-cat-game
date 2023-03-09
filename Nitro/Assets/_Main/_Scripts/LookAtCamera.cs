using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField] bool invert;

    void Start() => cameraTransform = Camera.main.transform;


    void LateUpdate() => transform.forward = (invert ? -1 : 1) * cameraTransform.forward;
}
