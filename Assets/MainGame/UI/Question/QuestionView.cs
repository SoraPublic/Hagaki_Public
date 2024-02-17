using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _questionText;

    public void UpdateQuestionText(string Question)
    {
        _questionText.text = Question;
    }
}
