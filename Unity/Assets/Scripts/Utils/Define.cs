using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    // WorldObject 열거형은 게임 세계에서의 오브젝트 유형을 정의합니다.
    // Unknown: 알 수 없는 오브젝트
    // Player: 플레이어 오브젝트
    // Monster: 몬스터 오브젝트
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    // State 열거형은 게임 오브젝트의 상태를 정의합니다.
    // Die: 사망 상태
    // Moving: 이동 중인 상태
    // Idle: 정지 상태
    // Skill: 스킬 사용 중인 상태
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    // Layer 열거형은 게임 오브젝트의 레이어를 정의합니다.
    // Monster: 몬스터 레이어 (레이어 번호 8)
    // Ground: 땅 레이어 (레이어 번호 9)
    // Block: 블록 레이어 (레이어 번호 10)
    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    // Scene 열거형은 게임 씬(Scene)을 정의합니다.
    // Unknown: 알 수 없는 씬
    // Login: 로그인 씬
    // Lobby: 로비 씬
    // Game: 게임 씬
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    // Sound 열거형은 사운드 유형을 정의합니다.
    // Bgm: 배경음
    // Effect: 효과음
    // MaxCount: 사운드 개수 최대값
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    // UIEvent 열거형은 UI 이벤트를 정의합니다.
    // Click: 클릭 이벤트
    // Drag: 드래그 이벤트
    public enum UIEvent
    {
        Click,
        Drag,
    }

    // MouseEvent 열거형은 마우스 이벤트를 정의합니다.
    // Press: 마우스 버튼을 누르는 이벤트
    // PointerDown: 마우스 버튼을 누르고 있는 동안의 이벤트
    // PointerUp: 마우스 버튼을 뗀 이벤트
    // Click: 클릭 이벤트
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    // CameraMode 열거형은 카메라 모드를 정의합니다.
    // QuarterView: 분할 화면 모드
    public enum CameraMode
    {
        QuarterView,
    }
}