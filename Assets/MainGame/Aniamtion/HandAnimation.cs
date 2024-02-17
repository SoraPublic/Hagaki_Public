using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Threading;
using System;

public class HandAnimation : MonoBehaviour
{
    [SerializeField]
    private float _idleAnimationTime = 0.2f;
    [SerializeField]
    private float _putAwayAnimationTime = 0.1f;
    [SerializeField]
    private float _writeAnimationTime = 0.5f;
    private Vector3 _iniPos;
    [SerializeField]
    private Vector3 _putAwayPos;
    [SerializeField]
    private Vector3 _insideInk;
    [SerializeField]
    private Vector3 _onPostCard;
    [SerializeField]
    private List<Vector3> _handsPointFrontList;
    [SerializeField]
    private List<Vector3> _handsPointBackList;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private CancellationToken _token;
    [SerializeField]
    private AudioSource _audioSource;

    private void Awake()
    {
        _token = _cancellationTokenSource.Token;
        _iniPos = this.transform.position;
    }

    public async UniTask DoIdleAnimationAsync()
    {
        this.transform.DOComplete();
        var sequence = DOTween.Sequence();
        await sequence.Append(this.transform.DOMove(_insideInk, _idleAnimationTime).SetEase(Ease.InOutSine))
            .Append(this.transform.DOMoveY(3, _idleAnimationTime).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutCirc))
            .Append(this.transform.DOMoveZ(-2, _idleAnimationTime).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutCirc))
            .Append(this.transform.DOMove(_onPostCard, _idleAnimationTime).SetEase(Ease.InOutSine)).WithCancellation(_token);
    }

    public async UniTask DoHandFrontPostCardAnimationAsync()
    {
        SetAnimation();
        this.transform.DOComplete();
        for (int i = 0; i < _handsPointFrontList.Count; i++)
        {
            await this.transform.DOMove(_handsPointFrontList[i], _writeAnimationTime).SetEase(Ease.InOutSine);
        }
    }

    public async UniTask DoHandBackPostCardAnimationAsync()
    {
        SetAnimation();
        this.transform.DOComplete();
        for (int i=0; i< _handsPointBackList.Count; i++)
        {
            await this.transform.DOMove(_handsPointBackList[i], _writeAnimationTime).SetEase(Ease.InOutSine);
        }
    }

    public async UniTask PutAwayPostCardAsync()
    {
        this.transform.DOComplete();
        var sequence = DOTween.Sequence();
        await sequence.Append(this.transform.DOMove(_putAwayPos, _putAwayAnimationTime))
            .Append(this.transform.DOMove(_iniPos, _putAwayAnimationTime));
    }

    public void SetAnimation()
    {
        _audioSource.Play();
        _cancellationTokenSource.Cancel();
    }

    public void SetIdle()
    {
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        _token = _cancellationTokenSource.Token;
        DoIdleAnimationAsync().Forget();
    }

}
