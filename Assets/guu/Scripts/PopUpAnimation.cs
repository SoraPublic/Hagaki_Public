using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PopUpAnimation : MonoBehaviour
{
    [SerializeField]
    private float _animationTime = 0.2f;
    private Transform _myTransform;
    private void Awake()
    {
        _myTransform = this.transform;
        _myTransform.localScale = Vector3.zero;
    }

    public async UniTask ShowAnimation()
    {
        await _myTransform.DOScale(Vector3.one, _animationTime).SetEase(Ease.InOutExpo);
    }
}
