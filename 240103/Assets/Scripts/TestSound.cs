using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Unity Component에 넣어 줄 Audio File, public 선언
    public AudioClip AudioClip;
    private void OnTriggerEnter(Collider other)
    {
        // AudioSource audio = GetComponent<AudioSource>();
        // audio.PlayOneShot(AudioClip);

        Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Bgm);
        Managers.Sound.Play("UnityChan/univ0002", Define.Sound.Bgm);
    }


}
