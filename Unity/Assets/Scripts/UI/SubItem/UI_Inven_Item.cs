using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    // 게임 오브젝트의 이름을 지정한 열거형입니다.
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    // 아이템 이름을 저장하는 변수입니다.
    string _name;

    // Init 메서드를 재정의합니다.
    public override void Init()
    {
        // Bind 메서드를 호출하여 GameObject 열거형을 바인딩합니다.
        Bind<GameObject>(typeof(GameObjects));

        // ItemNameText 게임 오브젝트의 Text 컴포넌트를 가져와서 _name 변수의 값을 설정합니다.
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;

        // ItemIcon 게임 오브젝트에 이벤트를 바인딩합니다.
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! {_name}"); });
    }

    // 아이템 정보를 설정하는 메서드입니다.
    public void SetInfo(string name)
    {
        _name = name;
    }
}