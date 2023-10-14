using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڵ� ���� ����� �ùٸ��� �������� �ʴ� ���� �����ϱ� ���� ���
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
