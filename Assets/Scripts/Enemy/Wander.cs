using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{

    [Tooltip("是否朝向右边")]
    [SerializeField]
    private bool m_FacingRight = true;

    [Tooltip("移动速度")]
    [SerializeField]
    private float m_MoveSpeed = 2f;

    private Rigidbody2D m_Rigidbody;

    private float m_CurrentMoveSpeed;

    private void Awake() {
        m_Rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(m_FacingRight){
            m_CurrentMoveSpeed = m_MoveSpeed;
        }else{
            m_CurrentMoveSpeed = -m_MoveSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        m_Rigidbody.velocity = new Vector2(m_CurrentMoveSpeed, m_Rigidbody.velocity.y);
    }

    public void Flip(){
        m_CurrentMoveSpeed = -m_CurrentMoveSpeed;
        // Vector3.Scale 将两个向量按分量相乘
        transform.localScale = Vector3.Scale(new Vector3(-1, 1, 1), transform.localScale);
    }
}
