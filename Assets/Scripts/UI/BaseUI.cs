using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    public virtual void Init()
    {
        
    }

    public virtual void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

   
}
