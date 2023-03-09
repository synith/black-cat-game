using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField, Range(0, 60)] float lifeTime;
    void Start() => Destroy(gameObject, lifeTime);
}
