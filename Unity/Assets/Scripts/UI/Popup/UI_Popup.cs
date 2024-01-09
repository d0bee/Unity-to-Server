csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // Init 메서드를 재정의합니다.
    public override void Init()
    {
        // Managers.UI의 SetCanvas 메서드를 호출하여 해당 게임 오브젝트의 캔버스를 활성화합니다.
        Managers.UI.SetCanvas(gameObject, true);
    }

    // 팝업 UI를 닫는 메서드
    public virtual void ClosePopupUI()
    {
        // Managers.UI의 ClosePopupUI 메서드를 호출하여 현재의 팝업 UI를 닫습니다.
        Managers.UI.ClosePopupUI(this);
    }
}