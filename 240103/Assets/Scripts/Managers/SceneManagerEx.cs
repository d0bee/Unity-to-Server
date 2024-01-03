using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    // Unity SceneManager와의 충돌을 막기 위0해 Ex를 붙임.
    // SceneManager가 해야할 일 LoadScene
    public void LoadScene(Define.Scene type)
    {
        // 씬 전환에 따라 매니저를 대리 호출 클리어.
        Managers.Clear();

        // 왜 화면 전환이 일어날 때 CurrentScene.Clear를 해줄까?
        // 자원 관리를 위한 작업.
        CurrentScene.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    // LoadScene의 SceneManager.LoadScene의 매개변수를 얻기 위해 enum.getname을 return해주는 함ㅅ
    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
