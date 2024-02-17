using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HagakiAnimation : MonoBehaviour
{
    [SerializeField]
    private float animationTime = 0.1f;
    [SerializeField]
    private Vector3 point;
    private Vector3 iniPos;

    private void Awake()
    {
        iniPos = this.transform.position;
    }

    public async UniTask doPostCardAnimationAsync()
    {
        this.transform.DOComplete();
        await this.transform.DOMove(point,animationTime);
    }

    public void Initialize()
    {
        this.gameObject.SetActive(false);
        this.transform.position = iniPos;
    }
}
