using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wander))]
public class Enemy : MonoBehaviour
{

    [Tooltip("障碍物检测点")]
    public Transform m_FrontCheck;

    private Wander m_Wander;
    private LayerMask m_LayerMask;

    private void Awake() {
        m_Wander = GetComponent<Wander>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LayerMask = LayerMask.GetMask("Obstacle");   
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(m_FrontCheck.position, m_LayerMask);
        if(frontHits.Length > 0){
            m_Wander.Flip();
        }
    }
}
