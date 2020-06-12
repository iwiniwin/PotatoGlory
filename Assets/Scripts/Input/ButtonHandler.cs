using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("激活按钮使用的名字")]
    public string Name;
    [Tooltip("按钮松开时的图片")]
    public Sprite NormalImage;
    [Tooltip("按钮被按下时显示的图片")]
    public Sprite ActiveImage;
    [Tooltip("按钮被禁用时显示的图片")]
    public Sprite DisableImage;

    private Image m_CurrentImage;
    private VirtualButton m_Button;

    private void Awake() {
        m_CurrentImage = GetComponent<Image>();
        m_Button = new VirtualButton(Name);
    }

    private void OnEnable() {
        InputManager.RegisterVirtualButton(m_Button);
        if(NormalImage != null)
            m_CurrentImage.sprite = NormalImage;
    }

    private void OnDisable() {
        InputManager.UnRegisterVirtualButton(m_Button);
        if(DisableImage != null)
            m_CurrentImage.sprite = DisableImage;
    }

    public void OnPointerUp(PointerEventData data){
        InputManager.SetButtonUp(m_Button);
        if(NormalImage != null)
            m_CurrentImage.sprite = NormalImage;
    }


    public void OnPointerDown(PointerEventData data){
        InputManager.SetButtonDown(m_Button);
        if(ActiveImage != null)
            m_CurrentImage.sprite = ActiveImage;
    }
}
