using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bomb : MonoBehaviour
{
    [Tooltip("炸弹产生的伤害值")]
    public float DamageAmount = 50f;

    [Tooltip("爆炸半径")]
    public float BombRadius = 10f;

    [Tooltip("爆炸时产生的冲击力")]
    public float BombForce = 800f;

    [Tooltip("炸弹爆炸时的音效")]
    public AudioClip BoomClip;

    [Tooltip("引信燃烧的时间")]
    public float fuseTime = 1.5f;

    [Tooltip("燃烧引信的音效")]
    public AudioClip FuseClip;

    [Tooltip("炸弹爆炸时的特效")]
    public GameObject BoomExplosion;

    private LayerMask m_LayerMask;
    private AudioSource m_AudioSource;

    private void Awake() {
        m_AudioSource = GetComponent<AudioSource>();
        // 取消默认播放
        m_AudioSource.playOnAwake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 设置LayerMask检测Enemy和Player
        m_LayerMask = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Player");
        // transform.root Returns the topmost transform in the hierarchy.
        // 如果不是附着在其他物体上，就开始执行燃烧引信的协程
        if(transform.root == transform){
            StartCoroutine(BombDetonation());
        }
    }

    private IEnumerator BombDetonation(){
        if(FuseClip != null){
            m_AudioSource.clip = FuseClip;
            m_AudioSource.Play();
        }else{
            Debug.LogWarning("请设置FuseClip");
        }
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    public void Explode(){
        // 获取一定范围内的所有Layer为Enemy或者Player的物体
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, BombRadius, m_LayerMask);

        foreach(Collider2D obj in objects){
            if(obj.tag == "Enemy"){
                obj.GetComponent<Enemy>().TakeDamage(this.transform, BombForce, DamageAmount);
                continue;
            }
            if(obj.CompareTag("Player")){
                obj.GetComponent<PlayerHealth>().TakeDamage(this.transform, BombForce, DamageAmount);
            }   
        }

        if(BoomExplosion != null){
            Instantiate(BoomExplosion, this.transform.position, Quaternion.identity);
        }

        // 播放爆炸音效
        if(BoomClip != null){
            AudioSource.PlayClipAtPoint(BoomClip, this.transform.position);
        }

        // 直接删除父物体
        Destroy(transform.root.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
