using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundManager
{
    // AudioSource 리스트, Define.Sound.MaxCount를 설정해놨기 때문에 쉽게 가능.
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    // 매번 Audio를 가져오는 작업 자체가 어느 정도 CPU 자원을 낭비함.
    // Dictionary를 사용하여 일종의 캐시를 구현.
    // 다만 종종 Clear를 해주지 않는다면, 이제는 램 낭비가 발생할 수도 있음.
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    // MP3 Player -> Audio Source -> Unity Component
    // MP3 음원 -> Audio Clip
    // 관객(귀) -> Audio Listener -> Unity Camera

    public void init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            // 씬이 전환되어도 제거되지 않음.
            // 항상 DontDestroy? 자원 낭비가 발생할 수도 있음.
            Object.DontDestroyOnLoad(root);

            // Define에 정의된 Sound 이름 목록을 soundNames 배열에 저장.
            string[] soundNames =  System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length-1; i++) 
            {
                // 저장된 soundNames의 이름을 GameObject 변수 go에 저장.
                GameObject go = new GameObject { name= soundNames[i] };

                // 빈 리스트인 Unity.AudioSource 객체에 go 정보를 기준으로 작성 및 AddComponent
                _audioSources[i] = go.AddComponent<AudioSource>();

                // 새로 생성된 Component의 위치를 전환
                go.transform.parent = root.transform;
            }

            // 상시 재생 Bgm의 loop true
            _audioSources[(int)Define.Sound.Bgm].loop = false;
        }
    }

    // 씬 전환시 Clear를 통해 사운드 자원을 일시적으로 날리기.
    public void Claer()
    {
        foreach (AudioSource audioSource in _audioSources) 
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    // 음악 재생용 함수 Play
    // 사운드의 종류, type에 따라 재생 방식이 달라짐.
    // ex) bgm, effect
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        // 버전 관리를 위해서 두 종류의 함수라도 base를 만들어두어 관리를 함.
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch );
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (type == Define.Sound.Bgm) // 상시 실행, Bgm일 경우
        {
            if (audioClip == null)
                return;

            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            // 현재 진행 중인 Bgm이 있다면 Stop
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // 상시 실행, Bgm이 아닐 경우
        {
            // Bgm이 아니라면 일시적인 Effect 효과음일 것.
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            // 앞에서 저장한 audioClip을 OneShot.
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        // 만약을 위한 path 경로 지정
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm) // 상시 실행, Bgm일 경우
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else  // 상시 실행, Bgm이 아닐 경우
        {
            // 만약 Dictionary _audioClips, 캐시에 찾으려는 audioClip이 있다면 바로 캐싱
            // 없다면 찾은 뒤에 _audioClips에 Dictionary 키, 밸류 추가
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
