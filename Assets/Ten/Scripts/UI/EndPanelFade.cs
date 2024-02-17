using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanelFade : MonoBehaviour
{
    public static EndPanelFade instance;

    private bool _isFade = false;
    public bool IsFade => _isFade;
    [SerializeField]
    private CanvasGroup _fadeCanvas;

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
    }

    public void FadeIn()
    {
        _fadeCanvas.DOFade(1, 1.0f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            _fadeCanvas.gameObject.SetActive(true);
        }).OnComplete(() =>
        {
            _isFade = true;
        });
    }
}
