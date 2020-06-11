using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoreManager
{
    [Tooltip("游戏胜利时的目标分数")]
    public int TargetScore = 5000;

    [Tooltip("保存嘲讽音效")]
    public AudioClip[] TauntClips;

    [Tooltip("得分之后播放嘲讽音效的概率")]
    public float TauntProbaility = 50f;

    [Tooltip("嘲讽的间隔")]
    public float TauntDelay = 1f;

    [Tooltip("显示目标分数")]
    public Text TargetScoreText;
    [Tooltip("显示当前的分数")]
    public Text ScoreText;

    private int m_CurrentScore;
    private int m_TauntIndex;
    private float m_LastTauntTime;
    // 当前管理器是否停止工作
    private bool m_Stop;

    private Transform m_Player;


    public void Init(){
        m_CurrentScore = 0;
        m_TauntIndex = 0;
        m_LastTauntTime = Time.time;
        m_Stop = false;
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        TargetScoreText.text = "" + TargetScore;
        ScoreText.text = "" + m_CurrentScore;
    }

    public void Stop(){
        m_Stop = true;
    }

    public void AddScore(int score){
        if(m_Stop) return;
        m_CurrentScore += score;
        ScoreText.text = "" + m_CurrentScore;
        if(m_CurrentScore >= TargetScore){
            GameStateManager.Instance.SetGameResult(true);
        }
        if(m_LastTauntTime <= Time.time + TauntDelay){
            float tauntChance = UnityEngine.Random.Range(0f, 100f);
            if(tauntChance > TauntProbaility){
                m_TauntIndex = TauntRandom();
                AudioSource.PlayClipAtPoint(TauntClips[m_TauntIndex], m_Player.position);
            }
        }
    }

    private int TauntRandom(){
        int i = Random.Range(0, TauntClips.Length);
        if(i == m_TauntIndex)
            return TauntRandom();
        else
            return i;
    }
}
