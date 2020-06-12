using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static Dictionary<string, VirtualAxis> m_VirtualAxes;
    private static Dictionary<string, VirtualButton> m_VirtualButtons;

    static InputManager(){
        m_VirtualAxes = new Dictionary<string, VirtualAxis>();
        m_VirtualButtons = new Dictionary<string, VirtualButton>();
    }

    public static bool AxisExists(string name){
        return m_VirtualAxes.ContainsKey(name);
    }

    public static bool ButtonExists(string name){
        return m_VirtualButtons.ContainsKey(name);
    }

    public static void RegisterVirtualAxis(VirtualAxis axis){
        if(m_VirtualAxes.ContainsKey(axis.Name)){
            Debug.LogError("repeat register : " + axis.Name);
        }else{
            m_VirtualAxes.Add(axis.Name, axis);
        }
    }

    public static void RegisterVirtualButton(VirtualButton button){
        if(m_VirtualButtons.ContainsKey(button.Name)){
            Debug.LogError("repeat register : " + button.Name);
        }else{
            m_VirtualButtons.Add(button.Name, button);
        }
    }

    public static void UnRegisterVirtualAxis(VirtualAxis axis){
        if(m_VirtualAxes.ContainsKey(axis.Name)){
            m_VirtualAxes.Remove(axis.Name);
        }
    }

    public static void UnRegisterVirtualButton(VirtualButton button){
        if(m_VirtualButtons.ContainsKey(button.Name)){
            m_VirtualButtons.Remove(button.Name);
        }
    }

    public static void SetButtonDown(VirtualButton button){
        if(InputManager.ButtonExists(button.Name)){
            button.Pressed();
        }else{
            Debug.LogError("there is not a button named " + button.Name);
        }
    }

    public static void SetButtonUp(VirtualButton button){
        if(InputManager.ButtonExists(button.Name)){
            button.Released();
        }else{
            Debug.LogError("there is not a button named " + button.Name);
        }
    }

    public static float GetAxis(string name){
        if(m_VirtualAxes.ContainsKey(name)){
            return m_VirtualAxes[name].GetValue();
        }else{
            Debug.LogError("there is not a axis named " + name);
            return 0f;
        }
    }

    public static bool GetButton(string name){
        if(m_VirtualButtons.ContainsKey(name)){
            return m_VirtualButtons[name].GetButton();
        }else{
            Debug.LogError("there is not a button named " + name);
            return false;
        }
    }

    public static bool GetButtonDown(string name){
        if(m_VirtualButtons.ContainsKey(name)){
            return m_VirtualButtons[name].GetButtonDown();
        }else{
            Debug.LogError("there is not a button named " + name);
            return false;
        }
    }

    public static bool GetButtonUp(string name){
        if(m_VirtualButtons.ContainsKey(name)){
            return m_VirtualButtons[name].GetButtonUp();
        }else{
            Debug.LogError("there is not a button named " + name);
            return false;
        }
    }
}
