csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // Managers 클래스의 유일한 인스턴스를 저장하는 변수입니다.
    static Managers Instance { get { Init(); return s_instance; } } // Managers 인스턴스를 가져옵니다.

    #region Contents
    GameManagerEx _game = new GameManagerEx(); // 게임 매니저를 생성합니다.

    public static GameManagerEx Game { get { return Instance._game; } } // 게임 매니저에 접근할 수 있는 속성입니다.
    #endregion

    #region Core
    DataManager _data = new DataManager(); // 데이터 매니저를 생성합니다.
    InputManager _input = new InputManager(); // 입력 매니저를 생성합니다.
    PoolManager _pool = new PoolManager(); // 풀 매니저를 생성합니다.
    ResourceManager _resource = new ResourceManager(); // 리소스 매니저를 생성합니다.
    SceneManagerEx _scene = new SceneManagerEx(); // 씬 매니저를 생성합니다.
    SoundManager _sound = new SoundManager(); // 사운드 매니저를 생성합니다.
    UIManager _ui = new UIManager(); // UI 매니저를 생성합니다.

    public static DataManager Data { get { return Instance._data; } } // 데이터 매니저에 접근할 수 있는 속성입니다.
    public static InputManager Input { get { return Instance._input; } } // 입력 매니저에 접근할 수 있는 속성입니다.
    public static PoolManager Pool { get { return Instance._pool; } } // 풀 매니저에 접근할 수 있는 속성입니다.
    public static ResourceManager Resource { get { return Instance._resource; } } // 리소스 매니저에 접근할 수 있는 속성입니다.
    public static SceneManagerEx Scene { get { return Instance._scene; } } // 씬 매니저에 접근할 수 있는 속성입니다.
    public static SoundManager Sound { get { return Instance._sound; } } // 사운드 매니저에 접근할 수 있는 속성입니다.
    public static UIManager UI { get { return Instance._ui; } } // UI 매니저에 접근할 수 있는 속성입니다.
    #endregion

    void Start()
    {
        Init(); // Managers 클래스를 초기화합니다.
    }

    void Update()
    {
        _input.OnUpdate(); // 입력 매니저의 업데이트 함수를 호출합니다.
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers"); // "@Managers"라는 이름의 게임 오브젝트를 찾습니다.
            if (go == null)
            {
                go = new GameObject { name = "@Managers" }; // "@Managers"라는 이름의 게임 오브젝트를 생성합니다.
                go.AddComponent<Managers>(); // Managers 컴포넌트를 추가합니다.
            }

            DontDestroyOnLoad(go); // 씬이 변경되어도 게임 오브젝트가 파괴되지 않도록 설정합니다.
            s_instance = go.GetComponent<Managers>(); // Managers 컴포넌트를 가져옵니다.

            s_instance._data.Init(); // 데이터 매니저를 초기화합니다.
            s_instance._pool.Init(); // 풀 매니저를 초기화합니다.
            s_instance._sound.Init(); // 사운드 매니저를 초기화합니다.
        }
    }

    public static void Clear()
    {
        Input.Clear(); // 입력 매니저를 초기화합니다.
        Sound.Clear(); // 사운드 매니저를 초기화합니다.
        Scene.Clear(); // 씬 매니저를 초기화합니다.
        UI.Clear(); // UI 매니저를 초기화합니다.
        Pool.Clear(); // 풀 매니저를 초기화합니다.
    }
}