using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowText
{
    private Dictionary<int, Dictionary<string, string>> _tableDic = new Dictionary<int, Dictionary<string, string>>();
    private readonly string Path = "Table/";

    public void Load(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(Path + path);

        if (textAsset != null)
            Parse(textAsset.text);
    }

    private void Parse(string text)
    {
        string[] row = text.Split('\n');

        List<string> rowlist = new List<string>();
        for (int i = 0; i < row.Length; ++i)
        {
            if (!string.IsNullOrEmpty(row[i]))
            {
                string s = row[i].Replace('\r', ' ');
                rowlist.Add(s.Trim());
            }
        }
        // 속성 이름을 분류합니다.
        string[] subject = rowlist[0].Split(',');

        for (int j = 1; j < rowlist.Count; ++j)
        {
            // 값을 분류합니다.
            string[] val = rowlist[j].Split(',');

            //int mainKey = 0;

            if (!int.TryParse(val[0], out int mainKey))
            {
                Debug.Log(j + " 행 값이 정수가 아닙니다.");
                continue;
            }

            if (!_tableDic.ContainsKey(mainKey))
                _tableDic.Add(mainKey, new Dictionary<string, string>());

            for (int col = 1; col < val.Length; ++col)
            {
                if (!_tableDic[mainKey].ContainsKey(subject[col]))
                    _tableDic[mainKey].Add(subject[col], val[col]);
            }
        }

    }

    public int ToI(int mainKey, string subKey)
    {
        if (_tableDic.ContainsKey(mainKey))
        {
            Dictionary<string, string> subDic = _tableDic[mainKey];

            if (subDic.ContainsKey(subKey))
            {
                string val = subDic[subKey];
                int.TryParse(val, out int iVal);
                return iVal;
            }
        }

        return -1;
    }

    public float ToF(int mainKey, string subKey)
    {
        if (_tableDic.ContainsKey(mainKey))
        {
            Dictionary<string, string> subDic = _tableDic[mainKey];

            if (subDic.ContainsKey(subKey))
            {
                string val = subDic[subKey];
                float.TryParse(val, out float iVal);
                return iVal;
            }
        }
        return -1;
    }

    public string ToS(int mainKey, string subKey)
    {
        if (_tableDic.ContainsKey(mainKey))
        {
            Dictionary<string, string> subDic = _tableDic[mainKey];

            if (subDic.ContainsKey(subKey))
            {
                return subDic[subKey];
            }
        }
        return string.Empty;
    }

    public int GetCount()
    {
        return _tableDic.Count;
    }
}



