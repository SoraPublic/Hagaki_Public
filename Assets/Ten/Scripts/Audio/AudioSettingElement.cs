using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioSettingElement : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private VolumeElement[] _volumeElements;
    [SerializeField]
    private AudioKind audioKind;
    private IntReactiveProperty volume = new IntReactiveProperty();
    public int GetVolume
    {
        get { return volume.Value; }
    }
    public void SetVolume(int value)
    {
        volume.SetValueAndForceNotify(Mathf.Clamp(value, 0, 5));
    }

    private bool _isSelect;

    private void Awake()
    {
        SetUp().Forget();
    }

    private async UniTask SetUp()
    {
        await UniTask.WaitUntil(() => AudioManager.instance != null);

        volume.Subscribe(I =>
        {
            for (int i = 0; i < _volumeElements.Length; i++)
            {
                if (i == I)
                {
                    _volumeElements[i].ChangeFlame(true);
                }
                else
                {
                    _volumeElements[i].ChangeFlame(false);
                }
            }
            if (AudioManager.instance.entryFlag)
            {
                AudioManager.instance.SetVolume(audioKind);
            }
        }).AddTo(this);

        switch (audioKind)
        {
            case AudioKind.BGM:
                AudioManager.instance.SetUpBGM(this);
                break;

            case AudioKind.SE:
                AudioManager.instance.SetUpSE(this);
                break;
        }

        foreach(VolumeElement element in _volumeElements)
        {
            element.parent = this;
        }
    }

    private void Update()
    {
        if (_isSelect)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeVolume(-1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeVolume(1);
            }
        }
    }

    private void ChangeVolume(int value)
    {
        SetVolume(GetVolume + Mathf.Clamp(value, -1, 1));
    }

    public void OnSelect(BaseEventData eventData)
    {
        _isSelect = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _isSelect = false;
    }
}

public enum AudioKind
{
    BGM,
    SE
}
