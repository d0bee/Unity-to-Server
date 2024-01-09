using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    // Init 메서드를 재정의합니다.
    public override void Init()
    {
        // Managers.UI.SetCanvas 메서드를 호출하여 현재 게임 오브젝트의 캔버스를 설정합니다.
        // 두 번째 인자인 false는 캔버스를 비활성화하는 것을 의미합니다.
        Managers.UI.SetCanvas(gameObject, false);
    }
}