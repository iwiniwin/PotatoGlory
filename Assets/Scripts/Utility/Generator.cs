using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 朝向

public enum Orientation {
    Left,
    Right,
    Random,
    None,
}

public class Generator : MonoBehaviour
{

    [Tooltip("多久之后开始实例化预设对象")]
    public float GenerateDelay = 2f;
    [Tooltip("实例化预设对象的时间间隔")]
    public float GenerateInterval = 3f;
    [Tooltip("预设对象的朝向")]
    public Orientation PrefabOrientation = Orientation.Right;
    [Tooltip("预设对象")]
    public GameObject[] Prefabs;

    private ParticleSystem m_Particle;

    private void Awake() {
        m_Particle = GetComponent<ParticleSystem>();
        if(Prefabs == null || Prefabs.Length == 0){
            Debug.LogError("请至少为Prefabs添加一个预设对象");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Generate", GenerateDelay, GenerateInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate(){
        int index = Random.Range(0, Prefabs.Length);
        // 实例化预设对象
        GameObject prefab = Instantiate(Prefabs[index], transform.position, Quaternion.identity);

        if(m_Particle != null){
            m_Particle.Play();
        }

        if(PrefabOrientation == Orientation.None)
            return;
        
        if(PrefabOrientation == Orientation.Left){
            Wander wander = prefab.GetComponent<Wander>();
            if(wander.FacingRight){
                wander.Flip();
            }
            return;
        }

        if(PrefabOrientation == Orientation.Right){
            Wander wander = prefab.GetComponent<Wander>();
            if(!wander.FacingRight){
                wander.Flip();
            }
            return;
        }

        if(PrefabOrientation == Orientation.Random){
            Wander wander = prefab.GetComponent<Wander>();
            if(Random.value <= 0.5){
                wander.Flip();
            }
            return;
        }
    }
}
