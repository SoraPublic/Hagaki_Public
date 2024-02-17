using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleGameTextAnimation : MonoBehaviour
{
    private readonly Color FIRST_COLOR = Color.white;
    private int insertIndex = 1;
    [SerializeField]
    private string _corectTextColor = "#FFB4B4";
    private string _firstQuestionSentence;
    private TMP_Text myText;

    private void Awake()
    {
        myText = GetComponent<TMP_Text>();
        _firstQuestionSentence = myText.text;
        myText.color = FIRST_COLOR;
    }
    public void doAnimation()
    {
        if(_firstQuestionSentence.Length<insertIndex)
        {
            Debug.Log("挿入する要素番号が間違っています");
            return;
        }

        string insertText = $"<color={_corectTextColor}>";
        string insertEndText = "</color>";

        string changedText = _firstQuestionSentence.Insert(insertIndex, insertEndText);
        changedText = changedText.Insert(0, insertText);
        myText.text = changedText;

        insertIndex++;
    }
}
