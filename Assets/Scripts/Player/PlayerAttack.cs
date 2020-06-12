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

    [Tooltip("使用火箭筒抛射炸弹的力")]
    public float ProjectileBombForce = 1000f;

    private PlayerController m_PlayerCtrl;

    private void Awake() {
        m_PlayerCtrl = GetComponent<PlayerController>();
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
        
#if UNITY_STANDALONE
        if(Input.GetButtonDown("Fire1")){
            Fire();
        }

        if(Input.GetButtonDown("Fire2")){
            LayBomb();
        }

        if(Input.GetButtonDown("Fire3")){
            ProjectileBomb();
        }
#elif UNITY_IOS || UNITY_ANDROID
        if(InputManager.GetButtonDown("Fire1")){
            Fire();
        }
        if(InputManager.GetButtonDown("Fire2")){
            LayBomb();
        }
        if(InputManager.GetButtonDown("Fire3")){
            ProjectileBomb();
        }
#endif
    }

    private void LayBomb(){
        if(GameStateManager.Instance.BombManagerInstance.ReleaseBomb(1) == false)
            return;
        Instantiate(BombPrefab, this.transform.position, Quaternion.identity);
    }

    private void ProjectileBomb(){
        if(GameStateManager.Instance.BombManagerInstance.ReleaseBomb(1) == false)
            return;
        Rigidbody2D body = Instantiate(BombPrefab, ShootingPoint.position, Quaternion.identity);
        if(m_PlayerCtrl.FacingRight)
            body.AddForce(Vector2.right * ProjectileBombForce);
        else
            body.AddForce(Vector2.left * ProjectileBombForce);
    }
}
