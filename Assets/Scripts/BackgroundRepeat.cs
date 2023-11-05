using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeat : MonoBehaviour {
	//스크롤할 속도를 상수로 지정해 줍니다.
	public float scrollXSpeed = 0.05f;
    public float scrollYSpeed = 0.05f;

    //Quad의 Material 데이터를 받아올 객체를 선언합니다.
    private Material thisMaterial;

	// Use this for initialization
	void Start () {
        //현재 객체의 Component들을 참조해 Renderer라는 컴포넌트의 Material정보를 받아옵니다
        thisMaterial = GetComponent<Renderer>().material; 
	}
	
	// Update is called once per frame
	void Update () 
    {    
        // 새롭게 지정해줄 OffSet 객체를 선언합니다.
        Vector2 newOffset = thisMaterial.mainTextureOffset;

        float moveX = newOffset.x + scrollXSpeed * Time.deltaTime;
        float moveY = newOffset.y + scrollYSpeed * Time.deltaTime;
        // Y부분에 현재 y값에 속도에 프레임 보정을 해서 더해줍니다.
        newOffset.Set(moveX, moveY);
        // 그리고 최종적으로 Offset값을 지정해줍니다.
        thisMaterial.mainTextureOffset = newOffset;
	}
}

