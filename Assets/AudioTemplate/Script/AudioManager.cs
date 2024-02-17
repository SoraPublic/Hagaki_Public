using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSettingElement BGMElement;
    [SerializeField] AudioSettingElement SEElement;
    [SerializeField] BGMSet[] _BGMSet;
    public AudioSource BGM;
    public AudioSource OnSelectUI;
    public AudioSource OnSubmitUI;
    public AudioSource KeyInput;
    [HideInInspector]
    public BGMChangeState State;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        State = BGMChangeState.Idle;
    }

    public bool entryFlag
    {
        get; private set;
    } = false;

    private IEnumerator SetUp()
    {
        yield return new WaitUntil(() => BGMElement != null && SEElement != null);

        SaveData saveData = SaveManager.Load();

        BGMElement.SetVolume(saveData.BGMVolume);
        SEElement.SetVolume(saveData.SEVolume);
        entryFlag = true;
        SetVolume(AudioKind.BGM);
        SetVolume(AudioKind.SE);
    }

    public void SetUpBGM(AudioSettingElement ase)
    {
        BGMElement = ase;
    }
    public void SetUpSE(AudioSettingElement ase)
    {
        SEElement = ase;
        StartCoroutine(SetUp());
    }

    public void SetVolume(AudioKind kind)
    {
        if (!entryFlag)
        {
            return;
        }

        switch (kind)
        {
            case AudioKind.BGM:
                audioMixer.SetFloat("BGM", ConvertVolume(BGMElement.GetVolume));
                SaveManager.SaveBGMVolume(BGMElement.GetVolume);
                break;

            case AudioKind.SE:
                audioMixer.SetFloat("SE", ConvertVolume(SEElement.GetVolume));
                SaveManager.SaveSEVolume(SEElement.GetVolume);
                break;
        }

        Debug.Log("音量調整");
    }

    private int ConvertVolume(int value)
    {
        switch (value)
        {
            case 0:
                return -80;

            case 1: 
                return -40;

            case 2:
                return -20;

            case 3:
                return -10;

            case 4:
                return -5;

            case 5:
                return 5;

            default:
                return -30;
        }
    }

    public void FadeOutChangeBGM(BGMKind kind)
    {
        State = BGMChangeState.Idle;
        BGM.DOFade(0, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ChangeBGM(kind);
            State = BGMChangeState.FadeOut;
        });
    }

    public void FadeInBGM()
    {
        State = BGMChangeState.Idle;
        BGM.Play();
        BGM.DOFade(0.5f, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            State = BGMChangeState.FadeIn;
        });
    }

    public void ChangeBGM(BGMKind kind)
    {
        AudioClip clip;
        try
        {
            clip = Array.Find(_BGMSet, I => I.Kind == kind).AudioClip;
        }
        catch
        {
            return;
        }

        BGM.clip = clip;
    }
/*
    float ConvertVolume2dB(float volume) => Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
    float ConvertVolume2linear(float decibel) => Mathf.Clamp(Mathf.Pow(10, decibel / 20f), 0f, 1f);
*/
}

[System.Serializable]
public class BGMSet
{
    public BGMKind Kind;
    public AudioClip AudioClip;
}

public enum BGMKind
{
    Title,
    Result,
    Easy,
    Normal,
    Hard
}

public enum BGMChangeState
{
    Idle,
    FadeOut,
    FadeIn
}
