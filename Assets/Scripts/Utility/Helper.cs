using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Helper
{
    // Creator
    public static T CreateComponent<T>(GameObject parent, bool callInit = true ) where T : Component
    {
        T t = parent.AddComponent<T>();

        if (callInit)
            t.SendMessage("Init", SendMessageOptions.DontRequireReceiver);

        return t;
    }

    // 새로운 오브잭트를 만든다.
    public static T CreateObject<T>(Transform parent, bool callInit = true) where T : Component
    {
        GameObject obj = new GameObject(typeof(T).Name, typeof(T));
        obj.transform.parent = parent;
        T t = obj.GetComponent<T>();

        if (callInit)
            t.SendMessage("Init", SendMessageOptions.DontRequireReceiver);

        return t;
    }

    // 
    public static T Instantiate<T>(string path, Vector3 pos, Quaternion rot, bool callInit = true, Transform parent = null) where T : Component
    {
        T t = Object.Instantiate(Resources.Load<T>(path), pos, rot, parent);

        if (callInit)
            t.gameObject.SendMessage("Init", SendMessageOptions.DontRequireReceiver);

        return t; 
    }
    // Finder
    public static T Find<T>(Transform transform, string path, bool state = true) where T : Component
    {
        Transform obj = transform.Find(path);

        if (obj != null)
        {
            obj.gameObject.SetActive(state);
            return obj.GetComponent<T>();
        }

        return null;
    }


    // Math
    public static Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result = (u3) * p1 + (3f * u2 * t) * p2 + (3f * u * t2) * p3 + (t3) * p4;

        return result;
    }

    public static Vector2 Bezier2D(float t, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector2 result = (u3) * p1 + (3f * u2 * t) * p2 + (3f * u * t2) * p3 + (t3) * p4;
        return result;
    }

    // Shuffle
    public static T[] ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rand = Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }

        return array;
    }


    public static T GetShuffleRandomValue<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int rand = Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[rand];
            array[rand] = temp;
        }

        int index = Random.Range(0, array.Length);
        return array[index];
    }

    // Chosen
    public static float Chosen(float[] prob)
    {
        float total = 0;

        foreach (var value in prob)
            total += value;

        float rand = Random.value * total;

        for (int i = 0; i < prob.Length; i++)
        {
            if (rand < prob[i])
                return i;

            rand -= prob[i];
        }

        return prob.Length - 1;
    }

    // Shader
    public static void SetPixelsAlpha(this Texture2D texture, int x, int y, float alpha)
    {
        //var pixels = texture.GetPixels();
        //pixels[index].a = alpha;
        //texture.SetPixels(pixels);

        var pixel = texture.GetPixel(x,y);
        pixel.a = alpha;
        texture.SetPixel(x, y, pixel);
        texture.Apply();
    }

    

    public static IEnumerator BlinkCount(Transform t, int count)
    {
        Debug.Log("In Blink");
        Color color = new Color(1, 1, 1, 0.5f);
        float blinkSpeed = 2;
        int currCount = 0;
        while (currCount <= count)
        {
            float percent = 0;
            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                t.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, color, interpolation);
                yield return null;
            }
            currCount++;
            yield return null;
        }
    }

    public static IEnumerator Blink(Transform t)
    {
        Debug.Log("In Blink");
        Color color = new Color(1, 1, 1, 0.5f);
        float blinkSpeed = 2;

        while (true)
        {
            float percent = 0;

            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 2;
                t.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, color, interpolation);
                yield return null;
            }
            yield return null;
        }
       
    }

    public static IEnumerator UIBlink(Transform t)
    {
        Color color = new Color(1, 1, 1, 0.5f);
        float blinkSpeed = 2;

        while(true)
        {
            float percent = 0;

            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 2;
                t.GetComponent<Image>().color = Color.Lerp(Color.white, color, interpolation);
                yield return null;
            }
            yield return null;
        }
    }


    public static Tile RayCastWithBresenham(Vector2 start, Vector2 target, Map map)
    {
        Vector2 delta = target - start;

        Vector2 primaryStep = new Vector2(Mathf.Sign(delta.x), 0);
        Vector2 secondaryStep = new Vector2(0, Mathf.Sign(delta.y));

        int primary = (int)Mathf.Abs(delta.x);
        int secondary = (int)Mathf.Abs(delta.y);

        if (secondary > primary)
        {
            int temp = primary;
            primary = secondary;
            secondary = temp;

            Vector2 tempVec = primaryStep;
            primaryStep = secondaryStep;
            secondaryStep = tempVec;
        }

        Vector2 curr = start;
        int erorr = 0;

        Tile currTile;
        Tile prevTile;

        while (true)
        {
            int x = (int)curr.x;
            int y = (int)curr.y;

            prevTile = map._tiles[x, y];

            curr += primaryStep;
            erorr += secondary;

            if (erorr * 2 >= primary)
            {
                curr += secondaryStep;
                erorr -= primary;
            }

            currTile = map._tiles[(int)curr.x, (int)curr.y];

            if (currTile.TILETYPE == TileType.Wall)
                return prevTile;

            if (currTile.TILETYPE == TileType.Monster || curr == target)
                break;
        }
        return currTile;
    }

    public static int DistanceVector2(Vector2 form, Vector2 to)
    {
        Vector2 delta = to - form;

        int deltaint = (int)(to - form).magnitude;

        return (int)(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2));
    }

    public static int RandomValue(int value)
    {
        float min = value * 0.5f;
        float max = value + min;
        int result = (int)Random.Range(min, max);
        return result;
    }
}
