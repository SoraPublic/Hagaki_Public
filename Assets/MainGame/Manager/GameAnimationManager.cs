using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimationManager : MonoBehaviour
{
    public static GameAnimationManager instance;
    [SerializeField]
    private float _intervalAnimation = 0.5f;
    [SerializeField]
    private GameObject _beforeInkstone;
    [SerializeField]
    private GameObject _afterInkstone;
    [SerializeField]
    private HandAnimation _hand;
    [SerializeField]
    private List<HagakiAnimation> _postCardFrontList;
    [SerializeField]
    private List<HagakiAnimation> _postCardBackList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        SetInitObject();
    }

    //private async void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.F))
    //    {
    //        await DoFrontAnimationAsync();
    //    }
    //    if(Input.GetKeyDown(KeyCode.B))
    //    {
    //        await DoBackAnimationAsync();
    //    }
    //    if(Input.GetKeyDown(KeyCode.I))
    //    {
    //        await DoIdleAnimationAsync(true);
    //    }
    //}

    private void SetInitObject()
    {
        _hand.gameObject.SetActive(true);
        _postCardFrontList[0].gameObject.SetActive(false);
        _postCardFrontList[1].gameObject.SetActive(false);
        _postCardBackList[0].gameObject.SetActive(false);
        _postCardBackList[1].gameObject.SetActive(false);
        _beforeInkstone.SetActive(true);
        _afterInkstone.SetActive(false);
    }

    private void ShowBeforePostCard(List<HagakiAnimation> postCardList)
    {
        postCardList[0].gameObject.SetActive(true);
        postCardList[1].gameObject.SetActive(false);
    }

    private void ShowAfterPostCard(List<HagakiAnimation> postCardList)
    {
        postCardList[0].gameObject.SetActive(false);
        postCardList[1].gameObject.SetActive(true);
    }

    public void ChangeInkstone()
    {
        _beforeInkstone.SetActive(false);
        _afterInkstone.SetActive(true);
    }

    public async UniTask DoIdleAnimationAsync(bool isFront)
    {
        if (_postCardFrontList[0] == null || _postCardBackList[0] == null)
        {
            return;
        }

        if (isFront)
        {
            ShowBeforePostCard(_postCardFrontList);
        }
        else
        {
            ShowBeforePostCard(_postCardBackList);
        }

        await _hand.DoIdleAnimationAsync();
    }

    public async UniTask DoWriteAnimation()
    {
        if(_postCardFrontList[0]==null || _postCardBackList[0] == null)
        {
            return;
        }

        if (_postCardFrontList[0].gameObject.activeSelf)
        {
            await DoFrontAnimationAsync();
        }
        else
        {
            await DoBackAnimationAsync();
        }
    }

    private async UniTask DoFrontAnimationAsync()
    {
        await _hand.DoHandFrontPostCardAnimationAsync();
        ShowAfterPostCard(_postCardFrontList);
        await WaitNextAnimationAsync();
        await UniTask.WhenAll(_hand.PutAwayPostCardAsync(),
            _postCardFrontList[1].doPostCardAnimationAsync()
            );
        _postCardFrontList[1].Initialize();
    }

    private async UniTask DoBackAnimationAsync()
    {
        await _hand.DoHandBackPostCardAnimationAsync();
        ShowAfterPostCard(_postCardBackList);
        await WaitNextAnimationAsync();
        await UniTask.WhenAll(_hand.PutAwayPostCardAsync(),
            _postCardBackList[1].doPostCardAnimationAsync()
            );
        _postCardBackList[1].Initialize();
    }

    private async UniTask WaitNextAnimationAsync()
    {
        await UniTask.WaitForSeconds(_intervalAnimation);
    }
}