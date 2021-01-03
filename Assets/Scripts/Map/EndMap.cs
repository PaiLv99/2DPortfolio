using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMap : Map
{
    public EndMap(GameObject holder, string str) : base(holder, str)
    {
        _rooms.Clear();
        _doors.Clear();

        DeleteTile();
        CreateEndMap();
    }

    private void DeleteTile()
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
            for (int y = 0; y < _tiles.GetLength(1); y++)
                if (_tiles[x, y] != null)
                    GameObject.Destroy(_tiles[x, y].gameObject);

        _tiles = null;
    }

    private void CreateEndMap()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Map/EndMap"));
        obj.name = "EndMap";
        obj.transform.parent = _holder.transform;

        _tiles = new Tile[40, 40];

        _height = 40;
        _width = 40;

        GameObject[] tileObj = GameObject.FindGameObjectsWithTag("Tile");

        foreach (var iter in tileObj)
        {
            Tile tile = iter.GetComponent<Tile>();
            tile.TileBuild((int)tile.transform.position.x, (int)tile.transform.position.y);
            if (tile.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                tile.TILETYPE = TileType.Wall;
                tile.ORIGINTYPE = TileType.Wall;
                tile.Transparent = false;
            }
            else
            {
                tile.TILETYPE = TileType.Floor;
                tile.ORIGINTYPE = TileType.Floor;
                tile.Transparent = true;
            }
            _tiles[tile.Position.x, tile.Position.y] = tile;

        }
        _start = _tiles[6, 4].Position;

        
    }

    public void EndingItemCreate()
    {
        GameBoy gameBoy = GameObject.Instantiate(Resources.Load<GameBoy>("Prefabs/Item/GameBoy"));
        gameBoy.transform.parent = _holder.transform;
        gameBoy.transform.position = new Vector3(6, 28);
        gameBoy.Init(DB.Instance.GetItemData("GameBoy"));
        gameBoy.HeroCheck();
    }
}
