using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    static MouseWorld instance;

    [SerializeField] LayerMask mousePlaneLayerMask;

    void Awake()
    {
        instance = this;  
    }

    public static Vector3 GetPosition()
    {
        //Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

        //Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mousePlaneLayerMask);

        //return hitInfo.point;

        return Vector3.zero;
    }
}
