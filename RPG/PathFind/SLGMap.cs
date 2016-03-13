using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SLGMap : MonoBehaviour
{
    public const float CELLSIZE = 10f;
    public const float HEIGHTOFFSET = 2f;

    public RPGCharacter TestCharacter;
    /*
     public GameObject gSprite_Attackable;
     public GameObject gSprite_Moveable;
     public GameObject gSprite_MouseArea;
     public GameObject gSprite_Companion;
     public GameObject gSprite_SkillPlayer;
     public GameObject gSprite_SkillEnemy;
     public GameObject gSprite_SkillBoth;
     public GameObject gSprite_Arrow1;
     public GameObject gSprite_Arrow2;
     public GameObject gSprite_Arrow3;
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
        _heights = new float[(TileHeight + 1), (TileWidth + 1)];
        RaycastHit hitInfo;
        Vector3 origin;
        int terrainLayerMask = LayerMask.GetMask("Terrain");
        for (int z = 0; z < TileHeight + 1; z++)
        {
            for (int x = 0; x < TileWidth + 1; x++)
            {
                origin = new Vector3(x * cellSize, 200, z * cellSize);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, terrainLayerMask);

                _heights[x, z] = hitInfo.point.y;
            }
        }
        MapTileData.InitMapData(TileWidth, TileHeight, new float[TileWidth, TileHeight], new int[TileWidth, TileHeight]);
    }
    [ContextMenu("添加子物体")]
    void AddChild()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        ComputeHeight();
        for (int i = 0; i < TileHeight; i++)
        {
            for (int j = 0; j < TileWidth; j++)
            {
                GameObject g = Instantiate<GameObject>(TilePrefab.gameObject);
                g.layer = 0;
                g.isStatic = true;
                g.name = i + "," + j;
                g.transform.SetParent(transform);
                g.transform.localPosition = new Vector3(i * CELLSIZE, HEIGHTOFFSET, j * CELLSIZE);
                PositionGrid pg = g.GetComponent<PositionGrid>();
                pg.Init(CELLSIZE, i, j);
                DestroyImmediate(pg);
            }
        }
    }
    public PositionGrid TilePrefab;
    public float cellSize = 10f;
    public float yOffset = 2f;
    private float[,] _heights;
    public float[,] Heights
    {
        get { return _heights; }
    }
#endif
    [SerializeField]
    private Point2D mouseTileXY;
    [SerializeField]
    private List<Point2D> PList;//Plist里记录的是真实可以移动到达的区域部分
    private Dictionary<Point2D, Point2D> _FootData;//存储指定坐标处的剩余路径
    private Dictionary<Point2D, int> _TempFootData = new Dictionary<Point2D, int>();//int表示剩余的移动范围消耗点
    [SerializeField]
    private List<Point2D> _AttackRangeData = new List<Point2D>();//int表示剩余的攻击范围消耗点
    [SerializeField]
    private Point2D charCenter;//角色坐标
    [SerializeField]
    private List<Point2D> _moveRoute = new List<Point2D>();//存储移动路径
    [SerializeField]
    private int _ItemRangeMin;//所有可用装备的最小范围
    [SerializeField]
    private int _ItemRangeMax; //所有可用装备的最大范围
    [SerializeField]
    private int _Mov; //移动力
    private int _Career;
    private EnumWeaponRangeType _WeaponRangeType;
    public BattleMapData MapTileData;
    public int TileWidth = 30;//地图x
    public int TileHeight = 20;//地图y

    public AStarNode[,] grid;

    UnityEvent FuncMoveStart;
    UnityEvent FuncMoveEnd;

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
    private bool[,] bMoveAcessList;//包含己方单位的可用坐标
    private bool[,] bMoveAcessListExcluded;//排除了己方单位的可用坐标
    private bool bPlayer;

    private GameObject[,] _SelectSkillRangeGameObjects;
    void Awake()
    {
        PList = new List<Point2D>();
        _FootData = new Dictionary<Point2D, Point2D>();
        bMoveAcessList = new bool[MapTileX, MapTileY];
        bMoveAcessListExcluded = new bool[MapTileX, MapTileY];
        grid = new AStarNode[TileWidth, MapTileY];

        _SelectSkillRangeGameObjects = new GameObject[MapTileX, MapTileY];
    }
    void Start()
    {
        HideAllRange();
        InitActionScope(TestCharacter, 5);
    }

    public void Clear()
    {
        Clear_SpriteMoving();
        Clear_SpriteRoutine();
        Clear_SpriteAttack();
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        charCenter.x = charCenter.y = 0;//角色坐标
    }
    private void ResetMoveAcessList()
    {
        PList.Clear();
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        for (int i = 0; i < MapTileX; i++)
            for (int j = 0; j < MapTileY; j++)
            {
                {
                    bMoveAcessList[i, j] = false;
                    bMoveAcessListExcluded[i, j] = false;
                }
            }
    }
    /*
    public void HideMouseArrow()
    {
        gSprite_MouseArea.transform.position = new Vector3(-1000f, 100f, 100f);
    }
    public void SetArrowCroods(int x, int y)
    {
        gSprite_MouseArea.transform.position = Point2D.Point2DToVector3(x, y);
    }

    public bool DrawMouseArea(Point2D mouseTileXY, int state)//state=0 判定是否在FootData中 ，在显示移动范围时使用，=1在选择攻击范围时使用
    {
        if (state == 0) //移动路线
        {
            if (_FootData.ContainsKey(mouseTileXY))//如果坐标不在FootData，则不显示
            {
                gSprite_MouseArea.transform.position = Point2D.Point2DToVector3(mouseTileXY.x, mouseTileXY.y, 0.015f);
                return true;
            }
            else
            {
                HideMouseArrow();
            }
        }
        if (state == 1) //攻击范围
        {
            if (_AttackRangeData.Contains(mouseTileXY))
            {
                gSprite_MouseArea.transform.position = Point2D.Point2DToVector3(mouseTileXY.x, mouseTileXY.y, 0.015f);
                return true;
            }
            else
            {
                HideMouseArrow();
            }
        }
        return false;
    }
    public bool DrawMouseArea(List<Point2D> companionCoordsList, Point2D p)//state=0 判定是否在FootData中 ，在显示移动范围时使用，=1在选择攻击范围时使用
    {
        if (companionCoordsList.Contains(p))
        {
            gSprite_MouseArea.transform.position = Point2D.Point2DToVector3(p.x, p.y, 0.015f);
            return true;
        }
        else
        {
            HideMouseArrow();
        }
        return false;
    }*/
    public void SetTileEnemyOccupied(int x, int y)
    {
        MapTileData.GetTileData(x, y).OccupyEnemy();
    }
    public void SetTilePlayerOccupied(int x, int y)
    {
        MapTileData.GetTileData(x, y).OccupyPlayer();
    }
    public void ResetTileOccupyState(int x, int y)
    {
        MapTileData.GetTileData(x, y).OccupyNone();
    }
    public void InitActionScope(RPGCharacter Gamechar, int Mov, bool Show = true)
    {
        bPlayer = (Gamechar.GetCamp() == EnumCharacterCamp.Player);//0,2为我方的单位

        _Mov = Mov;
        /* if (Gamechar.SkillGroup.isHaveStaticSkill(18))//探险家，无视地形，将其职业设为天马
             _Job = 15;//medifyneed
         if (Gamechar.SkillGroup.isHaveStaticSkill(19))
             _Mov += 2;*/
        _Career = Gamechar.GetCareer();
        charCenter = Gamechar.GetTileCoord();

        ResetMoveAcessList();
        bMoveAcessList[charCenter.x, charCenter.y] = true;
        bMoveAcessListExcluded[charCenter.x, charCenter.y] = true;
        _FootData.Add(charCenter, new Point2D(-1, -1));
        _TempFootData.Add(charCenter, _Mov);

        int countPoint = 0;

        while (countPoint < _Mov)
        {
            _FindDistance(Gamechar.GetCareer(), _Mov);//递归查询距离   _FindDistance(Table._JobTable.getBranch(gamechar.attribute.Job), _Mov, 0, 0);
            countPoint++;
        }

        WeaponItem item = Gamechar.Item.GetEquipItem();
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
            ShowRange(PList, new Color(0, 1, 1, 0.5f));
            ShowRange(_AttackRangeData, new Color(233f / 255f, 150f / 255f, 122f / 255f, 0.5f));
        }
    }

    private void _FindDistance(int job, int movement)
    {
        List<Point2D> buffer = new List<Point2D>(_TempFootData.Keys);
        foreach (Point2D key in buffer)
        {
            DirectionScan(key, new Point2D(key.x, key.y - 1), _TempFootData[key]); //北
            DirectionScan(key, new Point2D(key.x, key.y + 1), _TempFootData[key]); //南
            DirectionScan(key, new Point2D(key.x - 1, key.y), _TempFootData[key]); //西
            DirectionScan(key, new Point2D(key.x + 1, key.y), _TempFootData[key]); //东
        }
    }

    private void DirectionScan(Point2D lastcord, Point2D cord, int surplusConsum)
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
                        PList.Add(new Point2D(cord.x, cord.y));
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

    private void AttackScan(Point2D MoveablePoint)//输入的是需要遍历的边缘路径,攻击最小和最大范围
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
                        Point2D p = new Point2D(i, j);
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
                    Point2D p = new Point2D(i, y);
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
                    Point2D p = new Point2D(x, i);
                    _AttackRangeData.Add(p);
                }
            }
        }
    }
    #region 范围显示
    public void ShowRange(List<Point2D> Range, Color color)
    {
        foreach (Point2D Key in Range)
        {
            Transform t = transform.GetChild(Key.x * TileHeight + Key.y);
            t.GetComponent<MeshRenderer>().material.color = color;
            t.gameObject.SetActive(true);
        }
    }

    public void ShowMoveableRange()
    {
        ShowRange(PList, new Color(0, 0, 1, 0.5f));
    }

    public void ShowAttackableRange()
    {
        ShowRange(_AttackRangeData, new Color(233f / 255f, 150f / 255f, 122f / 255f, 0.5f));
    }

    public void ShowCompanionSprite(Point2D p)
    {
        Transform t = transform.GetChild(p.x * TileHeight + p.y);
        t.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 1, 0.5f);
        t.gameObject.SetActive(true);
    }
    public void ShowSkillPlayer(List<Point2D> Points)
    {

    }
    public void ShowSkillEnemy(List<Point2D> Points)
    {

    }
    public void SHowSkillBoth(List<Point2D> Points)
    {

    }
    public void ShowSkillSelectRange(List<Point2D> points)
    {
        foreach (Point2D Key in points)
        {

        }
        //DrawGrid(gSprite_Attackable,rootAttackParent,points );
    }
    public void ShowSkillEffectRange(List<Point2D> selectPoints, List<Point2D> effectPoints, int effectType)
    {
        Clear_SpriteSkillEffect();

        for (int i = 0; i < selectPoints.Count; i++)
        {
            GameObject obj = _SelectSkillRangeGameObjects[selectPoints[i].x, selectPoints[i].y];
            if (obj != null)
            {
                obj.SetActive(true);
            }
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
            GameObject obj = _SelectSkillRangeGameObjects[effectPoints[i].x, effectPoints[i].y];
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void HideAllRange()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void Clear_SpriteRoutine()
    {
        /* foreach (Transform child in rootRoutineParent.transform)
         {
             Destroy(child.gameObject);
         }
         _moveRoute.Clear();*/
    }
    public void Clear_SpriteAttack()
    {
        /*foreach (Transform child in rootAttackParent.transform)
        {
            Destroy(child.gameObject);
        }
        _AttackRangeData.Clear();*/
    }
    public void Clear_SpriteMoving()
    {
        /* foreach (Transform child in rootMoveParent.transform)
         {
             Destroy(child.gameObject);
         }
         _moveRoute.Clear();*/
    }
    public void Clear_SpriteCompanion()
    {
        /* foreach (Transform child in rootCompanionParent.transform)
         {
             Destroy(child.gameObject);
         }*/
    }
    public void Clear_SpriteSkillSelect()
    {
        /* foreach (Transform child in rootAttackParent.transform)
         {
             Destroy(child.gameObject);
         }*/
    }
    public void Clear_SpriteSkillEffect()
    {
        /*  foreach (Transform child in rootMoveParent.transform)
          {
              Destroy(child.gameObject);
          }*/
    }
    #endregion
    public bool ShowMoveRoutine(int x, int y)
    {
        Point2D point = new Point2D(x, y);
        if (_FootData.ContainsKey(point))
        {
            buildMoveRoutine(x, y);
            if (_moveRoute.Count < 1)
                return false;
            if (_moveRoute.Count == 1)
                return true;
            int lastDir;
            int curDir;
            curDir = getDirByPoint(_moveRoute[0], _moveRoute[1]);
            if (_moveRoute.Count == 2)//如果是相邻的
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
                Vector3 ve = Point2D.Point2DToVector3(_moveRoute[1].x, _moveRoute[1].y);
                ve.y += 0.011f;
                Quaternion q = Quaternion.Euler(90.0f, 0.0f, zRotation);
                //GameObject obj = Instantiate(gSprite_Arrow1, ve, q) as GameObject;//绘制可移动范围和可攻击范围      
                //obj.transform.parent = rootRoutineParent.transform;
            }
            else
            {
                lastDir = curDir;
                for (int i = 1; i < _moveRoute.Count - 1; i++)
                {
                    int j = i + 1;
                    curDir = getDirByPoint(_moveRoute[i], _moveRoute[j]);
                    Quaternion q;
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
                        Vector3 ve = Point2D.Point2DToVector3(_moveRoute[i].x, _moveRoute[i].y);
                        ve.y += 0.011f;

                        q = Quaternion.Euler(90.0f, 0.0f, zRotation);
                        // GameObject obj = Instantiate(gSprite_Arrow3, ve, q) as GameObject;//绘制可移动范围和可攻击范围      
                        // obj.transform.parent = rootRoutineParent.transform;
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
                        Vector3 ve = Point2D.Point2DToVector3(_moveRoute[i].x, _moveRoute[i].y);
                        ve.y += 0.011f;
                        q = Quaternion.Euler(90.0f, 0.0f, zRotation);
                        // GameObject obj = Instantiate(gSprite_Arrow2, ve, q) as GameObject;//绘制可移动范围和可攻击范围      
                        // obj.transform.parent = rootRoutineParent.transform;
                    }
                    lastDir = curDir;
                }
                //再绘制最后一个箭头
                int last = _moveRoute.Count - 1;
                curDir = getDirByPoint(_moveRoute[last - 1], _moveRoute[last]);
                float z = 0.0f;
                if (curDir == 0)
                    z = 0.0f;
                if (curDir == 1)
                    z = 180.0f;
                if (curDir == 2)
                    z = 90.0f;
                if (curDir == 3)
                    z = 270.0f;
                Vector3 vec = Point2D.Point2DToVector3(_moveRoute[last].x, _moveRoute[last].y);
                vec.y += 0.011f;
                Quaternion qua = Quaternion.Euler(90.0f, 0.0f, z);
                // GameObject objf = Instantiate(gSprite_Arrow1, vec, qua) as GameObject;//绘制可移动范围和可攻击范围      
                // objf.transform.parent = rootRoutineParent.transform;
            }
            return true;
        }
        return false;
    }
    private int getDirByPoint(Point2D pFirst, Point2D pSecond)
    {
        int dir;//0上，1下，2左，3右
        if (pSecond.x == pFirst.x)
        {
            if (pSecond.y > pFirst.y) //向上
                dir = 1;
            else
                dir = 0;
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

    public bool MoveWithOutShowRoutine(RPGCharacter Gamechar, int x, int y)
    {
        Gamechar.SetTileCoord(x, y);
        Point2D point = new Point2D(x, y);
        if (_FootData.ContainsKey(point))
        {
            buildMoveRoutine(x, y);
            Move(Gamechar);
            return true;
        }
        return false;
    }

    private void buildMoveRoutine(int x, int y)//将存储在_FootData中的节点转换为实际移动路径
    {
        Point2D parentPoint = new Point2D(x, y);
        while (parentPoint.x != -1) //当父节点存在时，不存在时为Point2D(-1,-1);
        {
            _moveRoute.Add(parentPoint);
            parentPoint = _FootData[parentPoint];
        }
        _moveRoute.Reverse();
    }
    private void iTweenGameCharMove(RPGCharacter Gamechar, Vector3[] paths, UnityAction callStart, UnityAction callEnd)
    {
        FuncMoveStart.AddListener(callStart);
        FuncMoveEnd.AddListener(callEnd);
        Gamechar.Run();
        Hashtable args = new Hashtable();
        //设置路径的点
        args.Add("path", paths);
        //设置类型为线性，线性效果会好一些。
        args.Add("easeType", iTween.EaseType.linear);
        //设置寻路的速度
        args.Add("speed", 3.0f);
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
    public void Move(RPGCharacter Gamechar, UnityAction callStart = null, UnityAction callEnd = null) //MoveRoutine的路径 移动
    {
        if (_moveRoute != null)
        {
            if (_moveRoute.Count > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[_moveRoute.Count];
                int c = 0;
                for (int i = 0; i < _moveRoute.Count; i++)//将动画数组复制到空间坐标
                {
                    paths[c] = Point2D.Point2DToVector3(_moveRoute[i].x, _moveRoute[i].y);
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
        Gamechar.SetTileCoord(x, y);
        Point2D point = new Point2D(x, y);
        if (_FootData.ContainsKey(point))
        {
            buildMoveRoutine(x, y);
        }
        if (_moveRoute != null)
        {
            if (_moveRoute.Count > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[_moveRoute.Count];
                int c = 0;
                for (int i = 0; i < _moveRoute.Count; i++)//将动画数组复制到空间坐标
                {
                    paths[c] = Point2D.Point2DToVector3(_moveRoute[i].x, _moveRoute[i].y);
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
    public void MoveByRoutine(RPGCharacter Gamechar, Point2D[] p, UnityAction callStart = null, UnityAction callEnd = null)
    {
        Gamechar.SetTileCoord(p[p.Length - 1].x, p[p.Length - 1].y);
        _FootData.Clear();//存储指定坐标处的剩余路径
        _TempFootData.Clear();//int表示剩余的移动范围消耗点
        _AttackRangeData.Clear();//int表示剩余的攻击范围消耗点
        _moveRoute.Clear();
        //HideMouseArrow();

        if (p != null)
        {
            if (p.Length > 1)//如果在原地则不移动,否则进行移动
            {
                Vector3[] paths = new Vector3[p.Length];
                int c = 0;
                for (int i = 0; i < p.Length; i++)//将动画数组复制到空间坐标
                {
                    paths[c] = Point2D.Point2DToVector3(p[i].x, p[i].y);
                    c++;
                }
                iTweenGameCharMove(Gamechar, paths, callStart, callEnd);
            }
        }
    }
    #region 移动后处理
    void OnMoveStart(RPGCharacter Gamechar)
    {
        if (FuncMoveStart != null)
            FuncMoveStart.Invoke();
        FuncMoveStart = null;
        Gamechar.Run();
        //Gamechar.PlayAnimation(5);
    }
    void OnMoveEnd(RPGCharacter Gamechar) //移动结束后
    {
        if (FuncMoveEnd != null)
            FuncMoveEnd.Invoke();
        FuncMoveEnd = null;
        Gamechar.StopRun();
        Clear();
        //Gamechar.PlayAnimation(0);//动画切换回来idle
        //SLGLevel.SLG.checkWinMission(Gamechar.TileCoords.x, Gamechar.TileCoords.y);//检查是否符合到达地点胜利条件
    }
    #endregion
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
                grid[i, j] = new AStarNode(bWalkable, i, j);
            }
        }
        AStarNode startNode = grid[startX, startY];
        AStarNode endNode = grid[x, y];
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
                MoveByRoutine(Gamechar, _moveRoute.ToArray());
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
        _moveRoute.Clear();
        AStarNode temp = endNode;
        while (temp != startNode)
        {
            _moveRoute.Add(new Point2D(temp._gridX, temp._gridY));
            temp = temp.parent;
        }
        _moveRoute.Reverse();
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
                    neibourhood.Add(grid[tempX, tempY]);
                }
            }
        }
        return neibourhood;
    }


    public void StopMoveImmediate()
    {
        iTween.Stop();
    }
    private bool IsOccupiedBySameParty(int x, int y)
    {
        if (bPlayer)
        {
            if (MapTileData.GetTileData(x, y).IsOccupiedByPlayer())
                return true;
        }
        else
        {
            if (MapTileData.GetTileData(x, y).IsOccupiedByEnemy())
                return true;
        }
        return false;
    }
    private bool IsOccupiedByDiffentParty(int x, int y)
    {
        if (bPlayer)
        {
            if (MapTileData.GetTileData(x, y).IsOccupiedByEnemy())
                return true;
        }
        else
        {
            if (MapTileData.GetTileData(x, y).IsOccupiedByPlayer())
                return true;
        }
        return false;
    }
    private bool IsEffectivelyCoordinate(Point2D p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < TileWidth && p.y < TileHeight ? true : false;
    }
    private bool IsEffectivelyCoordinate(int x, int y)
    {
        return x >= 0 && y >= 0 && x < TileWidth && y < TileHeight ? true : false;
    }

    public int GetMapPassValue(int job, int x, int y)//得到此处的人物通过消耗
    {
        if (IsOccupiedByDiffentParty(x, y))//图块被敌方占用，则我方不可通过,敌方按正常计算
        {
            return 100;
        }
        else
            return ResourceManager.GetMapDef().GetMovementConsume(MapTileData.GetTileData(x, y).GetTileType(), job);
    }
    public int GetMapPhysicalDefenseValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).GetTileType();
        return ResourceManager.GetMapDef().GetPhysicalDefense(mapData);
    }
    public int GetMapMagicalDefenseValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).GetTileType();
        return ResourceManager.GetMapDef().GetMagicalDefense(mapData);
    }
    public int GetMapAvoidValue(int x, int y)//得到此处的人物通过消耗
    {
        int mapData = MapTileData.GetTileData(x, y).GetTileType();
        return ResourceManager.GetMapDef().GetAvoid(mapData);
    }

    public List<Point2D> FindAttackRangeWithoutShow(RPGCharacter Gamechar)//查找所有的武器所能攻击的范围
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
                        Point2D p = new Point2D(i, j);
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
                    Point2D p = new Point2D(i, y);
                    if (_AttackRangeData.Contains(p))
                        continue;
                    _AttackRangeData.Add(p);

                }
                for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - y);

                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax)
                        continue;
                    Point2D p = new Point2D(x, i);
                    if (_AttackRangeData.Contains(p))
                        continue;
                    _AttackRangeData.Add(p);
                }
            }
        }
        return _AttackRangeData;

    }

    public void FindAttackRange(int x, int y, int itemID, bool bShow = true)//指定的坐标
    {
        _AttackRangeData.Clear();
        WeaponDef def = ResourceManager.GetWeaponDef(itemID);
        _ItemRangeMax = def.RangeType.MaxSelectRange;
        _ItemRangeMin = def.RangeType.MinSelectRange;
        EnumWeaponRangeType rangeType = def.RangeType.RangeType;

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
                        if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Point2D(i, j)))
                            continue;
                        _AttackRangeData.Add(new Point2D(i, j));
                    }
                }
            }
            if (rangeType == EnumWeaponRangeType.十字形)//为1则是只能上下左右寻找目标
            {
                for (int i = left; i <= right; i++)//得到x轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - x);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Point2D(i, y)))
                        continue;
                    _AttackRangeData.Add(new Point2D(i, y));

                }
                for (int i = up; i <= bottom; i++)//得到y轴上所有的范围
                {
                    int absLen = Mathf.Abs(i - y);
                    if (absLen < _ItemRangeMin || absLen > _ItemRangeMax || _AttackRangeData.Contains(new Point2D(x, i)))
                        continue;
                    _AttackRangeData.Add(new Point2D(x, i));
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
                        _AttackRangeData.Add(new Point2D(i, j));
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
                    _AttackRangeData.Add(new Point2D(i + x, j + y));
                }
            }
            _AttackRangeData.Remove(new Point2D(x, y));
        }
        if (bShow)
        {
            ShowAttackableRange();
        }
    }
    public bool[,] getMoveAcessArray()
    {
        return bMoveAcessList;
    }
    public List<Point2D> getAttackRangeData()
    {
        return _AttackRangeData;
    }

    public int MapTileX
    {
        get
        {
            return this.TileWidth;
        }
    }

    public int MapTileY
    {
        get
        {
            return this.TileHeight;
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
    public void DrawGrid(GameObject g, GameObject parent, List<Point2D> points)
    {
        foreach (Point2D Key in points)
        {
            Vector3 ve = Point2D.Point2DToVector3(Key.x, Key.y);
            ve.y += 0.01f;
            GameObject obj = Instantiate(g, ve, g.transform.rotation) as GameObject;//绘制可移动范围和可攻击范围
            obj.transform.parent = parent.transform;
        }
    }
}
