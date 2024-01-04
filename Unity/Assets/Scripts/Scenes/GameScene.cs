using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // Coroutine 상태저장, 일정 시간 뒤 재시작 등에서 쓰인다.
    // yield return 일시 반환, yield break 영구 반환
    // Update문 외에도 특정 반복이 필요할 때 주로 사용된다.
    // Update/Invoke와 다르게 매개변수 전달 가능
    // Coroutine 잘못 사용시 템복사 등의 문제 가능
    // 왜? 코루틴 작업 중에 DB저장과 아이템 이동 사이에
    // 어떤 이유로 yield return이 있어야 할 경우에 발생하는 DB와 로컬 괴리감으로 인해서 나타날 수 있다.
    // + 일정 시간 뒤에 발동되는 캐스팅 스킬 등의 작업때 사용된다.
   
    protected override void init()
    {
        base.init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
    }

    public override void Clear()
    {
    }

}
