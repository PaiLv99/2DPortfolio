using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMng : MonoBehaviour //: TSingleton<MapMng>
{
    private Dictionary<int, Map> _maps = new Dictionary<int, Map>();
    public Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    public Map CurrMap { get; private set; }

    public Transform _holder;

    private readonly string _tilePath = "Prefabs/Map/Tiles";
    private readonly string _spritePath = "Image/Map/Tiles";

    public bool IsDone;
    public float progress;

    private PathFinderPQ pathFinder = new PathFinderPQ();


    public void Init()
    {
        //Tile[] tiles = Resources.LoadAll<Tile>(_tilePath);

        //for (int i = 0; i < tiles.Length; i++)
        //{
        //    tiles[i].SetData(DB.Instance.GetTileData(tiles[i].name));
        //    _tiles.Add(tiles[i].name, tiles[i]);
        //}

        Sprite[] sprites = Resources.LoadAll<Sprite>(_spritePath);

        for (int i = 0; i < sprites.Length; i++)
            _sprites.Add(sprites[i].name, sprites[i]);
    }

    public List<Tile> PathFinding(Tile start, Tile target)
    {
        return pathFinder.PathFind(start, target);
    }

    public Tile GetTile(Vector2 pos)
    {
        if (pos.x > 0 && pos.y > 0)
        {
            Tile tile = CurrMap._tiles[(int)pos.x, (int)pos.y];
            if (tile != null)
                return tile;
        }
       
        return null;
    }

    public Map GetCurrentMap()
    {
        return CurrMap;
    }

    public void CreateMap(int count)
    {
        _holder = GameObject.FindGameObjectWithTag("MapHolder").GetComponent<Transform>();

        StartCoroutine(IECreateMap(count));

        EndMap endMap = new EndMap(new GameObject("EndMap"), "EndMap");
        endMap._holder.gameObject.SetActive(false);
        endMap.PrevMap = _maps[_maps.Count - 1];
        _maps[_maps.Count - 1].NextMap = endMap;

        _maps.Add(_maps.Count, endMap);
    }

    private IEnumerator IECreateMap(int count)
    {
        int index = 0;

        while (index < count)
        {
            GameObject mapHolder = new GameObject("Map" + index);
            mapHolder.transform.parent = transform;

            Map map = new Map(mapHolder, mapHolder.name);
            map._holder.gameObject.SetActive(false);
            _maps.Add(index, map);

            yield return null;

            progress = (float)index / (float)count;
            index++;
        }

        CurrMap = _maps[0];
        CurrMap._holder.SetActive(true);

        IsDone = true;

        for (int i = 0; i < _maps.Count; i++)
        {
            if (_maps.ContainsKey(i - 1))
                _maps[i].PrevMap = _maps[i - 1];
            if (_maps.ContainsKey(i + 1))
                _maps[i].NextMap = _maps[i + 1];
        }

        InToDungeon();
    }

    public void Clear()
    {
        TileClear();

        for (int i = 0; i < _maps.Count; i++)
        {
            Destroy(_maps[i]._holder.gameObject);
        }

        _maps.Clear();
    }

    public void InToDungeon()
    {
        GameMng.Sound.SfxPlay("InToDungeon");
        // 현재 맵 설정
        // 플레이어 캐릭터 설정

        string path = DB.Instance.GetHeroData(GameMng.CharMng.HeroName)._prefab;

        Hero hero = Instantiate(Resources.Load<Hero>(path));
        GameMng.CharMng.AddChar(hero.UniqueID, hero);
        hero.Init(DB.Instance.GetHeroData(GameMng.CharMng.HeroName));
        hero.SetMap(CurrMap);
        hero.SetPosition(CurrMap._start);

        //PathFinder.Instance.SetMap(CurrMap);
        pathFinder.SetMap(CurrMap);

        // monster spawn
        GameMng.Spawn.SetMap(CurrMap);
        GameMng.Spawn.MonsterSpawn();

        // CameraSetting
        GameMng.Camera.Init();
        GameMng.Camera.CameraSetting();
        // 플레이어 턴 시작
        TurnMng.Instance.HeroTurn();
    }

    public void DownTranslate(Map map)
    {
        Debug.Log("DownStart");
        MapReplace(map);
        GameMng.CharMng.GetHero().SetPosition(map._start);

        if (map._holder.name == "EndMap")
        {
            EndMap endMap = map as EndMap;
            endMap.EndingItemCreate();
        }
        else      // Monster Spawn
        {
            GameMng.Spawn.SetMap(map);
            GameMng.Spawn.MonsterSpawn();
        }

        // GameStart
        GameMng.Sound.SfxPlay("InToDungeon");
        TurnMng.Instance.HeroTurn();
    }

    public void UpTranslate(Map map)
    {
        MapReplace(map);
        GameMng.CharMng.GetHero().SetPosition(map._end);

        // 몬스터 등록
        List<Monster> monsters = new List<Monster>();
        monsters.AddRange(CurrMap._holder.GetComponentsInChildren<Monster>());

        for (int i = 0; i < monsters.Count; i++)
            GameMng.CharMng.AddChar(monsters[i].UniqueID, monsters[i]);

        // GameStart
        GameMng.Sound.SfxPlay("InToDungeon");
        TurnMng.Instance.HeroTurn();
    }

    private void MapReplace(Map map)
    {
        // UI process
        //UIMng.Instance.SetActive(UIList.Loading, true);
        //UIMng.Instance.CallEvent(UIList.Loading, "FalseLoading");

        UIMng.Instance.FadeIn(FadeType.RandomPixel);

        GameMng.CharMng.MonsterClear();

        CurrMap._holder.gameObject.SetActive(false);

        Map nextMap = map;
        //PathFinder.Instance.SetMap(map);
        pathFinder.SetMap(map);
        GameMng.CharMng.GetHero().SetMap(map);

        nextMap._holder.gameObject.SetActive(true);
        CurrMap = nextMap;
    }

    public void TileClear()
    {
        foreach(var iter in _maps)
        {
            Map map = iter.Value;
            for (int x = 0; x < map._tiles.GetLength(0); x++)
                for (int y = 0; y < map._tiles.GetLength(1); y++)
                {
                    Tile tile = map.GetTile(x, y);
                    if (tile != null)
                    {
                        tile.ID = 0;
                        tile.Explored = false;
                        GameMng.Pool.TilePush(map._tiles[x, y], map._tiles[x, y].name);
                    }
                }
        }
    }
}
