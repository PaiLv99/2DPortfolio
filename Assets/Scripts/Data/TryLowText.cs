using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryLowText
{
    private Dictionary<int, Dictionary<string, string>> _table = new Dictionary<int, Dictionary<string, string>>();

    public void Load(string path)
    {
        TextAsset text = Resources.Load<TextAsset>(path);

        if (text != null)
            Parse(text.text);
    }

    protected void Parse(string text)
    {
        string[] row = text.Split('\n');

        List<string> rowList = new List<string>();

        for (int i = 0; i < row.Length; i++)
        {
            if (!string.IsNullOrEmpty(row[i]))
            {
                string s = row[i].Replace('\r', ' ');
                rowList.Add(s.Trim());
            }
        }

        string[] sub = rowList[0].Split(',');

        for (int i = 0; i < rowList.Count; i++)
        {
            string[] value = rowList[i].Split(',');

            int mainKey = 0;

            // Exception
            if (!int.TryParse(value[0], out mainKey))
                continue;

            if (!_table.ContainsKey(mainKey))
                _table.Add(mainKey, new Dictionary<string, string>());

            for (int col = 1; col < value.Length; col++)
                if (!_table[mainKey].ContainsKey(sub[col]))
                    _table[mainKey].Add(sub[col], value[col]);
        }

    }

    public int ToI(int mainKey, string subKey)
    {
        if (_table.ContainsKey(mainKey))
        {
            Dictionary<string, string> sub = _table[mainKey];

            if (sub.ContainsKey(subKey))
            {
                string value = sub[subKey];
                int.TryParse(value, out int iValue);
                return iValue;
            }
        }
        return -1;
    }

    public float ToF(int mainKey, string subKey)
    {
        if (_table.ContainsKey(mainKey))
        {
            Dictionary<string, string> sub = _table[mainKey];

            if (sub.ContainsKey(subKey))
            {
                string value = sub[subKey];
                float.TryParse(value, out float fValue);
                return fValue;
            }
        }
        return -1.0f;
    }

    public string ToS(int mainKey, string subKey)
    {
        if (_table.ContainsKey(mainKey))
        {
            Dictionary<string, string> sub = _table[mainKey];

            if (sub.ContainsKey(subKey))
                return sub[subKey];
        }
        return string.Empty;
    }
}
