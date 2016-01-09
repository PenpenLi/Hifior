using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace RPGEditor
{
    /// <summary>
    /// 编辑Tile属性的窗口
    /// </summary>
    public class MapTilePropertyWindow : EditorWindow
    {
        public static int SelectedTileIndex
        {
            get
            {
                return CurrentX + CurrentY * MapTilePropertyWindow.TILESET_W;
            }
        }
        public static bool IsShowing
        {
            get;
            private set;
        }
        /// <summary>
        /// Path后面必须要加个 /
        /// </summary>
        public const string MAP_FILEPATH = "Assets/RPG Data/Map";
        public const string MAP_DATANAME = "TileProperty";
        public const string TILESET_FILEPATH = "Assets/Editor/Res/TileSet/";
        public const int TILESIZE = 32;
        public const int TILESET_W = 10;
        public static int CurrentSelectedTileID;
        public static Texture2D CurrentTexture
        {
            get { return TileTextures[CurrentSelectedTileID]; }
        }

        public readonly static List<Texture2D> TileTextures = new List<Texture2D>();
        private static MapTileDef TileDef;
        private static Vector2 scrollVector = Vector2.zero;
        private static string[] tileSeriesNames;
        private static int tileSeriesCount = 0;
        private static bool[] tileEnableTable;
        private static int CurrentX;
        private static int CurrentY;
        public static List<TileAttribute> TileDataDef
        {
            get
            {
                if (TileDef != null)
                    return TileDef.TileProperty;
                return CreateTileData().TileProperty;
            }
        }
        [MenuItem("RPGEditor/Map Editor", false)]
        public static void OpenDatabaseEditor()
        {
            TileDef = CreateTileData();
            MapTilePropertyWindow mapEditor = EditorWindow.GetWindow<MapTilePropertyWindow>();
            mapEditor.Show();
            IsShowing = true;
        }
        /// <summary>
        /// 判定是否有地图属性文件存在
        /// </summary>
        public static MapTileDef CreateTileData()
        {
            string absolutePath = MAP_FILEPATH + "/" + MAP_DATANAME + ".asset";
            if (!ScriptableObjectUtility.FileExists(absolutePath))
            {
                MapTileDef tileDef = ScriptableObjectUtility.CreateAsset<MapTileDef>(
                    MAP_DATANAME,
                    MAP_FILEPATH,
                    true
                );
                return tileDef;
            }
            return AssetDatabase.LoadAssetAtPath(absolutePath, typeof(MapTileDef)) as MapTileDef;
        }

        public void Awake()
        {
            tileSeriesNames = System.Enum.GetNames(typeof(EnumCareerSeries));
            tileSeriesCount = tileSeriesNames.Length;
            tileEnableTable = EnumTables.GetTrueArray(tileSeriesCount);

            int count = LoadTileTexture();
            int CountDif = count - TileDef.TileProperty.Count;
            if (TileDef.TileProperty.Count < count)
            {
                for (int i = 0; i < CountDif; i++)
                    TileDef.TileProperty.Add(new TileAttribute());
            }
            AssetDatabase.SaveAssets();
        }
        public static int LoadTileTexture()
        {
            string[] textFileNames = ScriptableObjectUtility.GetFiles(TILESET_FILEPATH, "png");
            int count = textFileNames.Length;
            if (count != TileTextures.Count)
            {
                TileTextures.Clear();
                for (int i = 0; i < count; i++)
                {
                    TileTextures.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(textFileNames[i]));
                }
            }
            return count;
        }
        void OnGUI()
        {
            scrollVector = EditorGUILayout.BeginScrollView(scrollVector, false, true, GUILayout.Width(340), GUILayout.Height(320));
            //GUI.Box(new Rect(10, 10, 200, 200), "图集");
            int xCount = TileTextures.Count % TILESET_W;
            int yCount = TileTextures.Count / TILESET_W;
            for (int i = 0; i < yCount; i++)
            {
                for (int j = 0; j < TILESET_W; j++)
                {
                    if (GUI.Button(new Rect(j * TILESIZE, i * TILESIZE, TILESIZE, TILESIZE), TileTextures[i * TILESET_W + j], RPGEditorGUI.RichLabelStyle))
                    {
                        CurrentX = j;
                        CurrentY = i;
                    }
                }
            }
            for (int j = 0; j < xCount; j++)
            {
                if (GUI.Button(new Rect(j * TILESIZE, yCount * TILESIZE, TILESIZE, TILESIZE), TileTextures[yCount * TILESET_W + j]))
                {
                    CurrentX = j;
                    CurrentY = yCount;
                }
            }

            GUILayout.Space(30);
            EditorGUILayout.EndScrollView();
            //EditorGUI.DrawTextureTransparent(new Rect(10, 10, 42, 42), TileTextures[0], ScaleMode.ScaleToFit);

            //文本框显示鼠标在窗口的位置

            //EditorGUILayout.LabelField("鼠标在窗口的位置", Event.current.mousePosition.ToString());

            //选择贴图
            //           texture = EditorGUILayout.ObjectField("添加贴图", texture, typeof(Texture), true) as Texture;
            RefreshTilePropertyEditor(CurrentX, CurrentY);

        }
        void RefreshTilePropertyEditor(int x, int y)
        {
            int id = y * TILESET_W + x;
            CurrentSelectedTileID = id;
            TileAttribute tile = TileDef.TileProperty[id];
            if (tile == null)
            {
                Debug.LogError("没有改Tile对应的数据");
                return;
            }
            tile.CommonProperty.ID = id;

            GUILayout.BeginArea(new Rect(350, 0, 300, 600));

            EditorGUILayout.LabelField("ID:" + tile.CommonProperty.ID);
            tile.CommonProperty.Name = EditorGUILayout.TextField("名称", tile.CommonProperty.Name);
            tile.Avoid = EditorGUILayout.IntSlider("回避", tile.Avoid, 0, 10);

            tile.PhysicalDefense = EditorGUILayout.IntSlider("防御", tile.PhysicalDefense, 0, 10);
            tile.MagicalDefense = EditorGUILayout.IntSlider("魔防", tile.MagicalDefense, 0, 10);

            tile.Recover = EditorGUILayout.IntSlider("恢复比例", tile.Recover, 0, 100);
            tile.BattleBackgroundID = EditorGUILayout.IntPopup("战斗背景", tile.BattleBackgroundID, new string[] { "草原", "冰川", "草屋" }, EnumTables.GetSequentialArray(3)); ;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("移动消耗");
            GUILayout.Space(20);
            if (GUILayout.Button("全部不可通行", GUILayout.Width(80)))
            {
                for (int i = 0; i < tileEnableTable.Length; i++)
                    tileEnableTable[i] = false;
            }
            if (GUILayout.Button("全部可以通行", GUILayout.Width(80)))
            {
                for (int i = 0; i < tileEnableTable.Length; i++)
                {
                    tileEnableTable[i] = true;
                    tile.MovementConsume[i] = 1;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < tileSeriesCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                tileEnableTable[i] = EditorGUILayout.Toggle(tileSeriesNames[i], tileEnableTable[i]);
                if (tileEnableTable[i])
                {
                    if (tile.MovementConsume[i] < 1)
                        tile.MovementConsume[i] = 1;
                    tile.MovementConsume[i] = EditorGUILayout.IntSlider(tile.MovementConsume[i], 1, 8);
                }
                else
                {
                    tile.MovementConsume[i] = 100;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.HelpBox("通行方向，蓝底背景表示可以从这个方向通过这个块", MessageType.None);

            EditorGUILayout.BeginVertical(RPGEditorGUI.BoxStyle, GUILayout.Width(100));

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(RPGEditorGUI.Icon_Arrow2_Up, tile.PassUp ? RPGEditorGUI.BoxStyle : RPGEditorGUI.RichLabelStyle, GUILayout.Width(32), GUILayout.Height(32)))
                    tile.PassUp = !tile.PassUp;
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(RPGEditorGUI.Icon_Arrow3_Left, tile.PassLeft ? RPGEditorGUI.BoxStyle : RPGEditorGUI.RichLabelStyle, GUILayout.Width(32), GUILayout.Height(32)))
                tile.PassLeft = !tile.PassLeft;
            EditorGUILayout.Space();
            if (GUILayout.Button(RPGEditorGUI.Icon_Arrow3_Right, tile.PassRight ? RPGEditorGUI.BoxStyle : RPGEditorGUI.RichLabelStyle, GUILayout.Width(32), GUILayout.Height(32)))
                tile.PassRight = !tile.PassRight;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(RPGEditorGUI.Icon_Arrow2_Down, tile.PassDown ? RPGEditorGUI.BoxStyle : RPGEditorGUI.RichLabelStyle, GUILayout.Width(32), GUILayout.Height(32)))
                    tile.PassDown = !tile.PassDown;
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.EndArea();
        }

        void OnInspectorUpdate()
        {
            this.Repaint();
        }
        void OnFocus()
        {
            if (TileDef == null)
                TileDef = CreateTileData();

            IsShowing = true;
        }
        void OnDestroy()
        {
            AssetDatabase.SaveAssets();
            IsShowing = false;
        }

    }
}
