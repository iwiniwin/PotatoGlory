using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Tooltip("角色受伤时减少的血量")]
    public float DamageAmount = 10f;

    [Tooltip("角色被怪物伤害时受到的击退力大小")]
    public float HurtForce = 500f;

    [Tooltip("障碍物检测点")]
    public Transform m_FrontCheck;

    [Tooltip("怪物的血量")]
    public float MaxHP = 10f;

    [Tooltip("怪物受伤时用来展示的图片")]
    public Sprite DamagedSprite;

    [Tooltip("怪物死亡时用来展示的图片")]
    public Sprite DeadSprite;

    [Tooltip("怪物死亡时用来展示的DeadSprite")]
    public SpriteRenderer BodySpriteRenderer;

    [Tooltip("怪物死亡时的音效")]
    public AudioClip[] DeathClips;

    private Wander m_Wander;
    private Rigidbody2D m_Rigidbody2D;

    private LayerMask m_LayerMask;
    private float m_CurrentHP;
    private bool m_Hurt;
    private bool m_Dead;

    private void Awake() {
        m_Wander = GetComponent<Wander>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LayerMask = LayerMask.GetMask("Obstacle");   
        m_CurrentHP = MaxHP;
        m_Hurt = false;
        m_Dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Dead){
            return;
        }
        Collider2D[] frontHits = Physics2D.OverlapPointAll(m_FrontCheck.position, m_LayerMask);
        if(frontHits.Length > 0){
            m_Wander.Flip();
        }
    }

    public void TakeDamage(Transform weapon, float hrutForce, float damage){
        m_CurrentHP -= damage;

        // 制造击退效果
        Vector3 hurtVector = transform.position - weapon.position;
        m_Rigidbody2D.AddForce(hurtVector.normalized * hrutForce);

        if(!m_Hurt){
            m_Hurt = true;
            if(DamagedSprite != null){
                // 禁用原有的sprite
                SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer child in children){
                    child.enabled = false;
                }

                // 显示怪物受伤的图片
                if(BodySpriteRenderer != null){
                    BodySpriteRenderer.enabled = true;
                    BodySpriteRenderer.sprite = DamagedSprite;
                }else{
                    Debug.LogWarning("请设置BodySpriteRenderer");
                }
            }else{
                Debug.LogWarning("请设置DamagedSprite");
            }
        }

        if(m_CurrentHP <= 0 && !m_Dead){
            m_Dead = true;
            Death();
        }
    }

    public void Death(){
        m_Wander.enabled = false;
        if(DeadSprite != null){
            // 获取自身和子物体上所有目标组件的引用，注意，也包括自身
            SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer child in children){
                child.enabled = false;
            }

            if(BodySpriteRenderer != null){
                BodySpriteRenderer.enabled = true;
                BodySpriteRenderer.sprite = DeadSprite;
            }else{
                Debug.LogWarning("请设置BodySpriteRenderer");
            }
        }else{
            Debug.LogWarning("请设置DeadSprite");
        }

        // 将所有的Collider2D都设置为Trigger，避免和其他物体发生碰撞
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols){
            c.isTrigger = true;
        }

        // 随机播放死亡的音效
        if(DeathClips != null && DeathClips.Length > 0){
            int i = Random.Range(0, DeathClips.Length);
            // 静态函数
            AudioSource.PlayClipAtPoint(DeathClips[i], transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(this.transform, HurtForce, DamageAmount);
        }
    }
}
