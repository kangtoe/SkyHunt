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

    // ship에 trail renderer 생성 -> ship 순간이동 직후 호출
    public void TrailAttach()
    {
        // 중복 호출 방지
        if (trailAttached) return;
        trailAttached = true;

        // 각 point 마다 trailPrefab 생성
        foreach (Transform tarilPoint in tarilPoints)
        {
            GameObject go = Instantiate(trailPrefab, tarilPoint);
            trails.Add(go.GetComponent<TrailRenderer>());
        }
    }

    // ship 과 trail renderer 분리 -> ship 순간이동 직전 호출
    public void TrailDistach()
    {
        // 중복 호출 방지
        if (!trailAttached) return;
        trailAttached = false;

        // 현재 모든 trail들 분리, 효과가 사라질때 자동으로 삭제하도록 변경
        foreach (TrailRenderer trail in trails)
        {
            trail.transform.parent = null;
            trail.autodestruct = true;
        }
        
        trails.Clear();
    }    
}
