using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // 만약 다른 Start보다 먼저 해야하는게 있다면 Awake로 처리
    
    protected override void init()
    {
        base.init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();

    }

    public override void Clear()
    {
    }

}
