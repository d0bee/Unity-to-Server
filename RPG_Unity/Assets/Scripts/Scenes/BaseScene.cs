using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknwon;

    void Awake()
    {
        init();
    }

    protected virtual void init()
    { 
        // 프리팹화한 EventSystem을 가져오기 위한 작업
        // 만약 obj가 없다면 prefab의 위치에서 가져온다.
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null ) 
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "EventSystem";
        }

    }

    public abstract void Clear();
}
