using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhraseAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animationTime = 0.1f;
    [SerializeField]
    private float _showPhraseTime = 1.5f;
    private void Awake()
    {
        this.transform.localScale = Vector3.zero;
    }

    public async UniTask ShowPhraseAsync()
    {
        await this.transform.DOScale(Vector3.one, _animationTime).SetEase(Ease.OutCirc);
        HidePhraseAsync().Forget();
    }

    private async UniTask HidePhraseAsync()
    {
        await UniTask.WaitForSeconds(_showPhraseTime);
        await this.transform.DOScale(Vector3.zero, _animationTime).SetEase(Ease.OutCirc);
    }
}
