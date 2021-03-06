﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public bool FacingRight = true;

    [Tooltip("移动时角色加速的力大小")]
    public float MoveForce = 365f;

    public float MaxSpeed = 5f;

    public float JumpForce = 1000f;

    [Tooltip("检测角色是否落地")]
    public Transform GroundCheck;

    [Tooltip("血量条")]
    public Transform HealthBar;

    [Tooltip("跳跃音效")]
    public AudioClip[] JumpClips;

    // 获取用户输入
    private Vector2 m_Input;

    private Animator m_Animator;

    // 记录角色是否处于准备跳跃状态
    private bool m_IsReadyJump = false;
    // 记录角色是否处于跳跃状态
    private bool m_IsJumping = false;
    // 记录角色是否处于着地状态
    private bool m_IsGrounded = false;

    private Rigidbody2D m_Rigidbody2D;

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(GroundCheck == null){
            Debug.LogError("请先设置GroundCheck");
        }

        m_Input = new Vector2();
        m_IsReadyJump = false;
        m_IsJumping = false;
        m_IsGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 对场景中的碰撞器投射线段
        m_IsGrounded = Physics2D.Linecast(transform.position, GroundCheck.position, LayerMask.GetMask("Obstacle"));

        m_Animator.SetBool("Grounded", m_IsGrounded);
#if UNITY_STANDALONE
        if(m_IsGrounded && !m_IsJumping && Input.GetButtonDown("Jump")){
            m_IsReadyJump = true;
        }
        m_Input.x = Input.GetAxis("Horizontal");
#elif UNITY_IOS || UNITY_ANDROID
        if(m_IsGrounded && !m_IsJumping && InputManager.GetButtonDown("Jump")){
            m_IsReadyJump = true;
        }
        m_Input.x = InputManager.GetAxis("Horizontal");
#endif
        if(m_IsGrounded && m_IsJumping){
            m_IsJumping = false;
        }
    }

    private void FixedUpdate() {
        float h = m_Input.x;

        m_Animator.SetFloat("Speed", Mathf.Abs(h));

        if(h * m_Rigidbody2D.velocity.x < MaxSpeed){
            m_Rigidbody2D.AddForce(Vector2.right * MoveForce * h);
        }

        if(Mathf.Abs(m_Rigidbody2D.velocity.x) > MaxSpeed){
            // Mathf.Sign 获取数值的正负号
            m_Rigidbody2D.velocity = new Vector2(Mathf.Sign(m_Rigidbody2D.velocity.x) * MaxSpeed, m_Rigidbody2D.velocity.y);
        }

        if(h > 0 && !FacingRight){
            Flip();
        }else if(h < 0 && FacingRight){
            Flip();
        }

        if(m_IsReadyJump){
            Jump();
        }
    }

    private void Flip(){
        FacingRight = !FacingRight;
        transform.localScale = Vector3.Scale(new Vector3(-1, 1, 1), transform.localScale);

        if(HealthBar != null){
            HealthBar.localScale = Vector3.Scale(new Vector3(-1, 1, 1), HealthBar.localScale);
        }else{
            Debug.LogError("请设置血量条");
        }
    }

    private void Jump(){
        m_IsJumping = true;

        m_Rigidbody2D.AddForce(Vector2.up * JumpForce);

        m_Animator.SetTrigger("Jump");

        m_IsReadyJump = false;

        if(JumpClips.Length > 0){
            int i = Random.Range(0, JumpClips.Length);
            AudioSource.PlayClipAtPoint(JumpClips[i], transform.position);
        }
    }
}
