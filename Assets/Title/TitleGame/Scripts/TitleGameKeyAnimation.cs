using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleGameKeyAnimation : MonoBehaviour
{
    [SerializeField]
    float animationTime = 0.1f;
    private Image _image;
    [SerializeField]
    private Color _pushedKeyColor = new Color(255, 180, 180);
    [SerializeField]
    private KeyCode _key;
    public KeyCode Key => _key;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    public void doAnimetion()
    {
        var sequence = DOTween.Sequence();
        sequence.Join(_image.DOColor(_pushedKeyColor, animationTime).SetEase(Ease.InSine))
            .Join(this.transform.DOScale(Vector3.one * 0.5f, animationTime).SetEase(Ease.InSine))
            .Append(this.transform.DOScale(Vector3.one, animationTime).SetEase(Ease.InSine));
    }
}
