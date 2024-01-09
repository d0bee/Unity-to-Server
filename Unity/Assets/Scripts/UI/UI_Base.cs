using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    // UI 오브젝트를 저장할 Dictionary 변수입니다.
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 파생 클래스에서 구현할 추상 메서드입니다.
    // 이 메서드를 사용하여 UI 초기화 작업을 수행합니다.
    public abstract void Init();

    private void Start()
    {
        // Init() 메서드를 호출하여 UI 초기화 작업을 수행합니다.
        Init();
    }

    // UI 오브젝트를 바인딩하는 메서드입니다.
    // T는 UnityEngine.Object를 상속하는 타입을 의미합니다.
    // type은 바인딩할 오브젝트의 타입을 의미합니다.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // type에서 열거형 상수의 이름들을 가져옵니다.
        string[] names = Enum.GetNames(type);
        // 이름들의 개수만큼 UnityEngine.Object 배열을 생성합니다.
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        // _objects 딕셔너리에 T 타입과 배열을 저장합니다.
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            // T 타입이 GameObject인 경우 Util.FindChild 메서드를 사용하여 자식 오브젝트를 찾아서 저장합니다.
            // T 타입이 GameObject가 아닌 경우 Util.FindChild<T> 메서드를 사용하여 자식 오브젝트를 찾아서 저장합니다.
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            // 오브젝트를 찾지 못한 경우에는 실패 메시지를 출력합니다.
            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    // 저장된 UI 오브젝트를 가져오는 메서드입니다.
    // idx는 가져올 오브젝트의 인덱스를 의미합니다.
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // Get<T> 메서드로 GameObject를 가져옵니다.
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    // Get<T> 메서드로 Text를 가져옵니다.
    protected Text GetText(int idx) { return Get<Text>(idx); }
    // Get<T> 메서드로 Button을 가져옵니다.
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    // Get<T> 메서드로 Image를 가져옵니다.
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // UI 오브젝트에 이벤트를 바인딩하는 메서드입니다.
    // go는 이벤트를 바인딩할 오브젝트를 의미하고,
    // action은 이벤트 핸들러로 등록할 액션 메서드를 의미합니다.
    // type은 UI 이벤트의 종류를 나타냅니다. 기본값은 Define.UIEvent.Click입니다.
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // UI_EventHandler 컴포넌트를 가져오거나 추가합니다.
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        // type에 따라서 이벤트 핸들러를 등록합니다.
        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}