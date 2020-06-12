using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualButton
{
    public string Name {get; private set;}

    // 按下按钮的帧数
    private int m_LastPressedFrame = -5;
    // 释放按钮的帧数
    private int m_ReleasedFrame = -5;
    // 按钮是否处于被按压的状态
    private bool m_Pressed;

    public VirtualButton(string name){
        Name = name;
    }

    public void Pressed(){
        if(m_Pressed) return;
        m_Pressed = true;
        m_LastPressedFrame = Time.frameCount;
    }

    public void Released() {
        m_Pressed = false;
        m_ReleasedFrame = Time.frameCount;
    }

    // 返回当前是否按下按钮
    public bool GetButton() {
        return m_Pressed;
    }

    // 判断是否刚刚按下按钮
    public bool GetButtonDown(){
        return m_LastPressedFrame == (Time.frameCount - 1);
    }

    // 判断是否刚刚松开按钮
    public bool GetButtonUp(){
        return m_ReleasedFrame == (Time.frameCount - 1);
    }

}
