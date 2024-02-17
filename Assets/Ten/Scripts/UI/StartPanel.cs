using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.DOFade(0, 1.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
