using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Remover : MonoBehaviour
{
    [Tooltip("浪花预设")]
    public GameObject SplashPrefab;

    private BoxCollider2D m_Trigger;

    private void Awake() {
        m_Trigger = GetComponent<BoxCollider2D>();
        m_Trigger.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            GameStateManager.Instance.SetGameResult(false);
        }
        Instantiate(SplashPrefab, collision.transform.position, transform.rotation);
        Destroy(collision.gameObject);
    }
}
