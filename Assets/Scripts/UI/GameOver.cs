using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : BasePopUpUI
{
    private Color _originColor;
    private bool _isOpen;


    public override void Init()
    {
        _baseImage = Helper.Find<Image>(transform, "Base", false);
        _originColor = _baseImage.color;
        GetComponentInChildren<GameOverButton>().Init(GoToMain);
    }

    public override void Open()
    {
        _isOpen = true;
        _baseImage.gameObject.SetActive(true);
        StartCoroutine(IEFade());
        //StartCoroutine(IEChecker());
    }

    public override void Closed()
    {
        _isOpen = false;
        SetActive(false);
    }

    private void GoToMain()
    {
        Closed();
        GameMng.Instance.Clear();
        //GameMng.Map.Clear();
        //GameMng.Instance.GameMngReset();
        SceneMng.Instance.SceneLoading(SceneType.Title);
    }

    private IEnumerator IEFade()
    {
        //GameObject rip = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/RIP"));
        //rip.transform.position = GameMng.Instance.GetHero().transform.position;
        //Vector3 ripPos = rip.transform.position;

        float elapsedTime = 0;
        //float percent = 0;

        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime;
            //percent += Time.deltaTime * 2;

            _baseImage.color = Color.Lerp(Color.clear, _originColor, elapsedTime);
            //float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            //rip.transform.position = Vector3.Lerp(ripPos, ripPos + new Vector3(0, 0.25f, 0), interpolation);
            yield return null;
        }
    }

}
