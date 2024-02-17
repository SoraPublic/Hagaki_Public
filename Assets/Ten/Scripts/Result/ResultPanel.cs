using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _content;
    [SerializeField]
    private GameObject _resultElement;
    [SerializeField]
    private GameObject _resultParentPanel;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private AudioSource _directAudio;
    private bool _isFinish = false;
    public bool IsFinish => _isFinish;

    public IEnumerator CreateResultPanel(List<QuizManager.AskedQuiz> quizzes)
    {
        yield return new WaitForSeconds(2);

        _content.SetActive(true);
        _directAudio.Play();

        yield return new WaitForSeconds(2);

        foreach (Transform n in _resultParentPanel.transform)
        {
            Destroy(n.gameObject);
        }

        int i = 0;
        foreach (var quiz in quizzes)
        {
            GameObject element = Instantiate(_resultElement, Vector3.zero, Quaternion.identity, _resultParentPanel.transform);
            ResultElement resultElement = element.GetComponent<ResultElement>();
            resultElement.SetUI(quiz);
            element.GetComponent<RectTransform>().DOScaleY(1, 0.5f).From(0).SetEase(Ease.OutQuad);
            _scrollRect.verticalNormalizedPosition = 0;
            yield return new WaitForSeconds(0.1f);
            i++;
        }

        _isFinish = true;
    }
}
