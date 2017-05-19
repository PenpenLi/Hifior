// ====================================================================================================================
// -== UniRPG ==-
// www.plyoung.com
// Copyright (c) 2013 by Leslie Young
// ====================================================================================================================

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RPGEditor
{

    public static class RPGEditorGUI
    {
        static RPGEditorGUI()
        {
            UseSkin();
        }
        // ================================================================================================================
        #region gui style/skin

        public const string EditorResourcePath = "Assets/Editor/Res/";

        public static GUISkin Skin = null;

        // colours
        public static Color DividerColor = new Color(0f, 0f, 0f, 0.25f);
        public static Color InspectorDividerColor = new Color(0.35f, 0.35f, 0.35f, 1f);
        public static Color ToolbarDividerColor = new Color(0f, 0.5f, 0.8f, 1f);
        public static Color ButtonOnColor = new Color(0.24f, 0.5f, 0.87f);

        // text & labels
        public static GUIStyle WarningLabelStyle;           // yellow text (in dark theme, else red)
        public static GUIStyle BoldLabelStyle;              // bold label
        public static GUIStyle RichLabelStyle;              // label that can parse html tags
        public static GUIStyle CenterLabelStyle;            // a centered label
        public static GUIStyle Head1Style;                  // big heading style
        public static GUIStyle Head2Style;                  // heading style
        public static GUIStyle Head3Style;                  // heading style
        public static GUIStyle Head4Style;                  // heading style
        public static GUIStyle LabelRightStyle;             // right-aligned label	
        public static GUIStyle InspectorHeadStyle;          // heading used in the inspector (similar in size to head3 but with different padding)
        public static GUIStyle InspectorHeadBoxStyle;       // a seperator like heading used in the inspector
        public static GUIStyle InspectorHeadFoldStyle;      // similar to InspectorHeadStyle, but has foldout icon
        public static GUIStyle HeadingFoldoutStyle;         // a foldout that has big text (like a heading style)

        // boxes & frames
        public static GUIStyle DividerStyle;
        public static GUIStyle BoxStyle;
        public static GUIStyle ScrollViewStyle;
        public static GUIStyle ScrollViewNoTLMarginStyle;
        public static GUIStyle ScrollViewNoMarginPaddingStyle;

        // buttons
        public static GUIStyle ButtonStyle;
        public static GUIStyle ButtonLeftStyle;
        public static GUIStyle ButtonMidStyle;
        public static GUIStyle ButtonRightStyle;
        public static GUIStyle LeftTabStyle;
        public static GUIStyle TinyButton;
        public static GUIStyle ToolbarStyle;
        public static GUIStyle ToolbarButtonLeftStyle;
        public static GUIStyle ToolbarButtonMidStyle;
        public static GUIStyle ToolbarButtonRightStyle;

        // fields
        public static GUIStyle DelTextFieldStyle;
        public static GUIStyle TextFieldStyle;                  // like EditorStyles.textField but with wordwrap=false

        // misc
        public static GUIStyle MenuBoxStyle;
        public static GUIStyle MenuItemStyle;
        public static GUIStyle MenuHeadStyle;
        public static GUIStyle ListItemBackDarkStyle;
        public static GUIStyle ListItemBackLightStyle;
        public static GUIStyle ListItemSelectedStyle;
        public static GUIStyle AboutLogoAreaStyle;

        // resources - icons
        public static Texture2D Icon_Tag;
        public static Texture2D Icon_Help;
        public static Texture2D Icon_Plus;
        public static Texture2D Icon_Minus;
        public static Texture2D Icon_Accept;
        public static Texture2D Icon_Cancel;
        public static Texture2D Icon_Refresh;
        public static Texture2D Icon_Exclaim_Red;
        public static Texture2D Icon_User;
        public static Texture2D Icon_Users;
        public static Texture2D Icon_UserPlus;
        public static Texture2D Icon_UserMinus;
        public static Texture2D Icon_Bag;
        public static Texture2D Icon_Star;
        public static Texture2D Icon_Screen;
        public static Texture2D Icon_Arrow_Up;
        public static Texture2D Icon_Arrow2_Up;
        public static Texture2D Icon_Arrow2_Down;
        public static Texture2D Icon_Arrow3_Left;
        public static Texture2D Icon_Arrow3_Right;
        public static Texture2D Icon_Pencil;
        public static Texture2D Icon_Page;
        public static Texture2D Icon_Copy;
        public static Texture2D Icon_Skull;
        public static Texture2D Icon_Weapon;

        public static Texture2D Icon_GameObject;
        public static Texture2D Icon_Animation;
        public static Texture2D Icon_Collider;

        // resources - skin
        public static Texture2D Texture_Logo;
        public static Texture2D Texture_LogoIcon;
        public static Texture2D Texture_Box;
        public static Texture2D Texture_FlatBox;
        public static Texture2D Texture_NoPreview;
        public static Texture2D Texture_BoxSides;
        public static Texture2D Texture_Selected;
        public static Texture2D Texture_FlatDarker;
        public static Texture2D Texture_FlatLighter;
        public static Texture2D Texture_ToolButtonSelected;
        public static Texture2D Texture_ToolButtonSelectedLeft;
        public static Texture2D Texture_ToolButtonSelectedMid;
        public static Texture2D Texture_ToolButtonSelectedRight;

        // ...
        private static GUIStyle[] customStyles;

        // ================================================================================================================

        public static void UseSkin()
        {
            // some stuff that can be set before test for inited
            EditorStyles.textField.wordWrap = true;
            if (EditorGUIUtility.isProSkin)
            {
                //EditorStyles.miniButtonMid.border = new RectOffset(2, 2, 0, 0); // fix a bug present in EditorStyles.miniButtonMid
            }
            else
            {
                InspectorDividerColor = new Color(0.45f, 0.45f, 0.45f, 1f);
            }

            // load the skin
            if (Skin != null)
            {
                GUI.skin = Skin;
                return;
            }

            LoadSkinTextures();

            // ----------------------------------------------------------------------------------------------------
            // init some styles that don't need skin file to define
            Skin = GUI.skin;//EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            customStyles = Skin.customStyles;

            // text
            WarningLabelStyle = new GUIStyle(EditorStyles.label)
            {
                name = "PLYWarningLabel",
                richText = false,
                normal = { textColor = (EditorGUIUtility.isProSkin ? Color.yellow : Color.red) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, WarningLabelStyle);

            BoldLabelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                name = "PLYBoldLabel",
                richText = false,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(3, 3, 0, 3),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, BoldLabelStyle);


            RichLabelStyle = new GUIStyle(EditorStyles.label)
            {
                name = "PLYRichLabel",
                richText = true,
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, RichLabelStyle);

            CenterLabelStyle = new GUIStyle(EditorStyles.label)
            {
                name = "PLYCenterLabel",
                richText = false,
                alignment = TextAnchor.MiddleCenter
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, CenterLabelStyle);

            Head1Style = new GUIStyle(EditorStyles.boldLabel)
            {
                name = "PLYHead1",
                richText = false,
                fontSize = 20,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(12, 0, 3, 3),
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.5f, 0.5f, 0.5f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, Head1Style);

            Head2Style = new GUIStyle(EditorStyles.boldLabel)
            {
                name = "PLYHead2",
                richText = false,
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 10),
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, Head2Style);

            Head3Style = new GUIStyle(EditorStyles.boldLabel)
            {
                name = "PLYHead3",
                richText = false,
                fontSize = 15,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 5),
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, Head3Style);

            Head4Style = new GUIStyle(Head3Style)
            {
                name = "PLYHead4",
                richText = false,
                fontSize = 12,
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, Head4Style);

            LabelRightStyle = new GUIStyle(Skin.label)
            {
                name = "PLYLabelRight",
                richText = false,
                alignment = TextAnchor.MiddleRight,
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, LabelRightStyle);

            InspectorHeadStyle = new GUIStyle(Head3Style)
            {
                name = "PLYInspectorHead",
                padding = new RectOffset(15, 0, 3, 3),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, InspectorHeadStyle);

            InspectorHeadBoxStyle = new GUIStyle(Skin.box)
            {
                name = "PLYInspectorHeadBox",
                richText = false,
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(25, 0, 3, 3),
                margin = new RectOffset(0, 0, 20, 10),
                stretchWidth = true,
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, InspectorHeadBoxStyle);

            InspectorHeadFoldStyle = new GUIStyle(EditorStyles.foldout)
            {
                name = "PLYInspectorHeadFold",
                richText = false,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(17, 0, 0, 0),
                margin = new RectOffset(10, 10, 2, 0),
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, InspectorHeadFoldStyle);

            HeadingFoldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                name = "PLYHeadingFoldout",
                richText = false,
                fontSize = 15,
                padding = new RectOffset(17, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 5),
                normal = { textColor = (EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.35f, 0.35f, 0.35f)) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, HeadingFoldoutStyle);

            // buttons
            ButtonStyle = new GUIStyle(Skin.button)
            {
                name = "PLYButton",
                richText = false,
                fontSize = 12,
                padding = new RectOffset(20, 20, 5, 5),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ButtonStyle);

            ButtonLeftStyle = new GUIStyle(EditorStyles.miniButtonLeft)
            {
                name = "PLYButtonLeft",
                richText = false,
                fontSize = 11,
                padding = new RectOffset(0, 0, 3, 5),
                margin = new RectOffset(2, 0, 0, 2),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ButtonLeftStyle);

            ButtonMidStyle = new GUIStyle(EditorStyles.miniButtonMid)
            {
                name = "PLYButtonMid",
                richText = false,
                fontSize = 11,
                padding = new RectOffset(0, 0, 3, 5),
                margin = new RectOffset(0, 0, 0, 2),
                border = new RectOffset(2, 2, 0, 0), // fix a bug present in EditorStyles.miniButtonMid
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ButtonMidStyle);

            ButtonRightStyle = new GUIStyle(EditorStyles.miniButtonRight)
            {
                name = "PLYButtonRight",
                richText = false,
                fontSize = 11,
                padding = new RectOffset(0, 0, 3, 5),
                margin = new RectOffset(0, 2, 0, 2),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ButtonRightStyle);

            LeftTabStyle = new GUIStyle(EditorStyles.miniButtonLeft)
            {
                name = "PLYLeftTab",
                richText = false,
                alignment = TextAnchor.MiddleLeft,
                fontSize = 10,
                fixedHeight = 24,
                stretchWidth = true,
                padding = new RectOffset(3, 3, 5, 5),
                margin = new RectOffset(3, 0, 0, 3),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, LeftTabStyle);

            TinyButton = new GUIStyle(EditorStyles.miniButton)
            {
                name = "PLYTinyButton",
                richText = false,
                fontSize = 10,
                padding = new RectOffset(2, 2, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, TinyButton);

            ToolbarStyle = new GUIStyle(Skin.button)
            {
                name = "PLYToolbar",
                fontStyle = FontStyle.Bold,
                fontSize = 11,
                padding = new RectOffset(5, 5, 5, 5),
                overflow = new RectOffset(0, 0, 0, 1),
                onNormal = { background = Texture_ToolButtonSelected, textColor = Color.white },
                onActive = { background = Texture_ToolButtonSelected, textColor = Color.white },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ToolbarStyle);

            ToolbarButtonLeftStyle = new GUIStyle(Skin.FindStyle("ButtonLeft"))
            {
                name = "PLYToolbarLeft",
                fontStyle = FontStyle.Bold,
                fontSize = 11,
                padding = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(4, 2, 2, 2),
                onNormal = { background = Texture_ToolButtonSelectedLeft },
                onActive = { background = Texture_ToolButtonSelectedLeft },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ToolbarButtonLeftStyle);

            ToolbarButtonMidStyle = new GUIStyle(Skin.FindStyle("ButtonMid"))
            {
                name = "PLYToolbarMid",
                fontStyle = FontStyle.Bold,
                fontSize = 11,
                padding = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(2, 2, 2, 2),
                onNormal = { background = Texture_ToolButtonSelectedMid },
                onActive = { background = Texture_ToolButtonSelectedMid },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ToolbarButtonMidStyle);

            ToolbarButtonRightStyle = new GUIStyle(Skin.FindStyle("ButtonRight"))
            {
                name = "PLYToolbarRight",
                fontStyle = FontStyle.Bold,
                fontSize = 11,
                padding = new RectOffset(5, 5, 5, 5),
                border = new RectOffset(2, 4, 2, 2),
                onNormal = { background = Texture_ToolButtonSelectedRight },
                onActive = { background = Texture_ToolButtonSelectedRight },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ToolbarButtonRightStyle);

            // boxes and such
            DividerStyle = new GUIStyle()
            {
                name = "PLYDivider",
                border = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { background = EditorGUIUtility.whiteTexture },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, DividerStyle);

            BoxStyle = new GUIStyle(Skin.box)
            {
                name = "PLYBox",
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 0, 5, 0),
                normal = { background = (EditorGUIUtility.isProSkin ? Texture_Box : Skin.box.normal.background) },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, BoxStyle);

            ScrollViewStyle = new GUIStyle(Skin.box)
            {
                name = "PLYScrollView",
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ScrollViewStyle);

            ScrollViewNoTLMarginStyle = new GUIStyle(ScrollViewStyle)
            {
                name = "PLYScrollViewNoTLMargin",
                margin = new RectOffset(0, 5, 0, 5),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ScrollViewNoTLMarginStyle);

            ScrollViewNoMarginPaddingStyle = new GUIStyle(ScrollViewStyle)
            {
                name = "PLYScrollViewNoTLMarginNoPadding",
                padding = new RectOffset(0, 0, 3, 3),
                margin = new RectOffset(0, 0, 0, 0),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ScrollViewNoMarginPaddingStyle);

            // fields
            DelTextFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                name = "PLYDelTextField",
                fixedHeight = 19,
                padding = new RectOffset(5, 5, 2, 2),
                margin = new RectOffset(EditorStyles.textField.margin.left, 0, 0, 5),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, DelTextFieldStyle);

            TextFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                name = "PLYTextField",
                wordWrap = false,
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, TextFieldStyle);

            // misc
            MenuBoxStyle = new GUIStyle(BoxStyle)
            {
                name = "PLYMenuBox",
                margin = new RectOffset(1, 10, 0, 0),
                padding = new RectOffset(1, 1, 0, 0),
                normal = { background = Texture_BoxSides },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, MenuBoxStyle);

            MenuItemStyle = new GUIStyle(EditorStyles.toggle)
            {
                name = "PLYMenuItem",
                richText = false,
                fontSize = 14,
                alignment = TextAnchor.MiddleRight,
                border = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 10, 11, 7),
                normal = { background = null },
                hover = { background = null },
                active = { background = null },
                focused = { background = null },
                onNormal = { background = Texture_Selected, textColor = Color.white },
                onHover = { background = null },
                onActive = { background = Texture_Selected, textColor = Color.white },
                onFocused = { background = null },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, MenuItemStyle);

            MenuHeadStyle = new GUIStyle(EditorStyles.label)
            {
                name = "PLYMenuHead",
                richText = false,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleRight,
                border = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 10, 20, 3),
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, MenuHeadStyle);

            ListItemBackDarkStyle = new GUIStyle(Skin.button)
            {
                name = "PLYListItemBackDark",
                richText = false,
                fontSize = 11,
                alignment = TextAnchor.MiddleLeft,
                clipping = TextClipping.Clip,
                stretchWidth = false,
                wordWrap = false,
                overflow = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(1, 1, 1, 1),
                margin = new RectOffset(0, 0, 1, 0),
                hover = { background = Texture_FlatDarker },
                onHover = { background = Texture_FlatDarker },
                normal = { background = Texture_FlatDarker },
                active = { background = Texture_FlatDarker },
                onNormal = { background = Texture_FlatDarker },
                onActive = { background = Texture_FlatDarker },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ListItemBackDarkStyle);

            ListItemBackLightStyle = new GUIStyle(ListItemBackDarkStyle)
            {
                name = "PLYListItemBackLight",
                hover = { background = Texture_FlatLighter },
                onHover = { background = Texture_FlatLighter },
                normal = { background = Texture_FlatLighter },
                active = { background = Texture_FlatLighter },
                onNormal = { background = Texture_FlatLighter },
                onActive = { background = Texture_FlatLighter },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ListItemBackLightStyle);

            ListItemSelectedStyle = new GUIStyle(ListItemBackDarkStyle)
            {
                name = "PLYListItemSelected",
                hover = { background = Texture_Selected, textColor = Color.white },
                onHover = { background = Texture_Selected, textColor = Color.white },
                normal = { background = Texture_Selected, textColor = Color.white },
                active = { background = Texture_Selected, textColor = Color.white },
                onNormal = { background = Texture_Selected, textColor = Color.white },
                onActive = { background = Texture_Selected, textColor = Color.white },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, ListItemSelectedStyle);

            AboutLogoAreaStyle = new GUIStyle(Skin.box)
            {
                name = "PLYAboutLogoArea",
                stretchWidth = true,
                margin = new RectOffset(0, 0, 0, 10),
                padding = new RectOffset(10, 10, 10, 10),
                normal = { background = EditorGUIUtility.whiteTexture },
            }; ArrayUtility.Add<GUIStyle>(ref customStyles, AboutLogoAreaStyle);



            // finally, set it
            Skin.customStyles = customStyles;
            GUI.skin = Skin;
        }

        private static void LoadSkinTextures()
        {
            Icon_GameObject = AssetPreview.GetMiniTypeThumbnail(typeof(GameObject));
            Icon_Animation = AssetPreview.GetMiniTypeThumbnail(typeof(Animation));
            Icon_Collider = AssetPreview.GetMiniTypeThumbnail(typeof(BoxCollider));

            Icon_Tag = LoadEditorTexture(EditorResourcePath + "Icons/tag.png");
            Icon_Help = LoadEditorTexture(EditorResourcePath + "Icons/help.png");
            Icon_Plus = LoadEditorTexture(EditorResourcePath + "Icons/plus.png");
            Icon_Minus = LoadEditorTexture(EditorResourcePath + "Icons/minus.png");
            Icon_Accept = LoadEditorTexture(EditorResourcePath + "Icons/accept.png");
            Icon_Cancel = LoadEditorTexture(EditorResourcePath + "Icons/cancel.png");
            Icon_User = LoadEditorTexture(EditorResourcePath + "Icons/user.png");
            Icon_Users = LoadEditorTexture(EditorResourcePath + "Icons/users.png");
            Icon_UserPlus = LoadEditorTexture(EditorResourcePath + "Icons/user_plus.png");
            Icon_UserMinus = LoadEditorTexture(EditorResourcePath + "Icons/user_minus.png");
            Icon_Bag = LoadEditorTexture(EditorResourcePath + "Icons/bag.png");
            Icon_Star = LoadEditorTexture(EditorResourcePath + "Icons/star.png");
            Icon_Screen = LoadEditorTexture(EditorResourcePath + "Icons/screen.png");
            Icon_Refresh = LoadEditorTexture(EditorResourcePath + "Icons/refresh.png");
            Icon_Exclaim_Red = LoadEditorTexture(EditorResourcePath + "Icons/exclaim_red.png");
            Icon_Arrow_Up = LoadEditorTexture(EditorResourcePath + "Icons/arrow_up.png");
            Icon_Arrow2_Up = LoadEditorTexture(EditorResourcePath + "Icons/arrow2_up.png");
            Icon_Arrow2_Down = LoadEditorTexture(EditorResourcePath + "Icons/arrow2_down.png");
            Icon_Arrow3_Left = LoadEditorTexture(EditorResourcePath + "Icons/arrow3_left.png");
            Icon_Arrow3_Right = LoadEditorTexture(EditorResourcePath + "Icons/arrow3_right.png");
            Icon_Pencil = LoadEditorTexture(EditorResourcePath + "Icons/pencil.png");
            Icon_Page = LoadEditorTexture(EditorResourcePath + "Icons/page.png");
            Icon_Copy = LoadEditorTexture(EditorResourcePath + "Icons/copy.png");
            Icon_Skull = LoadEditorTexture(EditorResourcePath + "Icons/skull.png");
            Icon_Weapon = LoadEditorTexture(EditorResourcePath + "Icons/weapon.png");

            Texture_Logo = LoadEditorTexture(EditorResourcePath + "Skin/logo.png");
            Texture_LogoIcon = LoadEditorTexture(EditorResourcePath + "Skin/logo_icon" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_Box = LoadEditorTexture(EditorResourcePath + "Skin/box.png");
            Texture_FlatBox = LoadEditorTexture(EditorResourcePath + "Skin/flatbox" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_NoPreview = LoadEditorTexture(EditorResourcePath + "Skin/no_preview.png");
            Texture_BoxSides = LoadEditorTexture(EditorResourcePath + "Skin/boxsides" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_Selected = LoadEditorTexture(EditorResourcePath + "Skin/selected" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_FlatDarker = LoadEditorTexture(EditorResourcePath + "Skin/flat_darker" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_FlatLighter = LoadEditorTexture(EditorResourcePath + "Skin/flat_lighter" + (EditorGUIUtility.isProSkin ? "" : "_l") + ".png");
            Texture_ToolButtonSelected = LoadEditorTexture(EditorResourcePath + "Skin/toolbar_selected.png");
            Texture_ToolButtonSelectedLeft = LoadEditorTexture(EditorResourcePath + "Skin/toolbar_selected_left.png");
            Texture_ToolButtonSelectedMid = LoadEditorTexture(EditorResourcePath + "Skin/toolbar_selected_mid.png");
            Texture_ToolButtonSelectedRight = LoadEditorTexture(EditorResourcePath + "Skin/toolbar_selected_right.png");
        }

        #endregion
        // ================================================================================================================
        #region resource loading

        public static Texture2D LoadEditorTexture(string fn)
        {
            Texture2D tx = AssetDatabase.LoadAssetAtPath(fn, typeof(Texture2D)) as Texture2D;
            if (tx == null) Debug.LogWarning("Failed to load texture: " + fn);
            else if (tx.wrapMode != TextureWrapMode.Clamp)
            {
                string path = AssetDatabase.GetAssetPath(tx);
                TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                tImporter.textureType = TextureImporterType.GUI;
                tImporter.npotScale = TextureImporterNPOTScale.None;
                tImporter.filterMode = FilterMode.Point;
                tImporter.wrapMode = TextureWrapMode.Clamp;
                tImporter.maxTextureSize = 64;
                tImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                AssetDatabase.SaveAssets();
            }
            return tx;
        }

        #endregion
        // ================================================================================================================
        #region misc

        //public static Texture2D GetAssetPreview(Object ob)
        //{
        //	#if UNITY_3
        //	return EditorUtility.GetAssetPreview(ob);
        //	#else
        //	return AssetPreview.GetAssetPreview(ob);
        //	#endif
        //}

        //public static void HideRendererWireframe(GameObject go)
        //{
        //	EditorUtility.SetSelectedWireframeHidden(go.renderer, true);
        //	for (int i = 0; i < go.transform.childCount; i++)
        //	{
        //		PLYEditorUtil.HideRendererWireframe(go.transform.GetChild(i).gameObject);
        //	}
        //}

        //public static void FocusSceneView()
        //{
        //	// focus the scene view
        //	if (SceneView.sceneViews.Count > 0) (SceneView.sceneViews[0] as SceneView).Focus();
        //}

        #endregion
        // ================================================================================================================
        #region GUI Controls

        // Draw button with icon and text
        public static bool IconButton(string label, Texture2D icon, params GUILayoutOption[] options) { return IconButton(label, icon, null, options); }
        public static bool IconButton(string label, Texture2D icon, GUIStyle style, params GUILayoutOption[] options)
        {
            if (style != null) return GUILayout.Button(new GUIContent(label, icon), style, options);
            else return GUILayout.Button(new GUIContent(label, icon), options);
        }

        // this toggle does not return the new state but rather if the state has changed (true) or not (false)
        public static bool ToggleButton(bool active, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            bool new_value = active;
            if (string.IsNullOrEmpty(label)) new_value = GUILayout.Toggle(active, GUIContent.none, style, options);
            else new_value = GUILayout.Toggle(active, label, style, options);
            return (new_value != active);
        }

        // this toggle does not return the new state but rather if the state has changed (true) or not (false)
        public static bool ToggleButton(bool active, string label, Texture2D icon, GUIStyle style, params GUILayoutOption[] options)
        {
            bool new_value = active;
            new_value = GUILayout.Toggle(active, new GUIContent(label, icon), style, options);
            return (new_value != active);
        }

        // this toggle does not return the new state but rather if the state has changed (true) or not (false)
        public static bool ToggleButton(bool active, GUIContent label, GUIStyle style, params GUILayoutOption[] options)
        {
            bool new_value = active;
            new_value = GUILayout.Toggle(active, label, style, options);
            return (new_value != active);
        }

        // this toggle does not return the new state but rather if the state has changed (true) or not (false)
        public static bool ToggleButton(bool active, string label, GUIStyle style, Color onTint, params GUILayoutOption[] options)
        {
            Color c = GUI.backgroundColor;
            if (active) GUI.backgroundColor = onTint;
            bool new_value = active;
            if (string.IsNullOrEmpty(label)) new_value = GUILayout.Toggle(active, GUIContent.none, style, options);
            else new_value = GUILayout.Toggle(active, label, style, options);
            GUI.backgroundColor = c;
            return (new_value != active);
        }

        // this toggle does not return the new state but rather if the state has changed (true) or not (false)
        public static bool ToggleButton(bool active, GUIContent label, GUIStyle style, Color onTint, params GUILayoutOption[] options)
        {
            Color c = GUI.backgroundColor;
            if (active) GUI.backgroundColor = onTint;
            bool new_value = active;
            if (label == null) new_value = GUILayout.Toggle(active, GUIContent.none, style, options);
            else new_value = GUILayout.Toggle(active, label, style, options);
            GUI.backgroundColor = c;
            return (new_value != active);
        }

        public static bool TintedButton(GUIContent label, Color tint, params GUILayoutOption[] options)
        {
            Color c = GUI.backgroundColor;
            GUI.backgroundColor = tint;
            bool ret = GUILayout.Button(label, options);
            GUI.backgroundColor = c;
            return ret;
        }

        public static bool TintedButton(string label, Color tint, params GUILayoutOption[] options)
        {
            Color c = GUI.backgroundColor;
            GUI.backgroundColor = tint;
            bool ret = GUILayout.Button(label, options);
            GUI.backgroundColor = c;
            return ret;
        }

        // foldout for use in inspector where you can click on label too, unlike EditorGUILayout.Foldout
        public static bool Foldout(bool foldout, string label, GUIStyle style, params GUILayoutOption[] options)
        {
            if (foldout)
            {
                GUIStyle style2 = new GUIStyle(style);
                style2.normal = style.onNormal;
                style2.active = style.onActive;
                if (GUILayout.Button(label, style2)) return false;
            }
            else
            {
                if (GUILayout.Button(label, style)) return true;
            }
            return foldout;
        }

        // menu (similar to the one in Unity Preferences). If the menuItem's name starts with "-" then that will be a separator
        // if the "-" is followed by more characters then that separator will have a heading. 
        // a null/empty string for menuItem will also cause a separator/space in the menu
        public static int Menu(int selected, string[] menuItems, params GUILayoutOption[] options)
        {
            if (selected >= menuItems.Length || selected < 0) selected = 0;
            EditorGUILayout.BeginVertical(MenuBoxStyle, options);
            {
                GUILayout.Space(20);

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (string.IsNullOrEmpty(menuItems[i])) { GUILayout.Space(15); continue; }

                    if (menuItems[i][0] == '-')
                    {
                        if (menuItems[i].Length > 1)
                        {
                            GUILayout.Label(menuItems[i].Substring(1), RPGEditorGUI.MenuHeadStyle);
                        }
                        else
                        {
                            RPGEditorGUI.DrawHorizontalLine(1, RPGEditorGUI.DividerColor, 5, 5);
                        }
                        continue;
                    }

                    if (GUILayout.Toggle((i == selected), menuItems[i], RPGEditorGUI.MenuItemStyle)) selected = i;
                }

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndVertical();
            return selected;
        }
        public static int MenuHorizontal(int selected, string[] menuItems, params GUILayoutOption[] options)
        {
            if (selected >= menuItems.Length || selected < 0) selected = 0;
            EditorGUILayout.BeginHorizontal(MenuBoxStyle, options);
            {
                GUILayout.Space(20);

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (string.IsNullOrEmpty(menuItems[i])) { GUILayout.Space(15); continue; }

                    if (menuItems[i][0] == '-')
                    {
                        if (menuItems[i].Length > 1)
                        {
                            GUILayout.Label(menuItems[i].Substring(1), RPGEditorGUI.MenuHeadStyle);
                        }
                        else
                        {
                            RPGEditorGUI.DrawVerticalLine(1, RPGEditorGUI.DividerColor, 5, 5);
                        }
                        continue;
                    }

                    if (GUILayout.Toggle((i == selected), menuItems[i], RPGEditorGUI.ButtonStyle)) selected = i;
                }

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            return selected;
        }
        #endregion
        // ================================================================================================================
        #region Layout

        // this scrollview makes al lattempts to hide the bottom/horizontal bar while always showing the vertical/right bar
        public static Vector2 BeginScrollView(Vector2 scroll, bool hideAllBars, params GUILayoutOption[] options)
        {
            return BeginScrollView(scroll, hideAllBars, null, options);
        }
        public static Vector2 BeginScrollView(Vector2 scroll, bool hideAllBars, GUIStyle backgroundStyle, params GUILayoutOption[] options)
        {
            if (!hideAllBars) return BeginScrollView(scroll, backgroundStyle, options);
            return EditorGUILayout.BeginScrollView(scroll, false, false, GUIStyle.none, GUIStyle.none, RPGEditorGUI.Skin.scrollView, options);
        }
        public static Vector2 BeginScrollView(Vector2 scroll, params GUILayoutOption[] options)
        {
            return BeginScrollView(scroll, null, options);
        }
        public static Vector2 BeginScrollView(Vector2 scroll, GUIStyle backgroundStyle, params GUILayoutOption[] options)
        {
            return EditorGUILayout.BeginScrollView(scroll, false, true, GUIStyle.none, RPGEditorGUI.Skin.verticalScrollbar, (backgroundStyle == null ? RPGEditorGUI.Skin.scrollView : backgroundStyle), options);
        }
        public static void EndScrollView()
        {
            GUILayout.Space(30);
            EditorGUILayout.EndScrollView();
        }

        #endregion
        // ================================================================================================================
        #region Line/Rect/Grid/etc drawing

        public static void DrawHorizontalLine(float thickness, Color color, float paddingTop = 0f, float paddingBottom = 0f, float width = 0f)
        {
            GUILayoutOption[] options = new GUILayoutOption[2]
            {
            GUILayout.ExpandHeight(false),
            (width > 0.0f ? GUILayout.Width(width) : GUILayout.ExpandWidth(true))
            };
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayoutUtility.GetRect(0f, (thickness + paddingTop + paddingBottom), options);
            Rect r = GUILayoutUtility.GetLastRect();
            r.y += paddingTop;
            r.height = thickness;
            GUI.Box(r, "", RPGEditorGUI.DividerStyle);
            GUI.backgroundColor = prevColor;
        }

        public static void DrawVerticalLine(float thickness, Color color, float paddingLeft = 0f, float paddingRight = 0f, float height = 0f)
        {
            GUILayoutOption[] options = new GUILayoutOption[2]
            {
            GUILayout.ExpandWidth(false),
            (height > 0.0f ? GUILayout.Height(height) : GUILayout.ExpandHeight(true))
            };
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayoutUtility.GetRect((thickness + paddingLeft + paddingRight), 0f, options);
            Rect r = GUILayoutUtility.GetLastRect();
            r.x += paddingLeft;
            r.width = thickness;
            GUI.Box(r, "", RPGEditorGUI.DividerStyle);
            GUI.backgroundColor = prevColor;
        }

        #endregion
        // ================================================================================================================

        public static void LookLikeControls(float labelWidth)
        {
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public static void LookLikeControls(float labelWidth, float fieldWidth)
        {
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUIUtility.fieldWidth = fieldWidth;
        }

        public static void LookLikeControls()
        {
            EditorGUIUtility.labelWidth = 0;
            EditorGUIUtility.fieldWidth = 0;
        }
        #region �Զ������
        /// <summary>
        /// ��List<int>�Զ�����ʾ,��Ҫ��OnEnable���ʼ��size
        /// </summary>
        /// <param name="size"></param>
        /// <param name="IDlist"></param>
        /// <param name="caption"></param>
        /// <param name="label"></param>
        public static void DynamicArrayView(ref int size,ref List<int> IDlist,string caption,string label)
        {
            size = EditorGUILayout.IntSlider(caption,size, 0, 20);
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            if (size == 0)
            {
                IDlist.Clear();
            }
            int dif = IDlist.Count - size;
            if (dif > 0)
            {
                IDlist.RemoveRange(size, dif);
            }
            if (dif < 0)
            {
                IDlist.AddRange(new int[-dif]);
            }
            for (int i = 0; i < size; i++)
            {
                IDlist[i] = EditorGUILayout.IntField(label, IDlist[i]);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        /// <summary>
        /// ��List<int> �Զ�����ʾ,������ͨ������ѡ��,��Ҫ��OnEnable���ʼ��size
        /// </summary>
        /// <param name="size"></param>
        /// <param name="IDlist"></param>
        /// <param name="caption"></param>
        /// <param name="child"></param>
        /// <param name="displayOptions"></param>
        /// <param name="optionValues"></param>
        public static void DynamicArrayView(ref int size, ref List<int> IDlist, string caption, string child,string[] displayOptions,int []optionValues,int maxSize=20)
        {
            size = EditorGUILayout.IntSlider(caption, size, 0, maxSize);
            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(Screen.width - 16));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            if (size == 0)
            {
                IDlist.Clear();
            }
            int dif = IDlist.Count - size;
            if (dif > 0)
            {
                IDlist.RemoveRange(size, dif);
            }
            if (dif < 0)
            {
                IDlist.AddRange(new int[-dif]);
            }
            for (int i = 0; i < size; i++)
            {
                IDlist[i] = EditorGUILayout.IntPopup(child, IDlist[i], displayOptions, optionValues);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        public static int IntFieldClamp(string label, ref int i, int left, int right)
        {
            int ret = EditorGUILayout.IntField(label, i);
            if (ret < left)
                ret = left;
            if (ret > right)
                ret = right;
            i = ret;
            return ret;
        }
        #endregion
    }
}