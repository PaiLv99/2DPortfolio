using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private float[] _chestProb = { 0.2f, 0.2f, 0.5f, 0.1f };
    private float[] _weaponProb = { };
    private float[] _potionProb = { };
    private float[] _scrollProb = { };

    private void CreateItem()
    {
        float index = Helper.Chosen(_chestProb);

    }
}
