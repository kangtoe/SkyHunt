using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 자동 삭제 기능이 올바르게 동작하지 않는 것을 보완하기 위해 사용
public class ManualDestroy : MonoBehaviour
{
    public float livetime = 5f;

    bool destroyProcessing = false;

    private void Update()
    {
        if (destroyProcessing) return;

        if (transform.parent == null)
        {
            //Debug.Log("Destroy after : " + livetime);
            Destroy(gameObject, livetime);
            destroyProcessing = true;
        }
    }
}
