using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCenterAround : MonoBehaviour
{
    public float AroundRadius = 1;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 lookAt = Vector2.zero + Random.insideUnitCircle * AroundRadius;
        Vector2 dir = lookAt - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
