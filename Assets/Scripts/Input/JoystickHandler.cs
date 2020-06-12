using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum AxisOption {
        Both,
        Horizontal,
        Vertical,
    }

    [Tooltip("虚拟摇杆的最大活动范围")]
    public float Range = 100;
    [Tooltip("是否根据屏幕的尺寸对虚拟摇杆的最大活动范围进行缩放")]
    public bool ScaleRange = true;
    [Tooltip("使用哪个轴")]
    public AxisOption AxisToUse = AxisOption.Both;
    [Tooltip("水平轴的名称")]
    public string HorizontalAxisName = "Horizontal";
    [Tooltip("竖直轴的名称")]
    public string VerticalAxisName = "Vertical";

    Vector3 m_StartPos;
    bool m_UseHorizontalAxis;
    bool m_UseVerticalAxis;

    VirtualAxis m_HorizontalVirtualAxis;
    VirtualAxis m_VerticalVirtualAxis;

    private void Awake(){
        if(ScaleRange){
            CanvasScaler scaler = transform.root.GetComponent<CanvasScaler>();
            float scaleX = Screen.width / scaler.referenceResolution.x;
            float scaleY = Screen.height / scaler.referenceResolution.y;
            if(scaleX > scaleY){
                Range *= scaleY;
            }else{
                Range *= scaleX;
            }
        }
        m_UseHorizontalAxis = (AxisToUse == AxisOption.Both || AxisToUse == AxisOption.Horizontal);
        m_UseVerticalAxis = (AxisToUse == AxisOption.Both || AxisToUse == AxisOption.Vertical);

        if(m_UseHorizontalAxis){
            m_HorizontalVirtualAxis = new VirtualAxis(HorizontalAxisName);
        }

        if(m_UseVerticalAxis){
            m_VerticalVirtualAxis = new VirtualAxis(VerticalAxisName);
        }
    }

    private void OnEnable() {
        if(m_UseHorizontalAxis)
            InputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        if(m_UseVerticalAxis)
            InputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    private void Start() {
        m_StartPos = transform.position;
    }

    private void OnDisable() {
        if(m_UseVerticalAxis)
            InputManager.UnRegisterVirtualAxis(m_VerticalVirtualAxis);
        if(m_UseHorizontalAxis)
            InputManager.UnRegisterVirtualAxis(m_HorizontalVirtualAxis);
    }

    private void UpdateVirtualAxes(Vector3 delta){
        transform.position = new Vector3(
            m_StartPos.x + delta.x,
            m_StartPos.y + delta.y,
            m_StartPos.z + delta.z
        );
        
        delta /= Range;

        if(m_UseHorizontalAxis)
            m_HorizontalVirtualAxis.Update(delta.x);
        if(m_UseVerticalAxis)
            m_VerticalVirtualAxis.Update(delta.y);
    }

    public void OnDrag(PointerEventData data){
        Vector3 newPos = Vector3.zero;
        if(m_UseHorizontalAxis){
            float delta = data.position.x - m_StartPos.x;
            newPos.x = delta;
        }
        if(m_UseVerticalAxis){
            float delta = data.position.y - m_StartPos.y;
            newPos.y = delta;
        }
        if(newPos.magnitude > Range){
            newPos = newPos.normalized * Range;
        }

        UpdateVirtualAxes(newPos);
    }

    public void OnPointerUp(PointerEventData data){
        UpdateVirtualAxes(Vector3.zero);
    }

    public void OnPointerDown(PointerEventData data){
        
    }
}
