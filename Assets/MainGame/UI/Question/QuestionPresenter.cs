using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPresenter : MonoBehaviour
{
    private QuestionView _questionView;
    private void Awake()
    {
        _questionView = GetComponent<QuestionView>();
    }

    public void SetQuestionText()
    {
        string maru = "<size=50%>    ";
        for(int i=0; i< QuizManager.instance.GetQuizAnswer().Length; i++)
        {
            maru += "<sprite=0>";
        }
        maru += "</size>\n";
        string question = QuizManager.instance.GetQuizSentence();
        _questionView.UpdateQuestionText(maru+question);
    }
}
