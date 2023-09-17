using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public GameObject trailPrefab;
    public Transform[] tarilPoints;

    bool trailAttached = false;

     List<TrailRenderer> trails = new List<TrailRenderer>();
    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        TrailAttach();
        
    }

    // ship�� trail renderer ���� -> ship �����̵� ���� ȣ��
    public void TrailAttach()
    {
        // �ߺ� ȣ�� ����
        if (trailAttached) return;
        trailAttached = true;

        // �� point ���� trailPrefab ����
        foreach (Transform tarilPoint in tarilPoints)
        {
            GameObject go = Instantiate(trailPrefab, tarilPoint);
            trails.Add(go.GetComponent<TrailRenderer>());
        }
    }

    // ship �� trail renderer �и� -> ship �����̵� ���� ȣ��
    public void TrailDistach()
    {
        // �ߺ� ȣ�� ����
        if (!trailAttached) return;
        trailAttached = false;

        // ���� ��� trail�� �и�, ȿ���� ������� �ڵ����� �����ϵ��� ����
        foreach (TrailRenderer trail in trails)
        {
            trail.transform.parent = null;
            trail.autodestruct = true;
        }
        
        trails.Clear();
    }    
}
