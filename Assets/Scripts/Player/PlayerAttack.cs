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

    private PlayerController m_PlayerCtrl;

    private void Awake() {
        m_PlayerCtrl = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

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
    }
}
