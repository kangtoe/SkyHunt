using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ȯ��ǰ�, �帴����
public class Pulse : BulletBase
{
    [Header("Pulse Info")]
    SpriteRenderer sprite; // ������ ������ ������
    
    public float expansionSpeed = 1f; // Ȯ�� �ӵ�
    public float expansionMax = 1f; // �ִ� Ȯ�� ������
    float currentExpansion = 0;
    float attackableRatio = 0.8f; // �������  Ȯ�� ��, ����ϰ� ����� ���� ���� ���� ������ �����.

    // Start is called before the first frame update
    override protected void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.transform.localScale = Vector2.one * currentExpansion;

        //Debug.Log("expansionMax : " + expansionMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentExpansion < expansionMax)
        {
            currentExpansion += Time.deltaTime * expansionSpeed;
        }
        else
        {
            Destroy(gameObject);
        }

        //Debug.Log("currentExpansion : " + currentExpansion);

        sprite.transform.localScale = Vector2.one * currentExpansion;
        
        float fadeStartRatio = 0.5f;
        float t;
        if (currentExpansion / expansionMax < fadeStartRatio) t = 0f;
        else t = currentExpansion / expansionMax / fadeStartRatio - 2 * fadeStartRatio;
        //Debug.Log("t : " + t);

        Color color = sprite.color;
        color.a = Mathf.Lerp(1f, 0f, t);
        sprite.color = color;
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        if (currentExpansion / expansionMax > attackableRatio) return;

        //Debug.Log("other:" + other.name);

        // targetLayer �˻�
        if (1 << other.gameObject.layer == targetLayer.value)
        {
            //Debug.Log("name:" + name + ", hit damege:" + damage);

            // �����ְ�
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.GetDamaged(damage);
            }

            Rigidbody2D rbody = other.GetComponent<Rigidbody2D>();
            if (rbody)
            {
                rbody.AddForce(transform.up * impact, ForceMode2D.Impulse);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if(!sprite) sprite = GetComponent<SpriteRenderer>();
    //    float size = sprite.sprite.bounds.size.x;

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, size * expansionMax / 2f);
    //}
}
