using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAxis
{
    public string Name {get; private set;}
    private float m_Value;
    
    public VirtualAxis(string name){
        Name = name;
    }

    public void Update(float value){
        m_Value = value;
    }

    public float GetValue(){
        return m_Value;
    }

    // 返回初始值
    public float GetValueRaw(){
        return m_Value;
    }
}
