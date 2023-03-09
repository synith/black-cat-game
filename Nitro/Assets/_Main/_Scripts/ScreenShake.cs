using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    CinemachineImpulseSource impulseSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float force = 1f)
    {
        impulseSource.GenerateImpulse(force);
    }
}
