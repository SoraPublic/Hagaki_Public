using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TitleFadeAnimation : FadeAnimation
{
    private float fadeValue = 1f;
    [SerializeField]
    private float decreaseFadeValue = 0.05f;
    [SerializeField]
    private float _goFadeAnimationTime = 1f;
    private Image _image;
    public async UniTask GoFadeAsync()
    {
        fadeValue-=decreaseFadeValue;
        _image.DOComplete();
        await _image.DOFade(fadeValue, _goFadeAnimationTime);
    }
}
