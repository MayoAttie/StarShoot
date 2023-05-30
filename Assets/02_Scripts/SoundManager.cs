using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum eTYPE_BGM
    {
        Stage_1 =0,
        Stage_2,
        Stage_3
    }
    [SerializeField] AudioClip[] bgmClips;
    public enum eEffect
    {
        Fire,
        Hit,
        Destroy
    }
    [SerializeField] AudioClip[] effectClips;

    static SoundManager _uniqueInsatance;

    AudioSource bgmPlayer;
    List<AudioSource> effPlayers;

    public static SoundManager _instance    //프로시저 기법
    {
        get
        {
            return _uniqueInsatance;
        }
    }

    private void Awake()
    {
        //자기자신을 부름
        _uniqueInsatance = this;

        bgmPlayer = GetComponent<AudioSource>();
        effPlayers = new List<AudioSource>();
    }

    void LateUpdate()
    {
        //생서되는 이펙트 브금을 제거하기 위한 함수
        foreach (AudioSource item in effPlayers)
        {
            if (item.isPlaying == false)
            {
                effPlayers.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    public void PlayBGM(eTYPE_BGM type, float volum = 1.0f, bool isloop = true)
    {
        bgmPlayer.clip = bgmClips[(int)type];
        bgmPlayer.volume = volum;
        bgmPlayer.loop = isloop;

        bgmPlayer.Play();
    }

    public void PlayEffect(eEffect type, float volume = 1.0f, bool loop = false)
    {
        //Game Object 내의 EffectClips 가져옴.
        GameObject go = new GameObject("EffectClips");      //나 자신, 사운드 매니저에게 넣어줌
        //부모에게 달리게 만듦
        go.transform.SetParent(transform);
        //지금 만든 effect sound에 오디오 소스를 추가
        AudioSource AS = go.AddComponent<AudioSource>();
        //소스의 출력과 관련된 함수들.
        AS.clip = effectClips[(int)type];
        AS.volume = volume;
        AS.loop = loop;

        AS.Play();
        //재생이 끝난 오디오는 파괴를 위해, 리스트로 보냄
        effPlayers.Add(AS);

    }

}
