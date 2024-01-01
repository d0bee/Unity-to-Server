using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void init()
    {
        base.init();

        SceneType = Define.Scene.Login;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Unity의 SceneManager.LoadScene을 이용하여 화면 전환을 사용해도 되지만 직접 작성한 SceneManagerEx를 사용하여 전환을 시도
            // BuildSetting이 설정되어 있어야 함.
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }

    public override void Clear()
    {
        Debug.Log("LoginScene Clear!");
    }

}
