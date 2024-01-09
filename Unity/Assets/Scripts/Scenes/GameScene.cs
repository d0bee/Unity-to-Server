using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        // 상위 클래스의 Init 메서드를 먼저 호출합니다.

        SceneType = Define.Scene.Game;
        // SceneType을 Define.Scene.Game으로 설정합니다.
        // 현재 씬의 타입을 나타냅니다.

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        // Managers.Data.StatDict를 사용하여 int형 키와 Data.Stat 형식의 값을 가지는 딕셔너리 dict를 선언합니다.
        // Managers.Data.StatDict는 게임 내 통계 데이터를 관리하는 매니저입니다.

        gameObject.GetOrAddComponent<CursorController>();
        // 현재 게임 오브젝트에 CursorController 컴포넌트를 추가합니다.
        // CursorController는 커서를 제어하는 역할을 합니다.

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        // Managers.Game.Spawn 메서드를 사용하여 Define.WorldObject.Player 타입의 오브젝트를 생성합니다.
        // 생성된 오브젝트의 이름은 "UnityChan"으로 설정됩니다.
        // Managers.Game은 게임 오브젝트를 생성하고 관리하는 매니저입니다.

        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        // Camera.main.gameObject에 CameraController 컴포넌트를 추가합니다.
        // CameraController는 카메라를 제어하는 역할을 합니다.
        // SetPlayer 메서드를 사용하여 플레이어 오브젝트를 설정합니다.

        GameObject go = new GameObject { name = "SpawningPool" };
        // 이름이 "SpawningPool"인 빈 게임 오브젝트 go를 생성합니다.

        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        // go에 SpawningPool 컴포넌트를 추가합니다.
        // SpawningPool은 몬스터를 생성하고 관리하는 역할을 합니다.

        pool.SetKeepMonsterCount(2);
        // SetKeepMonsterCount 메서드를 사용하여 유지할 몬스터의 개수를 2로 설정합니다.
    }

    public override void Clear()
    {
        // 하위 클래스에서 Clear 메서드를 구현해야 합니다.
    }
}