using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(SLGChapter))]
/// <summary>
/// 每个地图的Terrain里都附加这个脚本
/// </summary>
public class SLGMap : MonoBehaviour
{
    public const float CELLSIZE = 10f;
    public const float HEIGHTOFFSET = 2f;

    private Texture2D gSprite_Arrow1;
    private Texture2D gSprite_Arrow2;
    private Texture2D gSprite_Arrow3;
    [Header("SLGMap")]
    public RPGCharacter TestCharacter;
    /*
     public GameObject gSprite_Attackable;
     public GameObject gSprite_Moveable;
     public GameObject gSprite_MouseArea;
     public GameObject gSprite_Companion;
     public GameObject gSprite_SkillPlayer;
     public GameObject gSprite_SkillEnemy;
     public GameObject gSprite_SkillBoth;
     public GameObject rootMoveParent;
     public GameObject rootAttackParent;
     public GameObject rootRoutineParent;
     public GameObject rootCompanionParent;*/

#if UNITY_EDITOR

    /// <summary>
    /// 计算网格的高度到数组中
    /// </summary>
    void ComputeHeight()
    {
        _heights = new float[(TileWidth + 1), (TileHeight + 1)];
        RaycastHit hitInfo;
        Vector3 origin;
        int terrainLayerMask = LayerMask.GetMask("Terrain");
        for (int z = 0; z < TileHeight + 1; z++)
        {
            for (int x = 0; x < TileWidth + 1; x++)
            {
                origin = new Vector3(x * CELLSIZE, 200, z * CELLSIZE);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, terrainLayerMask);

                _heights[x, z] = hitInfo.point.y;
            }
        }
        float[,] CenterHeightData = new float[TileWidth, TileHeight];
        for (int i = 0; i < TileHeight; i++)
        {
            for (int j = 0; j < TileWidth; j++)
            {
                CenterHeightData[j, i] = Mathf.Max(_heights[j, i], _heights[j + 1, i], _heights[j, i + 1], _heights[j + 1, i + 1]);
            }
        }
        MapTileData.InitMapData(TileWidth, TileHeight, CenterHeightData, new int[TileWidth, TileHeight]);
    }
    [ContextMenu("生成Tile网格")]
    void AddChild()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        ComputeHeight();
        for (int y = 0; y < TileHeight; y++)
        {
            for (int x = 0; x < TileWidth; x++)
            {
                GameObject g = Instantiate<GameObject>(TilePrefab.gameObject);
                g.layer = 0;
                g.isStatic = true;
                g.name = x + "," + y;
                g.transform.SetParent(transform);
                g.transform.localPosition = new Vector3(x * CELLSIZE, HEIGHTOFFSET, y * CELLSIZE);
                PositionGrid pg = g.GetComponent<PositionGrid>();
                pg.Init(CELLSIZE, x, y);
                g.AddComponent<TileType>();
                DestroyImmediate(pg);
            }
        }
    }

    public PositionGrid TilePrefab;
    private float[,] _heights;
    public float[,] Heights
    {
        get { return _heights; }
    }
#endif
    [Header("地图信息")]
    public BattleMapData MapTileData;
    public int TileWidth = 30;//地图x
    public int TileHeight = 20;//地图y
    /// <summary>
    /// 前一个图块类型
    /// </summary>
    private int[,] Tile_type_prev;
    /// <summary>
    /// 图块占用
    /// </summary>
    private EnumOccupyStatus[,] Tile_occupy;
    [Header("图块坐标详情")]
    [SerializeField]
    private Vector2Int CharacterCenter;//角色坐标
    [SerializeField]
    private Vector2Int MouseTileXY;
    [SerializeField]
    private List<Vector2Int> PList;//Plist里记录的是真实可以移动到达的区域部分
    private Dictionary<Vector2Int, Vector2Int> _FootData;//存储指定坐标处的剩余路径
    private Dictionary<Vector2Int, int> _TempFootData = new Dictionary<Vector2Int, int>();//int表示剩余的移动范围消耗点
    [SerializeField]
    private List<Vector2Int> _AttackRangeData = new List<Vector2Int>();//int表示剩余的攻击范围消耗点
    [SerializeField]
    private List<Vector2Int> _MoveRoute = new List<Vector2Int>();//存储移动路径
    [Header("人物信息")]
    [SerializeField]
    private int _ItemRangeMin;//所有可用装备的最小范围
    [SerializeField]
    private int _ItemRangeMax; //所有可用装备的最大范围
    [SerializeField]
    private int _Mov; //移动力
    private int _Career;
    private EnumWeaponRangeType _WeaponRangeType;

    public AStarNode[,] AStarGrid;

    private UnityEvent FuncMoveStart = new UnityEvent();
    private UnityEvent FuncMoveEnd = new UnityEvent();

    /*
     攻击范围的判断，在消耗点用完的那个最终节点进行遍历，如果遍历的坐标包括在_FootData移动数据里，则不进行红色显示，
     如果不存在，则进行红色的范围显示，将其存入directory，其也包含消耗攻击范围为两个整数（1-1，1-2法师，2-2弓箭手，2-3长弓）
     遍历时将其加上起始的第一个值，后面依此加一，遍历次数为后一个范围减去前一个
     */
    /* public static int[,] MapDataArray = new int[18, 14]{//每一行为列
         {15,15,15,0,0,0,0,0,0,0,0,0,0,0},
         {15,15,15,0,0,0,0,0,0,0,0,0,0,0},
         {15,15,15,0,0,0,0,0,0,0,0,0,0,0},
         {0,15,15,15,15,15,15,15,15,15,15,0,0,0},
         {0,15,15,15,15,15,15,15,15,15,15,0,0,0},
         {0,15,2,15,15,15,15,15,15,15,15,0,0,0},
         {15,15,2,15,15,15,15,15,15,15,15,0,0,0},
         {15,15,15,15,15,15,0,15,15,15,15,0,0,0},
         {15,15,15,15,15,15,0,15,15,15,15,0,0,0},
         {0,0,0,0,0,0,0,15,15,15,15,15,15,15},//10
         {0,0,0,0,0,0,0,15,15,15,15,15,15,15},
         {0,0,0,0,0,0,0,15,15,15,15,15,15,15},
         {0,0,0,0,0,0,0,15,15,15,0,0,0,15},
         {0,15,15,15,15,15,15,15,15,0,0,0,15,15},
         {0,15,15,15,15,15,15,15,15,0,0,0,15,15},
         {15,15,15,15,15,15,15,15,15,15,15,15,15,15},
         {0,0,0,0,15,15,15,15,15,15,15,15,15,15},
         {0,0,0,0,15,15,15,15,15,15,0,0,0,0},
     };
     public static int[,] MapHeightArray = new int[50, 50];*/
    /// <summary>
    /// 包含己方单位的可用坐标
    /// </summary>
    private bool[,] bMoveAcessList;
    /// <summary>
    /// 排除了己方单位的可用坐标,也就是未被占用的坐标
    /// </summary>
    private bool[,] bMoveAcessListExcluded;
    private bool bPlayer;
    private MeshRenderer[] tileMeshRenders;
    [Header("范围数据")]
    [SerializeField]
    private List<Vector2Int> m_AttackRangeList = new List<Vector2Int>();
    [SerializeField]
    private List<Vector2Int> m_MoveRangeList = new List<Vector2Int>();
    [SerializeField]
    private List<Vector2Int> m_SkillSelectRangeList = new List<Vector2Int>();
    [SerializeField]
    private List<Vector2Int> m_SkillEffectRangeList = new List<Vector2Int>();
    [SerializeField]
    private List<Vector2Int> m_CompanionRange = new List<Vector2Int>();
    ////////////////////////////////////////////////////////////////////////////////

    void Awake()
    {
        Utils.MiscUtil.SetChildActive(transform, false);

        gSprite_Arrow1 = Resources.Load<Texture2D>(Utils.TextUtil.GetResourcesFullPath("arrow1", "App", "Map"));
        gSprite_Arrow2 = Resources.Load<Texture2D>(Utils.TextUtil.GetResourcesFullPath("arrow2", "App", "Map"));
        gSprite_Arrow3 = Resources.Load<Texture2D>(Utils.TextUtil.GetResourcesFullPath("arrow3", "App", "Map"));

        tileMeshRenders = new MeshRenderer[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            tileMeshRenders[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }

        PList = new List<Vector2Int>();
        _FootData = new Dictionary<Vector2Int, Vector2Int>();
        Tile_type_prev = new int[TileWidth, TileHeight];
        Tile_occupy = new EnumOccupyStatus[TileWidth, TileHeight];
        bMoveAcessList = new bool[TileWidth, TileHeight];
        bMoveAcessListExcluded = new bool[TileWidth, TileHeight];
        AStarGrid = new AStarNode[TileWidth, TileHeight];
    }

    public float GetTileHeight(int x, int y)
    {
        return MapTileData.GetTileData(x, y).Height;
    }
    public T GetTileComponent<T>(int x, int y) where T : Component
    {
        return tileMeshRenders[y * TileWidth + x].GetComponent<T>();
    }
    public MeshRenderer GetTileMeshRender(int x, int y)
    {
        return tileMeshRenders[y * TileWidth + x];
    }
    public void SetTileActive(int x, int y, bool active)
    {
        GetTileMeshRender(x, y).gameObject.SetActive(active);
    }
    private void SetTileMaterialTexture(int x, int y, Texture2D texture = null, float rotation = 0.0f)
    {
        Material mat = GetTileMeshRender(x, y).material;
        mat.SetTexture("_Texture", texture);
        mat.SetFloat("_RotationDegrees", Mathf.Deg2Rad * rotation);
    }
    private void SetTextureRotation(int x, int y, float rotation)
    {
        GetTileMeshRender(x, y).material.SetFloat("_RotationDegrees", Mathf.Deg2Rad * rotation);
    }
    public void Clear()
    {
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        CharacterCenter.x = CharacterCenter.y = 0;//角色坐标
    }
    private void ResetMoveAcessList()
    {
        PList.Clear();
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        for (int i = 0; i < TileWidth; i++)
            for (int j = 0; j < TileHeight; j++)
            {
                {
                    bMoveAcessList[i, j] = false;
                    bMoveAcessListExcluded[i, j] = false;
                }
            }
    }

    #region 图块占用处理函数
    public void SetTileEnemyOccupied(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            Tile_occupy[x, y] = EnumOccupyStatus.Enemy;
    }
    public void SetTilePlayerOccupied(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            Tile_occupy[x, y] = EnumOccupyStatus.Player;
    }
    public void ResetTileOccupyStatus(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            Tile_occupy[x, y] = EnumOccupyStatus.None;
    }
    public bool IsOccupyByEnemy(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return Tile_occupy[x, y] == EnumOccupyStatus.Enemy;
        return false;
    }
    public bool IsOccupyByPlayer(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return Tile_occupy[x, y] == EnumOccupyStatus.Player;
        return false;
    }
    public bool IsOccupyByNone(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return Tile_occupy[x, y] == EnumOccupyStatus.None;
        return false;
    }
    public EnumOccupyStatus GetTileOccupyStatus(int x, int y)
    {
        if (IsEffectivelyCoordinateWithWarning(x, y))
            return Tile_occupy[x, y];
        return EnumOccupyStatus.None;
    }
    private bool IsOccupiedBySameParty(int x, int y)
    {
        if (Tile_occupy[x, y] == EnumOccupyStatus.None)
            return false;
        if (bPlayer)
        {
            return (Tile_occupy[x, y] == EnumOccupyStatus.Player);
        }
        else
        {
            return (Tile_occupy[x, y] == EnumOccupyStatus.Enemy);
        }
    }
    private bool IsOccupiedByDiffentParty(int x, int y)
    {
        if (Tile_occupy[x, y] == EnumOccupyStatus.None)
            return false;
        if (bPlayer)
        {
            return (Tile_occupy[x, y] == EnumOccupyStatus.Enemy);
        }
        else
        {
            return (Tile_occupy[x, y] == EnumOccupyStatus.Player);
        }
    }
    #endregion
    public void InitActionScope(RPGCharacter Gamechar, bool Show = true)
    {
        InitActionScope(Gamechar, Gamechar.GetMovement(), Show);
    }
    public void InitActionScope(RPGCharacter Gamechar, int Movement, bool Show = true)
    {
        HideAllRange();
        bPlayer = (Gamechar.GetCamp() == EnumCharacterCamp.Player);//0,2为我方的单位

        _Mov = Movement;
        /* if (Gamechar.SkillGroup.isHaveStaticSkill(18))//探险家，无视地形，将其职业设为天马
             _Job = 15;//medifyneed
         if (Gamechar.SkillGroup.isHaveStaticSkill(19))
             _Mov += 2;*/
        _Career = Gamechar.GetCareer();
        CharacterCenter = new Vector2Int(Gamechar.GetTileCoord().x, Gamechar.GetTileCoord().x);

        ResetMoveAcessList();
        bMoveAcessList[CharacterCenter.x, CharacterCenter.y] = true;
        bMoveAcessListExcluded[CharacterCenter.x, CharacterCenter.y] = true;
        _FootData.Add(CharacterCenter, new Vector2Int(-1, -1));
        _TempFootData.Add(CharacterCenter, _Mov);

        int countPoint = 0;

        while (countPoint < _Mov)
        {
            _FindDistance(Gamechar.GetCareer(), _Mov);//递归查询距离   _FindDistance(Table._JobTable.getBranch(gamechar.attribute.Job), _Mov, 0, 0);
            countPoint++;
        }

        WeaponItem item = Gamechar.Item.GetEquipWeapon();
        item = new WeaponItem(1);
        if (item != null)//装备武器不为空
        {
            _AttackRangeData.Clear();
            _ItemRangeMin = item.GetDefinition().RangeType.MinSelectRange;
            _ItemRangeMax = item.GetDefinition().RangeType.MaxSelectRange;
            _WeaponRangeType = item.GetDefinition().RangeType.RangeType;
            if (_ItemRangeMax != 0 && _ItemRangeMax - _ItemRangeMin > -1)//武器有距离
            {
                for (int i = 0; i < PList.Count; i++)//遍历可移动的区域
                {
                    AttackScan(PList[i]);//递归查询距离   _FindDistance(Table._JobTable.getBranch(gamechar.attribute.Job), _Mov, 0, 0);
                }
            }
        }

        if (Show)
        {
            ShowMoveableRange();
            ShowAttackableRange();
        }
    }

    private void _FindDistance(int job, int movement)
    {
        List<Vector2Int> buffer = new List<Vector2Int>(_TempFootData.Keys);
        foreach (Vector2Int key in buffer)
        {
            DirectionScan(key, new Vector2Int(key.x, key.y - 1), _TempFootData[key]); //北
            DirectionScan(key, new Vector2Int(key.x, key.y + 1), _TempFootData[key]); //南
            DirectionScan(key, new Vector2Int(key.x - 1, key.y), _TempFootData[key]); //西
            DirectionScan(key, new Vector2Int(key.x + 1, key.y), _TempFootData[key]); //东
        }
    }

    private void DirectionScan(Vector2Int lastcord, Vector2Int cord, int surplusConsum)
    {
        if (IsEffectivelyCoordinate(cord))
        {
            int value = surplusConsum - GetMapPassValue(_Career, cord.x, cord.y);//该坐标处剩余可行步数
            if (value >= 0)
            {
                if (!bMoveAcessList[cord.x, cord.y])
                {
                    bMoveAcessList[cord.x, cord.y] = true;
                    if (!IsOccupiedBySameParty(cord.x, cord.y))//被相同方占用此处可以继续查找，但是不可以到达此处
                    {
                        PList.Add(new Vector2Int(cord.x, cord.y));
                        bMoveAcessListExcluded[cord.x, cord.y] = true;
                    }
                    _TempFootData.Add(cord, value);
                    _FootData.Add(cord, lastcord);
                }
                else
                {
                    if (value > _TempFootData[cord])
                    { //各方向到达同一地点，只取最小机动力消耗
                        _TempFootData[cord] = value;//更新最小消耗
                        _FootData[cord] = lastcord;//更新当前节点最小消耗的父节点
                    }
                }
            }
        }
    }

    private void AttackScan(Vector2Int MoveablePoint)//输入的是需要遍历的边缘路径,攻击最小和最大范围
    {
        //查找四个方向的可用攻击范围坐标
        int x = MoveablePoint.x;
        int y = MoveablePoint.y;
        int left = (x - _ItemRangeMax) < 0 ? 0 : x - _ItemRangeMax;
        int right = (x + _ItemRangeMax) > TileWidth - 1 ? TileWidth - 1 : x + _ItemRangeMax;
        int up = (y - _ItemRangeMax < 0) ? 0 : y - _ItemRangeMax;
        int bottom = (y + _ItemRangeMax) > TileHeight - 1 ? TileHeight - 1 : y + _ItemRangeMax;
        if (_WeaponRangeType == EnumWeaponRangeType.菱形菱形)
        {
            for (int i = left; i <= right; i++)
            {
                for (int j = up; j <= bottom; j++)
                {
                    if (bMoveAcessListExcluded[i, j])
                        continue;
                    int absLen = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                        continue;
                    else
                    {
                        bMoveAcessList[i, j] = true;
                        Vector2Int p = new Vector2Int(i, j);
                        _AttackRangeData.Add(p);
                    }
                }
            }
        }
        if (_WeaponRangeType == EnumWeaponRangeType.十字形)//为1则是只能上下左右寻找目标
        {
            for (int i = left; i <= right; i++)//得到x轴上所有的范围
            {
                if (bMoveAcessListExcluded[i, y])
                    continue;
                int absLen = Mathf.Abs(i - x);
                if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                    continue;
                else
                {
                    bMoveAcessList[i, y] = true;
                    Vector2Int p = new Vector2Int(i, y);
                    _AttackRangeData.Add(p);
                }

            }
            for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
            {
                if (bMoveAcessListExcluded[x, i])
                    continue;
                int absLen = Mathf.Abs(i - y);
                if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                    continue;
                else
                {
                    bMoveAcessList[x, i] = true;
                    Vector2Int p = new Vector2Int(x, i);
                    _AttackRangeData.Add(p);
                }
            }
        }
    }
    #region 范围显示
    public void ShowRange(List<Vector2Int> Range, Color color)
    {
        foreach (Vector2Int p in Range)
        {
            ShowRange(p, color);
        }
    }
    public void ShowRange(Vector2Int Point, Color color)
    {
        MeshRenderer mr = GetTileMeshRender(Point.x, Point.y);
        mr.material.SetColor("_Color", color);
        mr.material.SetTexture("_Texture", null);
        mr.gameObject.SetActive(true);
    }
    public void ShowMoveableRange()
    {
        ShowRange(PList, new Color(0, 0.36f, 1f, 0.5f));
        m_MoveRangeList.AddRange(PList);
    }

    public void ShowAttackableRange()
    {
        ShowRange(_AttackRangeData, new Color(1f, 90f / 255f, 0f, 0.5f));
        m_AttackRangeList.AddRange(_AttackRangeData);
    }

    public void ShowCompanionSprite(List<Vector2Int> Points)
    {
        ShowRange(Points, new Color(0.2f, 0.8f, 1, 0.5f));
        m_CompanionRange.AddRange(Points);
    }

    public void ShowSkillPlayer(List<Vector2Int> Points)
    {
        ShowRange(Points, Color.green);
        m_SkillSelectRangeList.AddRange(Points);
    }
    public void ShowSkillEnemy(List<Vector2Int> Points)
    {
        ShowRange(Points, Color.red);
        m_SkillSelectRangeList.AddRange(Points);
    }
    public void ShowSkillBoth(List<Vector2Int> Points)
    {
        ShowRange(Points, Color.black);
        m_SkillSelectRangeList.AddRange(Points);
    }

    public void ShowSkillEffectRange(List<Vector2Int> selectPoints, List<Vector2Int> effectPoints, int effectType)
    {
        HideSkillEffect();

        for (int i = 0; i < selectPoints.Count; i++)
        {
            m_SkillSelectRangeList.Add(selectPoints[i]);
        }
        if (effectType == 0)
        {
            ShowSkillPlayer(effectPoints);
        }
        if (effectType == 1)
        {
            ShowSkillEnemy(effectPoints);
        }
        if (effectType == 2)
        {
            ShowSkillEnemy(effectPoints);
        }

        for (int i = 0; i < effectPoints.Count; i++)
        {
            m_SkillSelectRangeList.Add(selectPoints[i]);
        }
    }

    public void HideAllRange()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void HideRoutine()
    {
        foreach (Vector2Int child in _MoveRoute)
        {
            SetTileMaterialTexture(child.x, child.y, null);
        }
        _MoveRoute.Clear();
    }
    public void HideMoveAttackRange()
    {
        HideAttackRange();
        HideMoveRange();
    }
    public void HideAttackRange()
    {
        foreach (Vector2Int p in m_AttackRangeList)
        {
            SetTileActive(p.x, p.y, false);
        }
        m_AttackRangeList.Clear();
    }
    public void HideMoveRange()
    {
        foreach (Vector2Int p in m_MoveRangeList)
        {
            SetTileActive(p.x, p.y, false);
        }
        m_MoveRangeList.Clear();
    }
    /// <summary>
    /// 隐藏选择同伴的图块
    /// </summary>
    public void HideCompanionRange()
    {
        foreach (Vector2Int p in m_CompanionRange)
        {
            SetTileActive(p.x, p.y, false);
        }
        m_CompanionRange.Clear();
    }
    public void HideSkillSelect()
    {
        foreach (Vector2Int p in m_SkillSelectRangeList)
        {
            SetTileActive(p.x, p.y, false);
        }
        m_SkillSelectRangeList.Clear();
    }
    public void HideSkillEffect()
    {
        foreach (Vector2Int p in m_SkillEffectRangeList)
        {
            SetTileActive(p.x, p.y, false);
        }
        m_SkillEffectRangeList.Clear();
    }
    /// <summary>
    /// 是否可以移动到这里
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool CanMoveTo(Vector2Int point)
    {
        return _FootData.ContainsKey(point);
    }
    #endregion
    public bool ShowMoveRoutine(int x, int y)
    {
        HideRoutine();
        Vector2Int point = new Vector2Int(x, y);
        if (CanMoveTo(point))
        {
            buildMoveRoutine(x, y);
            if (_MoveRoute.Count < 1)
                return false;
            if (_MoveRoute.Count == 1)
                return true;
            int lastDir;
            int curDir;
            curDir = GetDirByPoint(_MoveRoute[0], _MoveRoute[1]);
            if (_MoveRoute.Count == 2)//如果是相邻的
            {
                float zRotation = 0.0f;
                if (curDir == 0)
                    zRotation = 0.0f;
                if (curDir == 1)
                    zRotation = 180.0f;
                if (curDir == 2)
                    zRotation = 90.0f;
                if (curDir == 3)
                    zRotation = 270.0f;

                SetTileMaterialTexture(_MoveRoute[1].x, _MoveRoute[1].y, gSprite_Arrow1, zRotation);
            }
            else
            {
                lastDir = curDir;
                for (int i = 1; i < _MoveRoute.Count - 1; i++)
                {
                    int j = i + 1;
                    curDir = GetDirByPoint(_MoveRoute[i], _MoveRoute[j]);
                    float zRotation = 0.0f;
                    if (curDir == lastDir)//方向相同--绘制箭头3，
                    {
                        if (curDir < 2)//是上下，
                        {
                            zRotation = 90.0f;
                        }
                        else//是左右
                        {
                            zRotation = 0.0f;
                        }
                        SetTileMaterialTexture(_MoveRoute[i].x, _MoveRoute[i].y, gSprite_Arrow3, zRotation);
                    }
                    else//方向不相同绘制九十度弯度箭头2
                    {
                        if ((lastDir == 3 && curDir == 0) || lastDir == 1 && curDir == 2)
                        {
                            zRotation = 0.0f;
                        }
                        if ((lastDir == 3 && curDir == 1) || lastDir == 0 && curDir == 2)
                        {
                            zRotation = 90.0f;
                        }
                        if ((lastDir == 0 && curDir == 3) || lastDir == 2 && curDir == 1)
                        {
                            zRotation = 180.0f;
                        }
                        if ((lastDir == 1 && curDir == 3) || lastDir == 2 && curDir == 0)
                        {
                            zRotation = 270.0f;
                        }
                        SetTileMaterialTexture(_MoveRoute[i].x, _MoveRoute[i].y, gSprite_Arrow2, zRotation);
                    }
                    lastDir = curDir;
                }
                //再绘制最后一个箭头
                int last = _MoveRoute.Count - 1;
                curDir = GetDirByPoint(_MoveRoute[last - 1], _MoveRoute[last]);
                float z = 0.0f;
                if (curDir == 0)
                    z = 0.0f;
                if (curDir == 1)
                    z = 180.0f;
                if (curDir == 2)
                    z = 90.0f;
                if (curDir == 3)
                    z = 270.0f;

                SetTileMaterialTexture(_MoveRoute[last].x, _MoveRoute[last].y, gSprite_Arrow1, z);
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// 0上，1下，2左，3右
    /// </summary>
    /// <param name="pFirst"></param>
    /// <param name="pSecond"></param>
    /// <returns></returns>
    private int GetDirByPoint(Vector2Int pFirst, Vector2Int pSecond)
    {
        int dir;
        if (pSecond.x == pFirst.x)
        {
            if (pSecond.y > pFirst.y) //向上
                dir = 0;
            else
                dir = 1;
        }
        else
        {
            if (pSecond.x > pFirst.x) //向右
                dir = 3;
            else
                dir = 2;
        }
        return dir;
    }

    public bool MoveWithOutShowRoutine(RPGCharacter Gamechar, int x, int y, UnityAction OnMoveFinish)
    {
        Gamechar.SetTileCoord(x, y, false);
        Vector2Int point = new Vector2Int(x, y);
        if (_FootData.ContainsKey(point))
        {
            buildMoveRoutine(x, y);
            Move(Gamechar, null, OnMoveFinish);
            return true;
        }
        return false;
    }

    private void buildMoveRoutine(int x, int y)//将存储在_FootData中的节点转换为实际移动路径
    {
        _MoveRoute.Clear();
        Vector2Int parentPoint = new Vector2Int(x, y);
        while (parentPoint != new Vector2Int(-1, -1)) //当父节点存在时，不存在时为Point2D(-1,-1);
        {
            _MoveRoute.Add(parentPoint);
            parentPoint = _FootData[parentPoint];
        }
        _MoveRoute.Reverse();
    }
    private void iTweenGameCharMove(RPGCharacter Gamechar, Vector3[] paths, UnityAction callStart, UnityAction callEnd)
    {
        FuncMoveStart.RemoveAllListeners();
        FuncMoveEnd.RemoveAllListeners();
        FuncMoveStart.AddListener(callStart);
        FuncMoveEnd.AddListener(callEnd);
        Hashtable args = new Hashtable();
        //设置路径的点
        args.Add("path", paths);
        //设置类型为线性，线性效果会好一些。
        args.Add("easeType", iTween.EaseType.linear);
        //设置寻路的速度
        args.Add("speed", 15f);
        //是否先从原始位置走到路径中第一个点的位置
        args.Add("movetopath", true);
        //是否让模型始终面朝当面目标的方向，拐弯的地方会自动旋转模型
        //如果你发现你的模型在寻路的时候始终都是一个方向那么一定要打开这个
        args.Add("orienttopath", true);

        args.Add("onstart", "OnMoveStart");
        args.Add("onstartparams", Gamechar);
        args.Add("onstarttarget", gameObject);
        //设置接受方法的对象，默认是自身接受，这里也可以改成别的对象接受，
        //那么就得在接收对象的脚本中实现AnimationStart方法。
        args.Add("oncomplete", "OnMoveEnd");
        args.Add("oncompleteparams", Gamechar);
        args.Add("oncompletetarget", gameObject);
        iTween.MoveTo(Gamechar.gameObject, args);
    }
    /// <summary>
    /// 按照当前的路径进行移动
    /// </summary>
    /// <param name="Gamechar"></param>
    /// <param name="callStart"></param>
    /// <param name="callEnd"></param>
    public void Move(RPGCharacter Gamechar, UnityAction callStart = null, UnityAction callEnd = null) //MoveRoutine的路径 移动
    {
        if (_MoveRoute != null)
        {
            if (_MoveRoute.Count > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[_MoveRoute.Count];
                int c = 0;
                for (int i = 0; i < _MoveRoute.Count; i++)//将动画数组复制到空间坐标
                {
                    paths[c] = new Vector3(_MoveRoute[i].x, _MoveRoute[i].y, 0);
                    c++;
                }
                iTweenGameCharMove(Gamechar, paths, callStart, callEnd);
            }
            else
            {
                OnMoveEnd(Gamechar);
            }
        }
    }
    /// <summary>
    /// 用于敌方行动回合移动到指定位置并攻击
    /// </summary>
    public void MoveThenAttack(RPGCharacter Gamechar, int x, int y, UnityAction callStart = null, UnityAction callEnd = null)  //直接移动到指定的地点,用于自动攻击的单位
    {
        Gamechar.SetTileCoord(x, y, false);
        Vector2Int point = new Vector2Int(x, y);
        if (_FootData.ContainsKey(point))
        {
            buildMoveRoutine(x, y);
        }
        if (_MoveRoute != null)
        {
            if (_MoveRoute.Count > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[_MoveRoute.Count];
                int c = 0;
                for (int i = 0; i < _MoveRoute.Count; i++)//将动画数组复制到空间坐标
                {
                    //paths[c] = Vector2Int.Vector2IntToVector3(_MoveRoute[i].x, _MoveRoute[i].y, true);
                    c++;
                }
                iTweenGameCharMove(Gamechar, paths, callStart, callEnd);
            }
            else
            {
                OnMoveEnd(Gamechar);
            }
        }
    }
    public void MoveByRoutine(RPGCharacter Gamechar, Vector2Int[] p, UnityAction callStart = null, UnityAction callEnd = null)
    {
        Gamechar.SetTileCoord(p[p.Length - 1].x, p[p.Length - 1].y, true);
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        _MoveRoute.Clear();
        //HideMouseArrow();

        if (p != null)
        {
            if (p.Length > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[p.Length];
                int c = 0;
                for (int i = 0; i < p.Length; i++)//将动画数组复制到空间坐标
                {
                    //paths[c] = Vector2Int.Vector2IntToVector3(p[i].x, p[i].y, true);
                    c++;
                }
                iTweenGameCharMove(Gamechar, paths, callStart, callEnd);
            }
        }
    }
    #region 移动后处理
    void OnMoveStart(RPGCharacter Gamechar)
    {
        if (FuncMoveStart != null && FuncMoveStart.GetPersistentEventCount() > 0)
            FuncMoveStart.Invoke();
    }
    void OnMoveEnd(RPGCharacter Gamechar) //移动结束后
    {
        if (FuncMoveEnd != null)
            FuncMoveEnd.Invoke();
        Clear();
    }
    #endregion
    #region A星寻路
    public void MoveToCroods(RPGCharacter Gamechar, int x, int y)
    {
        int startX = Gamechar.GetTileCoord().x;
        int startY = Gamechar.GetTileCoord().y;
        if (x == startX && y == startY)
            return;
        bPlayer = (Gamechar.GetCamp() == EnumCharacterCamp.Player);//0,2为我方的单位
        if (!IsEffectivelyCoordinate(x, y))//超出地图范围
            return;
        //被敌方单位占领则不可通过，被我方单位占领则可以通过，不被占领则当PassValue为小于10时可以通过
        bool bWalkable = false;

        for (int i = 0; i < TileWidth; i++)
        {
            for (int j = 0; j < TileHeight; j++)
            {
                if (GetMapPassValue(Gamechar.GetCareer(), i, j) > 10 || IsOccupiedByDiffentParty(i, j))
                    bWalkable = false;
                else if (IsOccupiedBySameParty(i, j))
                    bWalkable = true;
                else
                    bWalkable = true;
                AStarGrid[i, j] = new AStarNode(bWalkable, i, j);
            }
        }
        AStarNode startNode = AStarGrid[startX, startY];
        AStarNode endNode = AStarGrid[x, y];
        List<AStarNode> openSet = new List<AStarNode>();
        HashSet<AStarNode> closeSet = new HashSet<AStarNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            AStarNode currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);
            if (currentNode._gridX == endNode._gridX && currentNode._gridY == endNode._gridY)
            {
                GeneratePath(startNode, endNode);
                MoveByRoutine(Gamechar, _MoveRoute.ToArray());
                return;
            }
            foreach (AStarNode node in GetNeibourhood(currentNode))
            {
                if (!node._canWalk || closeSet.Contains(node))
                    continue;
                int newCost = currentNode.gCost + GetDistanceNode(currentNode, node);
                if (newCost < node.gCost || !openSet.Contains(node))
                {
                    node.gCost = newCost;
                    node.hCost = GetDistanceNode(node, endNode);
                    node.parent = currentNode;
                    if (!openSet.Contains(node))
                        openSet.Add(node);
                }
            }
        }
    }
    private void GeneratePath(AStarNode startNode, AStarNode endNode)
    {
        _MoveRoute.Clear();
        AStarNode temp = endNode;
        while (temp != startNode)
        {
            _MoveRoute.Add(new Vector2Int(temp._gridX, temp._gridY));
            temp = temp.parent;
        }
        _MoveRoute.Reverse();
    }
    public int GetDistanceNode(AStarNode a, AStarNode b)
    {
        int cntX = Mathf.Abs(a._gridX - b._gridX);
        int cntY = Mathf.Abs(a._gridY - b._gridY);
        if (cntX > cntY)
        {
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else
        {
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

    public List<AStarNode> GetNeibourhood(AStarNode node)
    {
        List<AStarNode> neibourhood = new List<AStarNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                int tempX = node._gridX + i;
                int tempY = node._gridY + j;
                if (IsEffectivelyCoordinate(tempX, tempY))
                {
                    neibourhood.Add(AStarGrid[tempX, tempY]);
                }
            }
        }
        return neibourhood;
    }
    #endregion
    public void StopMoveImmediate()
    {
        iTween.Stop();
    }

    private bool IsEffectivelyCoordinate(Vector2Int p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < TileWidth && p.y < TileHeight ? true : false;
    }
    private bool IsEffectivelyCoordinateWithWarning(int x, int y)
    {
        bool effective = x >= 0 && y >= 0 && x < TileWidth && y < TileHeight;
        if (effective)
        {
            return true;
        }
        else
        {
            Debug.LogError("输入的坐标不是有效的坐标" + new Vector2Int(x, y));
            return false;
        }
    }
    private bool IsEffectivelyCoordinate(int x, int y)
    {
        return x >= 0 && y >= 0 && x < TileWidth && y < TileHeight;
    }
    public int GetMapPassValue(int job, int x, int y)//得到此处的人物通过消耗
    {
        if (IsOccupiedByDiffentParty(x, y))//图块被敌方占用，则我方不可通过,敌方按正常计算
        {
            return 100;
        }
        else
            return ResourceManager.GetMapDef().GetMovementConsume(MapTileData.GetTileData(x, y).Type, job);
    }
    public int GetMapPhysicalDefenseValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).Type;
        return ResourceManager.GetMapDef().GetPhysicalDefense(mapData);
    }
    public int GetMapMagicalDefenseValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).Type;
        return ResourceManager.GetMapDef().GetMagicalDefense(mapData);
    }
    public int GetMapAvoidValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).Type;
        return ResourceManager.GetMapDef().GetAvoid(mapData);
    }

    public List<Vector2Int> FindAttackRangeWithoutShow(RPGCharacter Gamechar)//查找所有的武器所能攻击的范围
    {
        _AttackRangeData.Clear();
        int x = Gamechar.GetTileCoord().x;
        int y = Gamechar.GetTileCoord().y;
        List<WeaponItem> items = Gamechar.Item.GetAttackWeapon();
        foreach (WeaponItem item in items)
        {
            _ItemRangeMax = item.GetDefinition().RangeType.MaxSelectRange;
            _ItemRangeMin = item.GetDefinition().RangeType.MinSelectRange;
            EnumWeaponRangeType rangeType = item.GetDefinition().RangeType.RangeType;

            int left = (x - _ItemRangeMax) < 0 ? 0 : x - _ItemRangeMax;
            int right = (x + _ItemRangeMax) > TileWidth - 1 ? TileWidth - 1 : x + _ItemRangeMax;
            int up = (y - _ItemRangeMax < 0) ? 0 : y - _ItemRangeMax;
            int bottom = (y + _ItemRangeMax) > TileHeight - 1 ? TileHeight - 1 : y + _ItemRangeMax;
            if (rangeType == EnumWeaponRangeType.菱形菱形)
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = up; j <= bottom; j++)
                    {
                        int absLen = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                        if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                            continue;
                        Vector2Int p = new Vector2Int(i, j);
                        if (_AttackRangeData.Contains(p))
                            continue;
                        _AttackRangeData.Add(p);
                    }
                }
            }
            if (rangeType == EnumWeaponRangeType.十字形)//为1则是只能上下左右寻找目标
            {
                for (int i = left; i <= right; i++)//得到x轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - x);

                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                        continue;
                    Vector2Int p = new Vector2Int(i, y);
                    if (_AttackRangeData.Contains(p))
                        continue;
                    _AttackRangeData.Add(p);

                }
                for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - y);

                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                        continue;
                    Vector2Int p = new Vector2Int(x, i);
                    if (_AttackRangeData.Contains(p))
                        continue;
                    _AttackRangeData.Add(p);
                }
            }
        }
        return _AttackRangeData;

    }

    public List<Vector2Int> FindAttackRange(int x, int y, WeaponDef EquipItem, bool bShow = true)//指定的坐标
    {
        _AttackRangeData.Clear();
        _ItemRangeMax = EquipItem.RangeType.MaxSelectRange;
        _ItemRangeMin = EquipItem.RangeType.MinSelectRange;
        EnumWeaponRangeType rangeType = EquipItem.RangeType.RangeType;

        if (_ItemRangeMin > 1)
        {
            int left = (x - _ItemRangeMax) < 0 ? 0 : x - _ItemRangeMax;
            int right = (x + _ItemRangeMax) > TileWidth - 1 ? TileWidth - 1 : x + _ItemRangeMax;
            int up = (y - _ItemRangeMax < 0) ? 0 : y - _ItemRangeMax;
            int bottom = (y + _ItemRangeMax) > TileHeight - 1 ? TileHeight - 1 : y + _ItemRangeMax;
            if (rangeType == 0)
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = up; j <= bottom; j++)
                    {
                        int absLen = Mathf.Abs(i - x) + Mathf.Abs(j - y);
                        if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Vector2Int(i, j)))
                            continue;
                        _AttackRangeData.Add(new Vector2Int(i, j));
                    }
                }
            }
            if (rangeType == EnumWeaponRangeType.十字形)//为1则是只能上下左右寻找目标
            {
                for (int i = left; i <= right; i++)//得到x轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - x);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Vector2Int(i, y)))
                        continue;
                    _AttackRangeData.Add(new Vector2Int(i, y));

                }
                for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - y);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Vector2Int(x, i)))
                        continue;
                    _AttackRangeData.Add(new Vector2Int(x, i));
                }
            }
            if (rangeType == EnumWeaponRangeType.正方形)//为2矩形攻击范围
            {
                for (int i = left; i <= right; i++)
                {
                    for (int j = up; j <= bottom; j++)
                    {
                        int absX = Mathf.Abs(i - x);
                        int absY = Mathf.Abs(j - y);
                        if (absX < _ItemRangeMin && absY < _ItemRangeMin)//在其中xy均小于最小坐标的不符合，直接进行下一个循环
                            continue;
                        _AttackRangeData.Add(new Vector2Int(i, j));
                    }
                }
            }
            /* if (rangeType == 3)//全屏攻击。放到外面单独处理，暂时用不到
             {
                 for (int i = 0; i < mapTileX; i++)
                 {
                     for (int j = 0; j < mapTileX; j++)
                     {
                         _AttackRangeData.Add(new Point2D(i, j));
                     }
                 }
             }*/
        }
        else //如果是最小攻击距离从1开始
        {

            for (int i = -_ItemRangeMax; i <= _ItemRangeMax; i++)
            {
                for (int j = -_ItemRangeMax; j <= _ItemRangeMax; j++)
                {
                    switch (rangeType)
                    {
                        case EnumWeaponRangeType.菱形菱形:
                            if (Math.Abs(i) + Math.Abs(j) > _ItemRangeMax) { continue; }
                            break;
                        case EnumWeaponRangeType.十字形:
                            if (Math.Abs(i) != 0 && Math.Abs(j) != 0) { continue; }
                            break;
                        case EnumWeaponRangeType.正方形:
                            break;
                    }
                    _AttackRangeData.Add(new Vector2Int(i + x, j + y));
                }
            }
            _AttackRangeData.Remove(new Vector2Int(x, y));
        }
        if (bShow)
        {
            ShowAttackableRange();
        }
        return _AttackRangeData;
    }
    public bool[,] GetMoveAcessArray()
    {
        return bMoveAcessList;
    }
    public List<Vector2Int> GetAttackRangeData()
    {
        return _AttackRangeData;
    }
    /// <summary>
    /// Tile的宽度-1
    /// </summary>
    public int MapTileX
    {
        get
        {
            return this.TileWidth - 1;
        }
    }
    /// <summary>
    /// Tile的高度-1
    /// </summary>
    public int MapTileY
    {
        get
        {
            return this.TileHeight - 1;
        }
    }
    public bool IsCoordsAccessable(int x, int y)//此处是否可以到达
    {
        return bMoveAcessList[x, y];
    }
    public void ResetSelectSprite()
    {
        /*foreach (Transform child in rootAttackParent.transform)
        {
            child.gameObject.SetActive(true);
        }*/
    }
    public List<Vector2Int> GetRealMoveableTiles()
    {
        return PList;
    }
}
