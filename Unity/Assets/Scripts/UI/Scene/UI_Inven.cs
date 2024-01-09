using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    // 게임 오브젝트의 이름을 지정한 열거형입니다.
    enum GameObjects
    {
        GridPanel
    }

    // Init 메서드를 재정의합니다.
    public override void Init()
    {
        base.Init();

        // Bind 메서드를 호출하여 GameObject 열거형을 바인딩합니다.
        Bind<GameObject>(typeof(GameObjects));

        // GridPanel 게임 오브젝트를 가져옵니다.
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        // GridPanel의 자식 오브젝트들을 모두 제거합니다.
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        // 실제 인벤토리 정보를 참고하여 아이템을 생성합니다.
        for (int i = 0; i < 8; i++)
        {
            // UI_Inven_Item 클래스를 사용하여 아이템 게임 오브젝트를 생성합니다.
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;

            // 생성된 아이템에 UI_Inven_Item 컴포넌트를 추가합니다.
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();

            // invenItem의 SetInfo 메서드를 호출하여 아이템 정보를 설정합니다.
            invenItem.SetInfo($"집행검{i}번");
        }
    }
}