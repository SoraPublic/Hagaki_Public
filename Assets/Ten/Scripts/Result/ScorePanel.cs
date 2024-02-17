using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _evaluationText;
    [SerializeField]
    private EvaluationSetting _setting;
    [SerializeField]
    private AudioSource _drumroll, _drumend;

    private string[] _evaluationStrs = new string[5]
    {
        "秀","優","良","可","不可"
    };
    private string _evaluationStr = string.Empty;
    private bool _isSet = false;
    public bool IsSet => _isSet;

    public IEnumerator SetScorePanel(int value, QuizManager.Level level)
    {
        if (_isSet)
        {
            yield break;
        }

        _isSet = true;

        Setting setting = Array.Find(_setting.Settings, I => I.Level == level);
        if(value > setting.Value)
        {
            _evaluationStr = "<color=red>秀";
        }
        else if(value > setting.Value * 0.8f)
        {
            _evaluationStr = "<color=red>優";
        }
        else if(value > setting.Value * 0.6f)
        {
            _evaluationStr = "<color=red>良";
        }
        else if(value > setting.Value * 0.4f)
        {
            _evaluationStr = "<color=black>可";
        }
        else
        {
            _evaluationStr = "<color=blue>不可";
        }

        Tweener tweener = _scoreText.DOCounter(0, value, 3.0f, true);

        yield return new WaitUntil(() => !tweener.IsActive());

        _drumroll.Play();

        for(int i = 0; _drumroll.isPlaying; i++)
        {
            _evaluationText.text = _evaluationStrs[i % _evaluationStrs.Length];
            yield return new WaitForSeconds(0.1f);
        }

        _evaluationText.text = _evaluationStr;
        _evaluationText.DOFontSize(250, 0.3f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        _drumend.Play();
    }
}
