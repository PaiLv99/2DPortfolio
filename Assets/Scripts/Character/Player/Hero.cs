using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : BaseChar
{
    private HeroStatus _status;
    private readonly Vector2 _offset = new Vector2(0, 0.25f);

    public System.Action Death;
    public int _radius = 5;

    public bool IsShoot { get; set; }
    public bool IsThrow { get; set; }
    public Slot SelectedSlot { private get; set; }
    public Tile SelectTile { get; private set; }
    private Tile _currTile, _prevTile;
    private FoV _fov;

    public Queue<BaseTask> TemporaryStorage { get; private set; } = new Queue<BaseTask>();

    public void Init(Data.HeroData info)
    {
        _animator = GetComponent<Animator>();

        if (_status == null)
        {
            _status = new HeroStatus(info);
        }

        Death += () => GameMng.Camera.SetCameraMode(Define.CameraMode.None);
        Death += () => GameMng.Input.TouchAction -= OnTouch;
        Death += DeathEffect;
        Death += () => Destroy(this.gameObject);

        ItemMng.Instance.SetStatus(_status);

        GameMng.Input.TouchAction -= OnTouch;
        GameMng.Input.TouchAction += OnTouch;
        _status.StartEquip();

        UIMng.Instance.CallEvent(UIList.HUD, "RegistHero", this);
        FoV fov = new FoV(GameMng.Map.CurrMap, _radius);
        _fov = fov;
    }

    public override void SetMap(Map map)
    {
        base.SetMap(map);
        _fov.SetMap(map);
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos + _offset;
    }

    public void FoV()
    {
        for (int i = 0; i < GameMng.Map.CurrMap._doors.Count; i++)
            GameMng.Map.CurrMap._doors[i].Check();

        // 초기화
        for (int x = 0; x < _map._width; x++)
        {
            for (int y = 0; y < _map._height; y++)
            {
                if (_map._tiles[x,y] != null)
                {
                    _map._tiles[x, y].Visible = false;
                    _map._tiles[x, y].SetVisible();
                }
            }
        }

        List<Vector2Int> tileList = new List<Vector2Int>();
        Vector2Int pos = NotifyPosition();
        ShadowCaster.ComputeFOVWithShadowCast(pos.x, pos.y, _radius,
                                                (x, y) => _map._tiles[x, y].Transparent == false,
                                                (x, y) => { tileList.Add(new Vector2Int(x, y)); });

        for (int i = 0; i < tileList.Count; i++)
        {
            int x = tileList[i].x;
            int y = tileList[i].y;

            _map._tiles[x, y].Visible = true;
            _map._tiles[x, y].Explored = true;
            _map._tiles[x, y].SetVisible();
        }

        List<Monster> monsters = GameMng.CharMng.GetMonsters();
        for (int i = 0; i < monsters.Count; i++)
            monsters[i].VisibleChange(tileList);

        List<Item> items = GameMng.CharMng.GetItems();
        for (int i = 0; i < GameMng.CharMng.GetItems().Count; i++)
            items[i].VisibleChange(tileList);
    }

    private void OnTouch(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.End)
            return;

        if (!GameMng.Camera.IsActive && !EventSystem.current.IsPointerOverGameObject())
        {
            // 입력이 들어왔슴으로 기존에 있던 Task는 모두 지워라.
            TemporaryStorage.Clear();

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile curr = FindCurrTile();
            Tile target = FindTargetTile(pos);

            if (target != null && curr != null && !(curr == target) && (target.Visible || target.Explored))
            {
                TileType type = target.TILETYPE;

                if (target.TileTypeStack.Count > 0)
                {
                    if (target.TILETYPE != TileType.Monster)
                        type = target.TileTypeStack.Pop();
                }

                if (IsThrow)
                {
                    GameMng.Task.CurrentExecute(new TaskThrow(target, gameObject, _map, SelectedSlot));
                    IsThrow = false;
                    SelectedSlot = null;
                    return;
                }
                if (IsShoot)
                {
                    Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                    target = Helper.RayCastWithBresenham(start, target.Position, _map);

                    if (target == FindCurrTile())
                    {
                        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 스스로를 겨냥할 수 없다.");
                        return;
                    }

                    GameMng.Task.CurrentExecute(new TaskShoot(target, gameObject, _map, SelectedSlot));
                    IsShoot = false;
                    SelectedSlot = null;
                    return;
                }
                if (type == TileType.Floor || type == TileType.FrontDoor || type == TileType.SideDoor)
                    Move(curr, target);
                if (type == TileType.Monster)
                    Attack(curr, target);
                if (type == TileType.Item)
                    Item(curr, target);
                if (type == TileType.DownStairway || type == TileType.UpStairway)
                    Translate(curr, target);
            }
        }
    }

    private void Translate(Tile start, Tile target)
    {
        List<Tile> path = GameMng.Map.PathFinding(start, target);

        if (path != null && path.Count > 0)
            for (int i = 0; i < path.Count; i++)
                TemporaryStorage.Enqueue(new TaskMove(path[i], gameObject));

        TemporaryStorage.Enqueue(new TaskTranslate(_map, target, gameObject));
        GameMng.Task.TaskRegister(TemporaryStorage.Dequeue());
        GameMng.Task.Execute();
    }

    private void Attack(Tile start, Tile target)
    {
        List<Tile> path = GameMng.Map.PathFinding(start, target);

        if (path != null && path.Count > 0)
            for (int i = 0; i < path.Count - 1; i++)
                TemporaryStorage.Enqueue(new TaskMove(path[i], gameObject));

        SelectTile = target;

        TemporaryStorage.Enqueue(new TaskAttack(target, gameObject,_status.GetAp(), _status.Weapon._itemSound));

        GameMng.Task.TaskRegister(TemporaryStorage.Dequeue());
        GameMng.Task.Execute();
    }

    private void Move(Tile start, Tile target)
    {
        List<Tile> path = GameMng.Map.PathFinding(start, target);

        if (path != null && path.Count > 0)
            for (int i = 0; i < path.Count; i++)
                TemporaryStorage.Enqueue(new TaskMove(path[i], gameObject));

        GameMng.Task.TaskRegister(TemporaryStorage.Dequeue());
        GameMng.Task.Execute();
    }

    private void Item(Tile start, Tile target)
    {
        List<Tile> path = GameMng.Map.PathFinding(start, target);

        if (path != null && path.Count > 0)
            for (int i = 0; i < path.Count; i++)
                TemporaryStorage.Enqueue(new TaskMove(path[i], gameObject));

        TemporaryStorage.Enqueue(new TaskPickUp(target, gameObject));
        GameMng.Task.TaskRegister(TemporaryStorage.Dequeue());
        GameMng.Task.Execute();
    }

    private float CalculataDir(Vector2 curr, Vector2 target)
    {
        Vector2 delta = curr - target;

        if (delta.x < 0)
            return -1;

        return 1;
    }

    public override void Damage(int value)
    {
        _status.Damage(value);
        Helper.BlinkCount(transform, 3);
        _animator.SetTrigger("Damage");
    }

    public override void MagicDamage(int value)
    {
        _status.MagicDamage(value);
        _animator.SetTrigger("Damage");
    }

    public void GetEXP(int value)
    {
        _status.GetEXP(value);
    }

    public HeroStatus GetStatus()
    {
        if (_status != null)
            return _status;
        return null;
    }

    public void TileTypeChange()
    {
        if (_currTile != null)
        {
            _prevTile = _currTile;
            if (_currTile.TileTypeStack.Count > 0)
                _prevTile.TILETYPE = _currTile.TileTypeStack.Pop();
            else
                _prevTile.TILETYPE = _currTile.ORIGINTYPE;
        }

        _currTile = FindCurrTile();
        //_currTile.ORIGINTYPE = _currTile.TILETYPE;
        if (_currTile != null)
            _currTile.TILETYPE = TileType.Hero;
    }

    public void Idle()
    {
        _animator.SetBool("Move", false);
    }

    private void DeathEffect()
    {
        StartCoroutine(IERIP());
    }

    private IEnumerator IERIP()
    {
        GameObject rip = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/RIP"), transform.position, Quaternion.identity, transform);
        Vector3 ripPos = rip.transform.position;

        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * 2;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            rip.transform.position = Vector3.Lerp(ripPos, ripPos + new Vector3(0, 0.25f, 0), interpolation);
            yield return null;
        }
    }
}