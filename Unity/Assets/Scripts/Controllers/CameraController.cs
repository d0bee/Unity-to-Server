using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView; // 카메라 모드를 나타내는 열거형 변수입니다.

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); // 카메라의 위치를 플레이어로부터 얼마나 떨어뜨릴지를 나타내는 벡터 변수입니다.

    [SerializeField]
    GameObject _player = null; // 카메라가 따라다닐 플레이어 오브젝트를 저장하는 변수입니다.

    public void SetPlayer(GameObject player) { _player = player; } // 플레이어 오브젝트를 설정하는 메서드입니다.

    void Start()
    {

    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView) // 카메라 모드가 QuarterView 모드일 경우에만 동작합니다.
        {
            if (_player.IsValid() == false) // 플레이어 오브젝트가 유효하지 않을 경우 함수를 종료합니다.
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f; // 플레이어와 충돌하는 물체가 있을 경우, 카메라의 거리를 충돌 지점과 플레이어 사이의 거리로 설정합니다.
                transform.position = _player.transform.position + _delta.normalized * dist; // 카메라의 위치를 플레이어 위치와 벡터 _delta의 방향으로 dist만큼 떨어진 곳으로 설정합니다.
            }
            else
            {
                transform.position = _player.transform.position + _delta; // 충돌하는 물체가 없을 경우, 카메라의 위치를 플레이어 위치와 벡터 _delta만큼 떨어진 곳으로 설정합니다.
                transform.LookAt(_player.transform); // 카메라가 플레이어를 바라보도록 설정합니다.
            }
        }
    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView; // 카메라 모드를 QuarterView 모드로 설정합니다.
        _delta = delta; // 카메라의 위치 설정 벡터인 _delta를 전달받은 delta로 설정합니다.
    }
}