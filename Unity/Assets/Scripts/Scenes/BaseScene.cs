using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
    // SceneType은 Define.Scene 열거형으로 정의된 Scene의 타입을 나타내는 속성입니다.
    // protected set으로 설정되어 있어 하위 클래스에서 SceneType 값을 설정할 수 있습니다.

    void Awake()
    {
        Init();
    }
    // Awake 함수는 스크립트가 활성화될 때 호출되는 함수입니다.
    // Init 함수를 호출하여 초기화 작업을 수행합니다.

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        // EventSystem 타입의 객체를 찾습니다.
        // EventSystem은 Unity UI 이벤트 시스템을 관리하는 컴포넌트입니다.

        if (obj == null)
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
            // obj가 null인 경우, Managers.Resource를 사용하여 "UI/EventSystem" 프리팹을 인스턴스화합니다.
            // 인스턴스화된 오브젝트의 이름을 "@EventSystem"으로 설정합니다.
        }
    }

    public abstract void Clear();
    // Clear 함수는 추상 메서드로, 하위 클래스에서 구현해야 합니다.
    // Clear 함수는 해당 씬을 정리하고 초기 상태로 되돌리는 작업을 수행합니다.
}