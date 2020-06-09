using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MedicalBoxPickup : MonoBehaviour
{
    [Tooltip("医疗箱的治疗量")]
    public float HealAmount;

    [Tooltip("被拾取时播放的音效")]
    public AudioClip PickupEffect;

    private Animator m_Animator;
    private bool m_Landed;

    private void Awake() {
        m_Animator = transform.root.GetComponent<Animator>();
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Landed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ground" && !m_Landed){
            m_Landed = true;

            transform.parent = null;  // 脱离降落伞
            gameObject.AddComponent<Rigidbody2D>();

            m_Animator.SetTrigger("Landing");
            return;
        }
        if(other.CompareTag("Player")){
            other.GetComponent<PlayerHealth>().Heal(HealAmount);

            AudioSource.PlayClipAtPoint(PickupEffect, transform.position);

            Destroy(transform.root.gameObject);
        }
    }
}
