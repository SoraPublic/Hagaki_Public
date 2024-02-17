using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField]
    private Image _flame;
    [SerializeField]
    private int _level;
    [HideInInspector]
    public AudioSettingElement parent;
    [SerializeField]
    private AudioSource _audioSource;

    public void ChangeFlame(bool value)
    {
        _flame.gameObject.SetActive(value);
        _audioSource.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.SetVolume(_level);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(parent.gameObject);
    }
}
