using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BombManager 
{
    [Tooltip("炸弹的初始数量")]
    public int InitBombNumber = 4;
    [Tooltip("炸弹UI")]
    public Image BombUI;

    [Tooltip("显示炸弹的数量")]
    public Text BombNumberText;

    private int m_CurrentBombNumber;

    private bool m_Stop;

    public void Init(){
        m_CurrentBombNumber = InitBombNumber;
        m_Stop = false;
        UpdateUI();
    }

    public bool ReleaseBomb(int bombNum){
        if(m_Stop) return false;
        int temp = m_CurrentBombNumber - bombNum;
        if(temp >= 0){
            m_CurrentBombNumber = temp;
            UpdateUI();
            return true;
        }else{
            return false;
        }
    }

    public void PickupBomb(int bombNum){
        if(m_Stop) return;
        m_CurrentBombNumber += bombNum;
        UpdateUI();
    }

    public void UpdateUI(){
        BombNumberText.text = "" + m_CurrentBombNumber;
        if(m_CurrentBombNumber <= 0){
            BombUI.color = new Color(255, 0, 0, BombUI.color.a / 2);
        }else{
            BombUI.color = new Color(255, 255, 255, BombUI.color.a);
        }
    }

    public void Stop(){
        m_Stop = true;
    }
}
