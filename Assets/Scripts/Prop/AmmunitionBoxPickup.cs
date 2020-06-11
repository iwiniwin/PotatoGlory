using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class AmmunitionBoxPickup : MonoBehaviour
{
    [Tooltip("增加的炸弹数")]
    public int BombAmount = 1;

    [Tooltip("被拾取时播放的音效")]
    public AudioClip PickupEffect;

    private Animator m_Animator;
    private bool m_Landed;

    private void Awake() {
        m_Animator = transform.root.GetComponent<Animator>();
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void Start() {
        m_Landed = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Ground" && !m_Landed){
            m_Landed = true;
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            // m_Animator.SetTrigger("Landing");
            return;
        }
        if(other.CompareTag("Player")){
            GameStateManager.Instance.BombManagerInstance.PickupBomb(BombAmount);
            AudioSource.PlayClipAtPoint(PickupEffect, transform.position);
            Destroy(transform.root.gameObject);
        }
    }

}
