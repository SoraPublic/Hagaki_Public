using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseDialog;
    [SerializeField]
    private ButtonType _type;


    private void FixedUpdate()
    {
        if(_type!=ButtonType.pause)
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {  
            onButtonClick();
        }
    }

    public void onButtonClick()
    {
        switch(_type)
        {
            case ButtonType.pause:
                if(_pauseDialog.activeSelf)
                {
                    _pauseDialog.SetActive(false);
                    this.GetComponent<Button>().enabled = true;
                }
                else
                {
                    _pauseDialog.SetActive(true);
                    this.GetComponent<Button>().enabled = false;
                }
                break;
            case ButtonType.close:
                _pauseDialog.SetActive(false);
                break;
            case ButtonType.exit:
                break;
            default:
                break;
        }
    }

    public enum ButtonType
    {
        pause,
        close,
        exit,
    }
}
