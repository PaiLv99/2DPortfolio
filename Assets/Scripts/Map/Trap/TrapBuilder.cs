using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapBuilder
{
    public struct TrapCoord
    {
        public int x, y;
        public TrapCoord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    private Map _map;
    private Dictionary<string, Sprite> _sprite = new Dictionary<string, Sprite>();

    public TrapBuilder(Map map)
    {
        _map = map;
        //_sprite = GameMng.Map._sprites;
    }

    public void BuildTrap(Room room)
    {
        //if (room._x <= 5 || room._y <= 5)
            //return;

        //if (room._isDeco)
            //return;

        int rand = Random.Range(0, 10);

        if (rand >= 6)
        {
            Queue<TrapCoord> coords = new Queue<TrapCoord>(RandomCoordArray(room));
            int trapCount = Random.Range(0, coords.Count);

            int trapcategory = Random.Range(0, 3);

            for (int i = 0; i < trapCount; i++)
            {
                TrapCoord coord = coords.Dequeue();
                Tile tile = _map.GetTile(coord.x, coord.y);

                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    tile.SetSprite(_sprite["Trap"]);
                    tile.TILETYPE = TileType.Trap;
                }
                else
                {
                    tile.IsHiding = true;
                    tile.TILETYPE = TileType.Trap;
                }


                tile.gameObject.AddComponent<Animator>();
                tile.gameObject.AddComponent<Trap>().Init(tile.X, tile.Y);
            }
        }


    }

    private TrapCoord[] RandomCoordArray(Room room)
    {
        TrapCoord[] positions;
        List<TrapCoord> coords = new List<TrapCoord>();

        for (int x = room._x; x < room._xMax; x++)
        {
            for (int y = room._y; y < room._yMax; y++)
            {
                if (_map.GetTile(x, y).TILETYPE == TileType.Floor)
                    coords.Add(new TrapCoord(x, y));
            }
        }

        positions = Helper.ShuffleArray(coords.ToArray());
        return positions;
    }

}
