using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private AudioSource _timerSE;
    public bool IsStart;

    public IEnumerator UpdateTimeAsync(int iniTime)
    {
        AudioManager.instance.FadeInBGM();
        IsStart = false;
        int time = iniTime;
        timerText.text = time.ToString();
        float fontSize = timerText.fontSize;
        timerText.DOComplete();

        yield return new WaitUntil(() => IsStart && AudioManager.instance.State == BGMChangeState.FadeIn);

        GameStateManager.instance.StartGame();
        Tweener tweener = timerText.DOFontSize(fontSize * 0.8f, 1.0f).SetEase(Ease.Linear).OnStepComplete(() =>
        {
            timerText.fontSize = fontSize;
            time -= 1;
            timerText.text = time.ToString();
            if(time < 11)
            {
                _timerSE.Play();
            }
        }).SetLoops(iniTime);

        yield return new WaitUntil(() => !tweener.IsActive());

        GameStateManager.instance.EndGame();
    }
}
