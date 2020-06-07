using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("最大生命值")]
    public float MaxHP = 100f;

    [Tooltip("角色受伤后的免伤时间")]
    public float FreeDamagePeriod = 0.35f;

    [Tooltip("角色的受伤音效")]
    public AudioClip[] OuchClips;

    [Tooltip("血量条")]
    public SpriteRenderer HealthSprite;

    private Vector3 m_InitHealthScale;

    private float m_CurrentHP;
    // 上一次受到伤害的时间
    private float m_LastFreeDamageTime;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHP = MaxHP;
        m_LastFreeDamageTime = 0f;
        m_InitHealthScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnCollisionEnter2D(Collision2D collision){
    //     if(collision.gameObject.tag == "Enemy"){
    //         if(Time.time > m_LastFreeDamageTime + FreeDamagePeriod){
    //             if(m_CurrentHP > 0f){
    //                 TakeDamage(collision.transform);
    //                 m_LastFreeDamageTime = Time.time;
    //             }else{
    //                 // 角色死亡
    //                 Death();
    //             }
    //         }
    //     }
    // }

    public void TakeDamage(Transform enemy, float hurtForce, float damage){
        // 处于免伤状态
        if(Time.time <= m_LastFreeDamageTime + FreeDamagePeriod){
            return;
        }
        m_LastFreeDamageTime = Time.time;

        Vector3 hurtVector = (transform.position - enemy.position) + Vector3.up * 5f;
        // hurtVector.x *= 10f;
        GetComponent<Rigidbody2D>().AddForce(hurtVector.normalized * hurtForce);

        m_CurrentHP -= damage;

        // 更新生命条
        UpdateHealthBar();

        int  i = Random.Range(0, OuchClips.Length);
        AudioSource.PlayClipAtPoint(OuchClips[i], transform.position);

        if(m_CurrentHP <= 0f){
            Death();
        }
    }

    public void UpdateHealthBar(){
        if(HealthSprite != null){
            HealthSprite.color = Color.Lerp(Color.green, Color.red, 1 - m_CurrentHP * 0.01f);
            HealthSprite.transform.localScale = Vector3.Scale(new Vector3(m_CurrentHP * 0.01f, 1, 1), m_InitHealthScale);
        }else{
            Debug.LogError("请设置Health Sprite");
        }
    }

    public void Death(){
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols){
            // c.enabled = false;
            c.isTrigger = true;
        }
        GetComponent<PlayerController>().enabled = false;

        GetComponent<Animator>().SetTrigger("Death");
    }
}
