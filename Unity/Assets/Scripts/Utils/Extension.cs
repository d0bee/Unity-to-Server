using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    // GetOrAddComponent<T>는 GameObject에 T 타입의 컴포넌트를 가져오거나 없을 경우 추가하는 메서드입니다.
    // T : UnityEngine.Component로 제한되어 있습니다.
    // 해당 GameObject에 T 타입의 컴포넌트가 이미 존재하면 그 컴포넌트를 반환하고,
    // 존재하지 않을 경우에는 T 타입의 컴포넌트를 추가하여 반환합니다.
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    // BindEvent는 GameObject에 UI 이벤트를 바인딩하는 메서드입니다.
    // action은 PointerEventData를 매개변수로 받는 액션(메서드)입니다.
    // type은 Define.UIEvent 타입으로 기본값은 Define.UIEvent.Click입니다.
    // UI_Base.BindEvent를 호출하여 GameObject에 UI 이벤트를 바인딩합니다.
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    // IsValid는 GameObject가 유효한지 여부를 확인하는 메서드입니다.
    // 해당 GameObject가 null이 아니고 활성화되어 있으면 true를 반환하고,
    // 그렇지 않으면 false를 반환합니다.
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}