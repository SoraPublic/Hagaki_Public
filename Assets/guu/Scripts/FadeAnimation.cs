using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField]
    private Color _firstColor = Color.black;
    [SerializeField]
    private float _animationTime = 1f;
    private Image _myImage;
    private void Awake()
    {
        _myImage = GetComponent<Image>();
        _myImage.color = _firstColor;
    }

    public async UniTask DoFadeOutAsync()
    {
        await _myImage.DOFade(0, _animationTime).SetEase(Ease.InExpo);
    }

    public async UniTask DoFadeInAsync()
    {
        await _myImage.DOFade(1, _animationTime).SetEase(Ease.InExpo);
    }
}
