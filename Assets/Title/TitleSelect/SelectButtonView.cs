using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonView : MonoBehaviour
{
    private GameObject _line;
    private Image _lineImage;
    private Image _brushImage;
    [SerializeField]
    private GameObject _topScoreUI;
    [SerializeField]
    private Color _firstColor = Color.black;
    [SerializeField]
    private Color _selectedColor = Color.red;
    private void Awake()
    {
        _line = this.transform.GetChild(transform.childCount - 1).gameObject;
        _lineImage = _line.transform.GetChild(0).GetComponent<Image>();
        _brushImage = _line.transform.GetChild(1).GetComponent<Image>();
        _line.SetActive(false);
        _lineImage.color = _firstColor;
    }

    public void Init()
    {
        _line.gameObject.SetActive(true);
    }

    public void Highlighted()
    {
        if (_line.activeSelf)
        {
            if(_lineImage.color == _selectedColor)
            {
                _brushImage.gameObject.SetActive(true);
                AudioManager.instance.OnSelectUI.Play();
            }
            return;
        }

        AudioManager.instance.OnSelectUI.Play();
        _line.gameObject.SetActive(true);
    }

    public void Selected()
    {
        _line.gameObject.SetActive(true);
        _topScoreUI.SetActive(true);
        _lineImage.color = _selectedColor;
        this.GetComponent<CursorSelectedButton>().interactable = false;
    }

    public void Unhighlighted()
    {
        if(_lineImage.color == _selectedColor)
        {
            _brushImage.gameObject.SetActive(false);
            return;
        }
        _line.gameObject.SetActive(false);
    }

    public void Unselected()
    {
        _line.gameObject.SetActive(false);
        _topScoreUI.SetActive(false);
        _brushImage.gameObject.SetActive(true);
        _lineImage.color = _firstColor;
        this.GetComponent<CursorSelectedButton>().interactable = true;
    }
}
