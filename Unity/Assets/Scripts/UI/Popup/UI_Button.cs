using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    // Buttons 열거형
    enum Buttons
    {
        PointButton
    }

    // Texts 열거형
    enum Texts
    {
        PointText,
        ScoreText,
    }

    // GameObjects 열거형
    enum GameObjects
    {
        TestObject,
    }

    // Images 열거형
    enum Images
    {
        ItemIcon,
    }

    // 상속받은 Init 메서드를 재정의합니다.
    public override void Init()
    {
        base.Init();

        // 버튼들을 바인딩합니다.
        Bind<Button>(typeof(Buttons));

        // 텍스트들을 바인딩합니다.
        Bind<Text>(typeof(Texts));

        // 게임 오브젝트들을 바인딩합니다.
        Bind<GameObject>(typeof(GameObjects));

        // 이미지들을 바인딩합니다.
        Bind<Image>(typeof(Images));

        // PointButton 버튼을 가져와서 클릭 이벤트에 OnButtonClicked 메서드를 연결합니다.
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        // ItemIcon 이미지를 가져와서 드래그 이벤트에 바인딩합니다.
        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }

    // 점수를 나타내는 변수
    int _score = 0;

    // 버튼 클릭 이벤트 처리 메서드
    public void OnButtonClicked(PointerEventData data)
    {
        // 점수를 증가시키고, ScoreText 텍스트에 점수를 표시합니다.
        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }
}