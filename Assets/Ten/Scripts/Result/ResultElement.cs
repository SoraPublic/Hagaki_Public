using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultElement : MonoBehaviour
{
    public CanvasGroup Group;
    [SerializeField]
    private Image[] _backImages;
    [SerializeField]
    private Color _correctColor, _incorrectColor;
    [SerializeField]
    private TMP_Text _correction;
    [SerializeField]
    private TMP_Text _questionSentence;
    [SerializeField]
    private TMP_Text _correctionSentence;
    [SerializeField]
    private TMP_Text _answerSentence;
    [SerializeField]
    private TMP_Text _time;

    public void SetUI(QuizManager.AskedQuiz quizElement)
    {
        if (quizElement.IsCorect)
        {
            foreach(var backImage in _backImages)
            {
                backImage.color = _correctColor;
            }

            _correction.SetText("正");
        }
        else
        {
            foreach (var backImage in _backImages)
            {
                backImage.color = _incorrectColor;
            }

            _correction.SetText("誤");
        }

        _questionSentence.SetText(quizElement.Quiz);
        _answerSentence.SetText(quizElement.Answer);
        _correctionSentence.SetText(quizElement.CorrectAnswer);
        _time.SetText(quizElement.ClearTime.ToString("f2") + "秒");
    }
}
