using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        // 상위 클래스의 Init 메서드를 먼저 호출합니다.

        SceneType = Define.Scene.Login;
        // SceneType을 Define.Scene.Login으로 설정합니다.
        // 현재 씬의 타입을 나타냅니다.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
        // 키보드의 Q 키가 눌리면 Managers.Scene.LoadScene 메서드를 호출하여 Define.Scene.Game 씬을 로드합니다.
        // Managers.Scene은 씬을 관리하는 매니저입니다.
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
        // Clear 메서드가 호출되면 "LoginScene Clear!"라는 로그를 출력합니다.
    }
}