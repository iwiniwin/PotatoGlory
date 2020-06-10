using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("水平方向上最大偏移量")]
    public float HorizontalMargin = 2f;

    [Tooltip("竖直方向上最大偏移量")]
    public float VerticalMargin = 2f;

    [Tooltip("水平方向上跟随角色的速度")]
    public float HorizontalFollowSpeed = 2f;

    [Tooltip("竖直方向上跟随角色的速度")]
    public float VerticalFollowSpeed = 2f;

    [Tooltip("摄像机可移动的范围")]
    public BoxCollider2D Region;

    private Vector2 m_HorizontalRegion;
    private Vector2 m_VerticalRegion;

    private Transform m_Player;


    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        if(m_Player == null){
            Debug.LogError("请添加tag为Player的GameObject");
        }

        

        Camera camera = GetComponent<Camera>();
        Vector3 cornerPos = camera.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(transform.position.z)));

        // https://www.rainbowcyan.com/tutorial/potato-glory/PotatoGloryTutorial-10/
        // 计算摄像机视口的宽高
        float cameraWidth = 2 * (cornerPos.x - transform.position.x);
        float cameraHeight = 2 * (cornerPos.y - transform.position.y);

        // Debug.Log(cornerPos);
        // Debug.Log(cameraHeight); // 22
        // Debug.Log(cameraWidth); // 1920/1080 * 22 = 39.1111

        // 计算Box Collider2D中心点的世界坐标
        // offset属性表示Collider2D中心点和Transform组件中心点的偏移量
        Vector2 regionPosition = new Vector2(
            Region.transform.position.x + Region.offset.x,
            Region.transform.position.y + Region.offset.y
        );

        float halfDeltaWidth = (Region.size.x - cameraWidth) / 2;
        float halfDeltaHeight = (Region.size.y - cameraHeight) / 2;
        if(halfDeltaWidth < 0){

        }
        if(halfDeltaWidth < 0){

        }

        m_HorizontalRegion = new Vector2(regionPosition.x - halfDeltaWidth, regionPosition.x + halfDeltaWidth);
        m_VerticalRegion = new Vector2(regionPosition.y - halfDeltaHeight, regionPosition.y + halfDeltaHeight);
    }

    private void LateUpdate() {
        // 如果角色被销毁，不再进行跟随
        if(m_Player != null){
            TrackPlayer();
        }
    }

    private void TrackPlayer(){
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if(CheckHorizontalMargin()){
            targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, HorizontalMargin * Time.deltaTime);
        }

        if(CheckVerticalMargin()){
            targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, VerticalMargin * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, m_HorizontalRegion.x, m_HorizontalRegion.y);
        targetY = Mathf.Clamp(targetY, m_VerticalRegion.x, m_VerticalRegion.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    private bool CheckHorizontalMargin(){
        return Mathf.Abs(transform.position.x - m_Player.position.x) > HorizontalMargin;
    }

    private bool CheckVerticalMargin(){
        return Mathf.Abs(transform.position.y - m_Player.position.y) > VerticalMargin;
    }
}
