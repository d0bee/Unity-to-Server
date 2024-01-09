using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    // 게임 오브젝트의 이름을 지정한 열거형입니다.
    enum GameObjects
    {
        HPBar
    }

    // Stat 클래스의 인스턴스를 저장하는 변수입니다.
    Stat _stat;

    // Init 메서드를 재정의합니다.
    public override void Init()
    {
        // Bind 메서드를 호출하여 GameObject 열거형을 바인딩합니다.
        Bind<GameObject>(typeof(GameObjects));

        // 부모 객체의 Stat 컴포넌트를 가져와서 _stat 변수에 할당합니다.
        _stat = transform.parent.GetComponent<Stat>();
    }

    // Update 메서드를 재정의합니다.
    private void Update()
    {
        // 부모 객체의 Transform 컴포넌트를 가져옵니다.
        Transform parent = transform.parent;

        // HP 바의 위치를 설정합니다.
        // HP 바의 위치는 부모 객체의 위치에서 위쪽으로 (부모 객체의 Collider 크기의 y값만큼) 이동한 위치입니다.
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);

        // HP 바의 회전을 설정합니다.
        // HP 바의 회전은 메인 카메라의 회전과 동일합니다.
        transform.rotation = Camera.main.transform.rotation;

        // 현재 체력 비율을 계산합니다.
        float ratio = _stat.Hp / (float)_stat.MaxHp;

        // 체력 바의 비율을 설정하는 메서드를 호출합니다.
        SetHpRatio(ratio);
    }

    // 체력 바의 비율을 설정하는 메서드입니다.
    public void SetHpRatio(float ratio)
    {
        // HPBar 게임 오브젝트의 Slider 컴포넌트의 value 값을 비율로 설정합니다.
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}