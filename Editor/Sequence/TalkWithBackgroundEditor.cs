using UnityEngine;
using UnityEditor;
namespace Sequence
{
    [CustomEditor(typeof(TalkWithBackground))]
    public class TalkWithBackgroundEditor : Editor
    {
        private static GenericMenu AddCommandMenu;
        private static int m_ChangeCommandIndex;
        TalkWithBackground _target;
        private const float DEFAULT_FADE_DURATION= 1.5f;
        public enum 事件类型
        {
            设置人物位置和头像,
            激活人物头像,
            删除人物头像,
            抖动人物头像,
            更换音乐,
            降低音量,
            重置音量,
            更换背景,
            自动播放文本,
            停止自动文本
        }
        public enum 显示位置
        {
            最左边,
            左边,
            右边,
            最右边
        }
        public override void OnInspectorGUI()
        {
            _target.Background = EditorGUILayout.IntField("默认背景", _target.Background);
            for (int i = 0; i < _target.Code.Count; i++)
            {
                string prefix = "%";
                if (_target.Code[i].Length >= 3)
                    prefix = _target.Code[i].Substring(0, 3);

                if (prefix.StartsWith("<"))
                {
                    if (prefix.Equals(GetCode(事件类型.设置人物位置和头像)))
                    {
                        DrawCommand("设置人物位置和头像", i, DrawShowPosition);
                    }
                    if (prefix.Equals(GetCode(事件类型.激活人物头像)))
                    {
                        DrawCommand("激活人物头像", i, DrawActivePosition);
                    }
                    if (prefix.Equals(GetCode(事件类型.删除人物头像)))
                    {
                        DrawCommand("删除人物头像", i, DrawHideCharacter);
                    }
                    if (prefix.Equals(GetCode(事件类型.抖动人物头像)))
                    {
                        DrawCommand("抖动人物头像", i, DrawShakePosition);
                    }
                    if (prefix.Equals(GetCode(事件类型.更换音乐)))
                    {
                        DrawCommand("更换音乐", i, DrawChangeBGM);
                    }
                    if (prefix.Equals(GetCode(事件类型.降低音量)))
                    {
                        DrawCommand("降低音量", i, DrawLowerVolume);
                    }
                    if (prefix.Equals(GetCode(事件类型.重置音量)))
                    {
                        DrawCommand("重置音量", i, DrawRestoreVolume);
                    }
                    if (prefix.Equals(GetCode(事件类型.更换背景)))
                    {
                        DrawCommand("更换背景", i, DrawChangeBackground);
                    }
                    if (prefix.Equals(GetCode(事件类型.自动播放文本)))
                    {
                        DrawCommand("自动播放文本", i, DrawBeginAutoText);
                    }
                    if (prefix.Equals(GetCode(事件类型.停止自动文本)))
                    {
                        DrawCommand("停止自动文本", i, DrawEndAutoText);
                    }
                }
                else
                {
                    DrawCommand("切换文本", i, DrawText);
                }
            }
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(GUI.skin.button);
            if (GUILayout.Button("添加", GUILayout.MaxWidth(60)))
            {
                PopupMenu();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.SelectableLabel(Utils.TextUtil.GetListString(_target.Code), GUILayout.ExpandHeight(true));
        }
        private void ChangeCommand(object CommandType)
        {
            switch ((事件类型)CommandType)
            {
                case 事件类型.设置人物位置和头像:
                    _target.Code[m_ChangeCommandIndex] = (ShowPosition(0, 0, 0));
                    break;
                case 事件类型.激活人物头像:
                    _target.Code[m_ChangeCommandIndex] = (ActivePosition(0));
                    break;
                case 事件类型.删除人物头像:
                    _target.Code[m_ChangeCommandIndex] = (HideCharacter(0));
                    break;
                case 事件类型.抖动人物头像:
                    _target.Code[m_ChangeCommandIndex] = (ShakePosition(0));
                    break;
                case 事件类型.更换音乐:
                    _target.Code[m_ChangeCommandIndex] = (ChangeBGM(0));
                    break;
                case 事件类型.降低音量:
                    _target.Code[m_ChangeCommandIndex] = (LowerVolume());
                    break;
                case 事件类型.重置音量:
                    _target.Code[m_ChangeCommandIndex] = (RestoreVolume());
                    break;
                case 事件类型.更换背景:
                    _target.Code[m_ChangeCommandIndex] = (ChangeBackground(0, DEFAULT_FADE_DURATION));
                    break;
                case 事件类型.自动播放文本:
                    _target.Code[m_ChangeCommandIndex] = (BeginAutoText());
                    break;
                case 事件类型.停止自动文本:
                    _target.Code[m_ChangeCommandIndex] = (EndAutoText());
                    break;
            }
        }
        private void AddNewCommand(object CommandType)
        {
            switch ((事件类型)CommandType)
            {
                case 事件类型.设置人物位置和头像:
                    _target.Code.Add(ShowPosition(0, 0, 0));
                    break;
                case 事件类型.激活人物头像:
                    _target.Code.Add(ActivePosition(0));
                    break;
                case 事件类型.删除人物头像:
                    _target.Code.Add(HideCharacter(0));
                    break;
                case 事件类型.抖动人物头像:
                    _target.Code.Add(ShakePosition(0));
                    break;
                case 事件类型.更换音乐:
                    _target.Code.Add(ChangeBGM(0));
                    break;
                case 事件类型.降低音量:
                    _target.Code.Add(LowerVolume());
                    break;
                case 事件类型.重置音量:
                    _target.Code.Add(RestoreVolume());
                    break;
                case 事件类型.更换背景:
                    _target.Code.Add(ChangeBackground(0, DEFAULT_FADE_DURATION));
                    break;
                case 事件类型.自动播放文本:
                    _target.Code.Add(BeginAutoText());
                    break;
                case 事件类型.停止自动文本:
                    _target.Code.Add(EndAutoText());
                    break;
            }
        }
        private void PopupMenu(int Index = -1)
        {
            m_ChangeCommandIndex = Index;
            AddCommandMenu = new GenericMenu();
            string[] caption = System.Enum.GetNames(typeof(事件类型));
            事件类型[] values = (事件类型[])System.Enum.GetValues(typeof(事件类型));
            for (int i = 0; i < caption.Length; i++)
            {
                if (Index > -1)
                    AddCommandMenu.AddItem(new GUIContent(caption[i]), false, ChangeCommand, values[i]);
                else
                    AddCommandMenu.AddItem(new GUIContent(caption[i]), false, AddNewCommand, values[i]);
            }
            AddCommandMenu.ShowAsContext();
        }
        private void DrawCommand(string CommandName, int Index, System.Action<int> Func)
        {
            GUILayout.BeginVertical(GUI.skin.box);

            GUI.backgroundColor = Color.gray;
            GUILayout.BeginHorizontal(GUI.skin.button);

            GUILayout.Label(CommandName, GUILayout.MinWidth(80), GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();
            GUILayout.Space(10);
            if (GUILayout.Button("▼"))
            {
                PopupMenu(Index);
            }

            GUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            EditorGUILayout.Separator();

            Func.Invoke(Index);

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("删除"))
            {
                _target.Code.RemoveAt(Index);
            }
            if (GUILayout.Button("插入"))
            {
                string s = (string)_target.Code[Index].Clone();
                _target.Code.Insert(Index, s);
            }
            EditorGUILayout.Space();
            //↖↑↗←↹→↙↓↘
            if (GUILayout.Button("↑"))
            {
                string s0 = _target.Code[Index - 1];
                string s1 = _target.Code[Index];
                _target.Code[Index - 1] = s1;
                _target.Code[Index] = s0;
            }
            if (GUILayout.Button("↓"))
            {
                string s0 = _target.Code[Index + 1];
                string s1 = _target.Code[Index];
                _target.Code[Index + 1] = s1;
                _target.Code[Index] = s0;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        private void DrawText(int Index)
        {
            _target.Code[Index] = EditorGUILayout.TextArea(_target.Code[Index], GUILayout.ExpandWidth(true), GUILayout.MinHeight(28));
        }
        private void DrawShowPosition(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            //int Pos = Mathf.Clamp(Position,(int) 显示位置.最左边, (int)显示位置.最右边);
            int Pos = int.Parse(paramsStr[0]);
            int CharacterID = int.Parse(paramsStr[1]);
            int FaceID = int.Parse(paramsStr[2]);
            CharacterID = EditorGUILayout.IntField("人物ID", CharacterID);
            Pos = (int)(显示位置)EditorGUILayout.EnumPopup(new GUIContent("位置", "人物头像出现的位置"), (显示位置)Pos);
            FaceID = EditorGUILayout.IntSlider("人物脸部ID", FaceID, 0, 3);
            _target.Code[Index] = ShowPosition(CharacterID, Pos, FaceID);
        }
        private string ShowPosition(int Position, int CharacterID, int FaceID)
        {
            return GetCode(事件类型.设置人物位置和头像) + " " + Position + "," + CharacterID + "," + FaceID;
        }

        private void DrawActivePosition(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            int Pos = int.Parse(paramsStr[0]);
            Pos = (int)(显示位置)EditorGUILayout.EnumPopup(new GUIContent("位置", "人物头像激活的位置"), (显示位置)Pos);
            _target.Code[Index] = ActivePosition(Pos);
        }
        private string ActivePosition(int Position)
        {
            return GetCode(事件类型.激活人物头像) + " " + Position;
        }

        private void DrawHideCharacter(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            int Pos = int.Parse(paramsStr[0]);
            Pos = (int)(显示位置)EditorGUILayout.EnumPopup(new GUIContent("位置", "人物头像消失的位置"), (显示位置)Pos);
            _target.Code[Index] = HideCharacter(Pos);
        }
        private string HideCharacter(int Position)
        {
            return GetCode(事件类型.删除人物头像) + " " + Position;
        }

        private void DrawShakePosition(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            int Pos = int.Parse(paramsStr[0]);
            Pos = (int)(显示位置)EditorGUILayout.EnumPopup(new GUIContent("位置", "人物头像震动的位置"), (显示位置)Pos);
            _target.Code[Index] = ShakePosition(Pos);
        }
        private string ShakePosition(int Position)
        {
            return GetCode(事件类型.抖动人物头像) + " " + Position;
        }

        private void DrawChangeBGM(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            int BGMID = int.Parse(paramsStr[0]);
            BGMID = EditorGUILayout.IntField("BGMID", BGMID);
            _target.Code[Index] = ChangeBGM(BGMID);
        }
        private string ChangeBGM(int BGMID)
        {
            return GetCode(事件类型.更换音乐) + " " + BGMID;
        }

        private void DrawLowerVolume(int Index)
        {
            _target.Code[Index] = LowerVolume();
        }
        private string LowerVolume()
        {
            return GetCode(事件类型.降低音量);
        }

        private void DrawRestoreVolume(int Index)
        {
            _target.Code[Index] = RestoreVolume();
        }
        private string RestoreVolume()
        {
            return GetCode(事件类型.重置音量);
        }

        private void DrawChangeBackground(int Index)
        {
            string[] paramsStr = _target.Code[Index].Substring(3).Trim().Split(',');
            int BackgroundID = int.Parse(paramsStr[0]);
            float Duration = float.Parse(paramsStr[1]);
            BackgroundID = EditorGUILayout.IntField("背景ID", BackgroundID);
            Duration = EditorGUILayout.FloatField("过渡时间", Duration);
            _target.Code[Index] = ChangeBackground(BackgroundID,Duration);
        }
        private string ChangeBackground(int BackgroundID, float Duration)
        {
            return GetCode(事件类型.更换背景) + " " + BackgroundID + "," + Duration;
        }

        private void DrawBeginAutoText(int Index)
        {
            _target.Code[Index] = BeginAutoText();
        }
        private string BeginAutoText()
        {
            return GetCode(事件类型.自动播放文本);
        }

        private void DrawEndAutoText(int Index)
        {
            _target.Code[Index] = EndAutoText();
        }
        private string EndAutoText()
        {
            return GetCode(事件类型.停止自动文本);
        }

        private string GetCode(事件类型 EventType)
        {
            switch (EventType)
            {
                case 事件类型.设置人物位置和头像:
                    return "<pp";
                case 事件类型.激活人物头像:
                    return "<ap";
                case 事件类型.删除人物头像:
                    return "<rp";
                case 事件类型.抖动人物头像:
                    return "<sp";
                case 事件类型.更换音乐:
                    return "<cm";
                case 事件类型.降低音量:
                    return "<lv";
                case 事件类型.重置音量:
                    return "<rv";
                case 事件类型.更换背景:
                    return "<cb";
                case 事件类型.自动播放文本:
                    return "<at";
                case 事件类型.停止自动文本:
                    return "<sa";
            }
            return "";
        }
        void OnEnable()
        {
            _target = target as TalkWithBackground;
            Undo.RegisterCompleteObjectUndo(_target, "Code");
        }
    }
}
