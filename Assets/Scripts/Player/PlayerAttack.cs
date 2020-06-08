using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour
{
    [Tooltip("导弹")]
    public Missile MissilePrefab;
    [Tooltip("导弹发射点")]
    public Transform ShootingPoint;
    [Tooltip("发射导弹的音效")]
    public AudioClip ShootEffect;

    [Tooltip("炸弹Prefab")]
    public Rigidbody2D BombPrefab;

    [Tooltip("炸弹的初始数量")]
    public int InitBombNumber = 4;

    [Tooltip("使用火箭筒抛射炸弹的力")]
    public float ProjectileBombForce = 1000f;

    private int m_CurrentBombNumber;

    private PlayerController m_PlayerCtrl;

    private void Awake() {
        m_PlayerCtrl = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentBombNumber = InitBombNumber;    
    }

    // 发射导弹
    private void Fire(){
        AudioSource.PlayClipAtPoint(ShootEffect, ShootingPoint.position);

        if(ShootingPoint != null){
            Missile instance = Instantiate(MissilePrefab, ShootingPoint.position, Quaternion.identity);
            if(m_PlayerCtrl.FacingRight ^ instance.FacingRight){
                instance.Flip();
            }
        }else{
            Debug.LogError("请设置射击点");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            Fire();
        }

        if(Input.GetButtonDown("Fire2")){
            LayBomb();
        }

        if(Input.GetButtonDown("Fire3")){
            ProjectileBomb();
        }
    }

    private void LayBomb(){
        if(m_CurrentBombNumber <= 0){
            return;
        }
        Instantiate(BombPrefab, this.transform.position, Quaternion.identity);
        m_CurrentBombNumber --;
    }

    private void ProjectileBomb(){
        if(m_CurrentBombNumber <= 0){
            return;
        }
        Rigidbody2D body = Instantiate(BombPrefab, ShootingPoint.position, Quaternion.identity);
        if(m_PlayerCtrl.FacingRight)
            body.AddForce(Vector2.right * ProjectileBombForce);
        else
            body.AddForce(Vector2.left * ProjectileBombForce);

        m_CurrentBombNumber --;
    }
}
