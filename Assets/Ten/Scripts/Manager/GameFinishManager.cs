using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameFinishManager : MonoBehaviour
{
    [SerializeField]
    private Canvas[] _UICanvases;
    [SerializeField]
    private Image _finishMessage;

    private void Start()
    {
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitUntil(() => GameStateManager.instance != null);
        yield return new WaitUntil(() => GameStateManager.instance.IsGame);
        yield return new WaitUntil(() => !GameStateManager.instance.IsGame && !GameStateManager.instance.IsInterrupt);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_finishMessage.transform.DOScale(1, 1.0f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            _finishMessage.gameObject.SetActive(true);
        })).AppendInterval(1.0f);

        yield return new WaitUntil(() => !sequence.IsActive());

        AudioManager.instance.FadeOutChangeBGM(BGMKind.Result);

        yield return new WaitUntil(() => AudioManager.instance.State == BGMChangeState.FadeOut);

        AudioManager.instance.FadeInBGM();

        for (int i = 0; i < _UICanvases.Length; i++)
        {
            _UICanvases[i].gameObject.SetActive(false);
        }

        yield return new WaitUntil(() => AudioManager.instance.State == BGMChangeState.FadeIn);

        TenSceneManager.AddScene(Scene.Result);
    }
}
