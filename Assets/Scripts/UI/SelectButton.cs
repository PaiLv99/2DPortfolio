using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image _image;
    //private System.Action Action;
    private Dictionary<string, Sprite> _sprites;
    private Coroutine _runningCoroutine;


    private void Start()
    {
        _image = GetComponent<Image>();
        _sprites = new Dictionary<string, Sprite>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/UI/Select");

        for (int i = 0; i < sprites.Length; i++)
            _sprites.Add(sprites[i].name, sprites[i]);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (Action != null)
            //Action();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = _sprites[""];
        Blick();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.sprite = _sprites[""];
        StopCoroutine(_runningCoroutine);
    }

    private void Blick()
    {
        if (_runningCoroutine != null)
            StopCoroutine(_runningCoroutine);
        _runningCoroutine = StartCoroutine(IEBlick());
    }

    private IEnumerator IEBlick()
    {
        float blinkSpeed = 1f;
        Color alpha = _image.color;
        float alphas = 0;

        while (true)
        {
            float percent = 0;
            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                alphas = Mathf.Lerp(0, 1, interpolation);
                alpha.a = alphas;
                _image.color = alpha;
                yield return null;
            }
            yield return null;
        }
    }
}
