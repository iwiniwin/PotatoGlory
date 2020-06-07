using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))][RequireComponent(typeof(CapsuleCollider2D))]
public class Missile : MonoBehaviour
{
    [Tooltip("爆炸效果")]
    public GameObject Explosion;

    [Tooltip("导弹飞行速度")]
    public float speed = 25f;

    [Tooltip("导弹是否朝向右边")]
    public bool FacingRight;

    [Tooltip("导弹造成的伤害")]
    public int DamageAmount = 10;

    [Tooltip("击退力的大小")]
    public float HurtForce = 50f;

    private Rigidbody2D m_Rigidbody2D;
    private CapsuleCollider2D m_Trigger;
    // Start is called before the first frame update

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Trigger = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        m_Rigidbody2D.velocity = new Vector2(speed, 0);

        m_Trigger.isTrigger = true;

        if(FacingRight){
            m_Rigidbody2D.velocity = new Vector2(speed, 0);
        }else{
            m_Rigidbody2D.velocity = new Vector2(-speed, 0);
        }
    }

    public void Flip(){
        FacingRight = !FacingRight;
        transform.localScale = Vector3.Scale(new Vector3(-1, 1, 1), transform.localScale);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Player")){
            return;
        }
        if(collider.CompareTag("Enemy")){
            collider.GetComponent<Enemy>().TakeDamage(this.transform, HurtForce, DamageAmount);
        }
        OnExplosion();
    }

    private void OnExplosion(){
        if(Explosion != null){
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(Explosion, transform.position, randomRotation);
        }else{
            Debug.LogWarning("请先设置Explosion");
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
