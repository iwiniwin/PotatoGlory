using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {
    Init,
    Start,
    Running,
    End,
}

[RequireComponent(typeof(AudioSource))]
public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_Instance = null;

    public static GameStateManager Instance {
        get {
            if(m_Instance == null){
                m_Instance = FindObjectOfType(typeof(GameStateManager)) as GameStateManager;
                // 场景中没有添加了GameStateManager脚本的GameObject就自动创建一个
                if(m_Instance == null){
                    GameObject obj = new GameObject("GameStateManager");
                    m_Instance = obj.AddComponent<GameStateManager>();
                }
            }
            return m_Instance;
        }
    }

    [Tooltip("游戏运行时的背景音乐")]
    public AudioClip BackgroundMusic;

    [Tooltip("游戏胜利时的音效")]
    public AudioClip GameWinClip;

    [Tooltip("游戏失败时的音效")]
    public AudioClip GameLoseClip;

    [Tooltip("ScoreManager的实例")]
    public ScoreManager ScoreManagerInstance = new ScoreManager();

    [Tooltip("BombManager的实例")]
    public BombManager BombManagerInstance = new BombManager();

    [Tooltip("游戏暂停界面")]
    public GameObject PausePanel;
    [Tooltip("游戏结束界面")]
    public GameObject GameResultPanel;
    [Tooltip("游戏结束")]
    public Text GameResultText;

    [Tooltip("场景中所有Generator的父物体")]
    public GameObject Generator;

    private GameState m_CurrentState;
    private bool m_IsPaused;
    // 游戏结果，true胜利，false失败
    private bool m_GameResult;

    private AudioSource m_AudioSource;

    private void Awake() {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_IsPaused = false;
        m_CurrentState = GameState.Init;
        StartCoroutine(GameMainLoop());
    }

    private IEnumerator GameMainLoop(){
        GameInit();

        while(m_CurrentState == GameState.Init)
            yield return null;
        
        GameStart();

        while(m_CurrentState == GameState.Running){
            GameRunning();
            yield return null;
        }

        GameEnd();
    }

    private void GameInit(){
        Debug.Log("Game Init");
        ScoreManagerInstance.Init();
        BombManagerInstance.Init();

        // 确保面板不显示
        PausePanel.SetActive(false);
        GameResultPanel.SetActive(false);

        m_CurrentState = GameState.Start;
    }

    private void GameStart(){
        Debug.Log("Game Start");
        if(BackgroundMusic != null){
            m_AudioSource.clip = BackgroundMusic;
            m_AudioSource.loop = true;
            m_AudioSource.Play();
        }
        m_CurrentState = GameState.Running;
    }

    public void GameResume(){
        m_AudioSource.UnPause();
        Time.timeScale = 1f;
        m_IsPaused = false;
        PausePanel.SetActive(false);
    }

    public void GamePause(){
        m_AudioSource.Pause();
        // 暂停游戏
        Time.timeScale = 0f;
        m_IsPaused = true;
        PausePanel.SetActive(true);
    }

    private void GameRunning(){
// 为了测试，使用宏来隔离平台
#if UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.P)){
            if(m_IsPaused)
                GameResume();
            else
                GamePause();
        }
#endif
        // if(Input.GetKeyDown(KeyCode.E)){
        //     SetGameResult(false);
        // }
        // if(Input.GetKeyDown(KeyCode.Q)){
        //     SetGameResult(true);
        // }
    }

    private void GameEnd(){
        Debug.Log("Game End");
        m_AudioSource.Stop();
        m_AudioSource.loop = false;

        ScoreManagerInstance.Stop();
        BombManagerInstance.Stop();

        float delay = 0f;
        if(m_GameResult){
            if(GameWinClip != null){
                AudioSource.PlayClipAtPoint(GameWinClip, this.transform.position);
                delay = GameWinClip.length;
            }
        }else{
            if(GameLoseClip != null){
                AudioSource.PlayClipAtPoint(GameLoseClip, this.transform.position);
                delay = GameLoseClip.length;
            }
        }
        GameResultPanel.SetActive(true);
        // 播放完音效之后，删除场景中所有的Generator
        Destroy(Generator, delay);
    }

    public void SetGameResult(bool result){
        m_GameResult = result;
        m_CurrentState = GameState.End;
    }

    public void Restart(){
        // 重新加载当前的游戏场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back(){

    }
}
