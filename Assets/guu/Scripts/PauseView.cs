using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField]
    private Button _returnButton;
    public Button ReturnButton => _returnButton;
    [SerializeField]
    private Button _exitButton;
    public Button ExitButton => _exitButton;
}
