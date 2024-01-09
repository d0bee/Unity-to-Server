using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    // 클릭 이벤트 핸들러를 저장할 액션 변수입니다.
    public Action<PointerEventData> OnClickHandler = null;
    // 드래그 이벤트 핸들러를 저장할 액션 변수입니다.
    public Action<PointerEventData> OnDragHandler = null;

    // IPointerClickHandler 인터페이스의 메서드를 구현합니다.
    // UI 오브젝트가 클릭되었을 때 호출됩니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        // OnClickHandler 액션이 null이 아닌 경우에만 이벤트 핸들러를 호출합니다.
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    // IDragHandler 인터페이스의 메서드를 구현합니다.
    // UI 오브젝트가 드래그되고 있는 동안 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        // OnDragHandler 액션이 null이 아닌 경우에만 이벤트 핸들러를 호출합니다.
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }
}