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

    [Tooltip("是否使用随机时间间隔来实例化预设对象")]
    public bool RandomGenerateInterval = false;

    [Tooltip("实例化预设对象的最短时间间隔")]
    public float MinGenerateInterval;
    [Tooltip("实例化预设对象的最长时间间隔")]
    public float MaxGenerateInterval;

    [Tooltip("是否在随机的X坐标上实例化预设对象")]
    public bool RandomGeneratePositionX = false;
    [Tooltip("实例化预设对象时的最小X坐标")]
    public float MinGeneratePositionX;
    [Tooltip("实例化预设对象时的最大X坐标")]
    public float MaxGeneratePositionX;

    [Tooltip("是否在随机的Y坐标上实例化预设对象")]
    public bool RandomGeneratePositionY = false;
    [Tooltip("实例化预设对象时的最小Y坐标")]
    public float MinGeneratePositionY;
    [Tooltip("实例化预设对象时的最大Y坐标")]
    public float MaxGeneratePositionY;

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
        // InvokeRepeating("Generate", GenerateDelay, GenerateInterval);
        StartCoroutine(RandomGenerate());
    }

    private IEnumerator RandomGenerate(){
        yield return new WaitForSeconds(GenerateDelay);
        while(true){
            float interval = GenerateInterval;
            if(RandomGenerateInterval){
                interval = Random.Range(MinGenerateInterval, MaxGenerateInterval);
            }
            yield return new WaitForSeconds(interval);
            Generate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate(){
        int index = Random.Range(0, Prefabs.Length);

        float x = transform.position.x;
        if(RandomGeneratePositionX){
            x = Random.Range(MinGeneratePositionX, MaxGeneratePositionY);
        }
        float y = transform.position.y;
        if(RandomGeneratePositionX){
            y = Random.Range(MinGeneratePositionY, MaxGeneratePositionY);
        }

        transform.position = new Vector3(x, y, transform.position.z);

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
