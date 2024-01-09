using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster); // Ground 레이어와 Monster 레이어를 사용하기 위한 비트 마스크를 설정합니다.

    Texture2D _attackIcon; // 공격 커서 아이콘을 저장하는 변수입니다.
    Texture2D _handIcon; // 손 커서 아이콘을 저장하는 변수입니다.

    enum CursorType // 커서의 종류를 나타내는 열거형 변수입니다.
    {
        None, // 커서가 없는 상태
        Attack, // 공격 커서
        Hand, // 손 커서
    }

    CursorType _cursorType = CursorType.None; // 현재 커서의 상태를 저장하는 변수입니다.

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack"); // Resources 폴더에서 공격 커서 아이콘을 로드하여 _attackIcon 변수에 저장합니다.
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand"); // Resources 폴더에서 손 커서 아이콘을 로드하여 _handIcon 변수에 저장합니다.
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼이 클릭된 상태라면 함수를 종료합니다.
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 메인 카메라를 기준으로 마우스 위치에서 레이를 쏘는 Ray를 생성합니다.

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask)) // Raycast를 사용하여 충돌하는 객체를 검출합니다. 최대 거리는 100.0f, 마스크는 _mask 변수를 사용합니다.
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) // 충돌한 객체의 레이어가 Monster 레이어일 경우
            {
                if (_cursorType != CursorType.Attack) // 현재 커서 타입이 Attack이 아닌 경우에만
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto); // 커서를 공격 커서로 변경합니다. 커서의 위치는 아이콘의 너비의 1/5 지점으로 설정합니다.
                    _cursorType = CursorType.Attack; // 커서 타입을 Attack으로 설정합니다.
                }
            }
            else // 충돌한 객체의 레이어가 Monster 레이어가 아닐 경우
            {
                if (_cursorType != CursorType.Hand) // 현재 커서 타입이 Hand가 아닌 경우에만
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto); // 커서를 손 커서로 변경합니다. 커서의 위치는 아이콘의 너비의 1/3 지점으로 설정합니다.
                    _cursorType = CursorType.Hand; // 커서 타입을 Hand로 설정합니다.
                }
            }
        }
    }
}
