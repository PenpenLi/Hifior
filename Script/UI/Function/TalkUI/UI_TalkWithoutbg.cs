using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class UI_TalkWithoutbg : IPanel
    {
        public Image Top;
        public Image Char_Top;
        public Image Arrow_Top;
        public Text ContentBox_Top;
        public Text NameBox_Top;

        public Image Bottom;
        public Image Char_Bottom;
        public Image Arrow_Bottom;
        public Text ContentBox_Bottom;
        public Text NameBox_Bottom;

        private bool bTop = false;

        private int scriptLineIndex = 0;
        private int lastLineIndex = 0;
        private bool bTyping = false;
        private bool bStopType = false;
        /*
        +8,1 split获取数组，判断数组长度，若为3则新建并激活位置，为1则激活某个位置
        #0 激活上面板
        -0 删除上面板
        %5,4 移动摄像机到5,4
     */
        private string[] mTalkData = {
		//i代表人物id，p代表显示的位置
		"+0,0",
        "你好啊，我是米卡娅",//a代表要激活的位置
		"+1,1",
        "我叫艾克，是一名剑客",
        "*0",
        "你用剑啊，粗鲁的男人",
        "*1",
        "咕~~(╯﹏╰)b，好无辜的感觉啊",
        "-1",
        "-0"
    };
        private float TypeTime = 0.1f;
        private Color COLOR_TRANS = new Color(0.345f, 0.302f, 0.784f, 0f);
        private Color COLOR_LIGHT = new Color(0.345f, 0.302f, 0.784f, 0.784f);
        private Color COLOR_DARK = new Color(0.176f, 0.176f, 0.476f, 0.784f);
        private Color COLOR_WHITE = new Color(1f, 1f, 1f, 1f);
        private const int POS_TOP = 0;
        private const int POS_BOTTOM = 1;
        private const int POS_BOTH = 2;
        private bool HasTop(int pos) { return pos == POS_TOP || pos == POS_BOTH; }
        private bool HasBottom(int pos) { return pos == POS_BOTTOM || pos == POS_BOTH; }
        private int FadeSpeed { get { return ConstTable.SHOW_TALK_UI_FADE_TIME(); } }
        protected override void Awake()
        {
            base.Awake();

            Top.color = COLOR_TRANS;
            Bottom.color = COLOR_TRANS;
            gameObject.SetActive(false);
            Top.gameObject.SetActive(false);
            Bottom.gameObject.SetActive(false);
            ContentBox_Top.text = null;
            ContentBox_Bottom.text = null;
        }
        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    Show(mTalkData);
        //}
        void Update()
        {
            if (scriptLineIndex < mTalkData.Length)
            {
                bool isWaitForInput = AnalyseOneLine(mTalkData[scriptLineIndex]);
                lastLineIndex = scriptLineIndex;
                if (!isWaitForInput)
                {
                    scriptLineIndex++;
                    bStopType = false;
                }
                if (Input.GetMouseButtonDown(0) && isWaitForInput)
                {
                    if (!bTyping)//没有进行打字则是已经显示完毕
                    {
                        scriptLineIndex++;
                        bStopType = false;
                    }
                    else
                    {
                        bTyping = false;//停止打字，并且将该处文字全部显示
                        bStopType = true;
                    }
                }
            }
            else//此处结束显示
            {
                SoundManage.Instance.NormalBGM();
                if (Top.gameObject.activeSelf)
                {
                    HideCharacter(POS_TOP);
                }
                if (Bottom.gameObject.activeSelf)
                {
                    HideCharacter(POS_BOTTOM);
                }
                if (!Top.gameObject.activeSelf && !Bottom.gameObject.activeSelf)
                    StartCoroutine(IFadeHide(POS_BOTH, true));
            }
        }
        void setActive(int Pos)//设置哪个面板激活，激活的，对话框背景和人物头像均为高亮，且arrow不停的闪烁显示
                               //非激活的对话框背景和人物头像为灰色，且不显示箭头
        {
            if (HasTop(Pos) && Top.gameObject.activeSelf)
            {
                bTop = true;

                Top.color = COLOR_LIGHT;
                setColor(POS_TOP, Color.white);
                NameBox_Top.transform.parent.gameObject.SetActive(true);
                Arrow_Top.gameObject.SetActive(true);

                Bottom.color = COLOR_DARK;
                setColor(POS_BOTTOM, Color.grey);
                NameBox_Bottom.transform.parent.gameObject.SetActive(false);
                Arrow_Bottom.gameObject.SetActive(false);

            }
            if (HasBottom(Pos) && Bottom.gameObject.activeSelf)
            {
                bTop = false;

                Top.color = COLOR_DARK;
                setColor(POS_TOP, Color.grey);
                NameBox_Top.transform.parent.gameObject.SetActive(false);
                Arrow_Top.gameObject.SetActive(false);

                Bottom.color = COLOR_LIGHT;
                setColor(POS_BOTTOM, Color.white);
                NameBox_Bottom.transform.parent.gameObject.SetActive(true);
                Arrow_Bottom.gameObject.SetActive(true);
            }
        }
        void SetTalkName(int Pos, string name)
        {
            if (HasTop(Pos))
            {
                NameBox_Top.text = name;
            }
            if (HasBottom(Pos))
            {
                NameBox_Bottom.text = name;
            }
        }
        void ShowCharacter(int Pos, Sprite icon, string name)
        {
            if (HasTop(Pos))
            {
                bTop = true;
                Char_Top.sprite = icon;
                NameBox_Top.text = name;
                NameBox_Top.transform.parent.gameObject.SetActive(true);
                NameBox_Bottom.transform.parent.gameObject.SetActive(false);
                Arrow_Top.gameObject.SetActive(true);
                Arrow_Bottom.gameObject.SetActive(false);
                if (!Top.gameObject.activeSelf)
                {
                    StartCoroutine(IFadeShow(POS_TOP));
                }
            }
            if (HasBottom(Pos))
            {
                bTop = false;
                Char_Bottom.sprite = icon;
                NameBox_Bottom.text = name;
                NameBox_Top.transform.parent.gameObject.SetActive(false);
                NameBox_Bottom.transform.parent.gameObject.SetActive(true);
                Arrow_Top.gameObject.SetActive(false);
                Arrow_Bottom.gameObject.SetActive(true);
                if (!Bottom.gameObject.activeSelf)
                {
                    StartCoroutine(IFadeShow(POS_BOTTOM));
                }
            }
        }

        void HideCharacter(int Pos)
        {
            if (HasTop(Pos) && Top.gameObject.activeSelf)
            {
                Top.gameObject.SetActive(false);
            }
            if (HasBottom(Pos) && Bottom.gameObject.activeSelf)
            {
                Bottom.gameObject.SetActive(false);
            }
        }
        bool AnalyseOneLine(string str)
        {
            if (str.StartsWith("+"))
            {
                string[] code = str.Substring(1).Split(',');
                int id, pos;
                int.TryParse(code[0], out id);
                int.TryParse(code[1], out pos);
                Sprite icon = ResourceManager.GetPlayerDef(id).Portrait;
                string name = ResourceManager.GetPlayerDef(id).CommonProperty.Name;
                ShowCharacter(pos, icon, name);
                return false;
            }
            else if (str.StartsWith("-"))
            {
                int pos;
                int.TryParse(str.Substring(1), out pos);
                HideCharacter(pos);
                return false;
            }
            else if (str.StartsWith("*"))
            {
                int pos;
                int.TryParse(str.Substring(1), out pos);
                setActive(pos);
                return false;
            }
            else if (str.StartsWith("#"))
            {
                string sCode = str.Substring(1);
                if (sCode == "end")
                    gameObject.SetActive(false);
                return false;
            }
            else
            {
                if (lastLineIndex != scriptLineIndex)//翻译的不是同一行
                {
                    StartCoroutine(ITyping(str));
                }
                return true;
            }
        }
        IEnumerator ITyping(string s)
        {
            int len = s.Length;
            bTyping = true;
            yield return new WaitForSeconds(0.3f);
            for (int i = 1; i <= len; i++)
            {
                if (bStopType)
                    break;
                string sContent = s.Substring(0, i);
                if (bTop)
                    ContentBox_Top.text = sContent;
                else
                    ContentBox_Bottom.text = sContent;
                yield return new WaitForSeconds(TypeTime);
            }
            PrintTalk(s);
            bTyping = false;
        }
        void PrintTalk(string sContent)
        {
            if (bTop)
                ContentBox_Top.text = sContent;
            else
                ContentBox_Bottom.text = sContent;
        }
        void setColor(int pos, Color color)
        {
            if (pos == POS_TOP)
            {
                Char_Top.color = color;
                ContentBox_Top.color = color;
                NameBox_Top.color = color;
            }
            if (pos == POS_BOTTOM)
            {
                Char_Bottom.color = color;
                ContentBox_Bottom.color = color;
                NameBox_Bottom.color = color;
            }
        }
        IEnumerator IFadeShow(int Pos)
        {
            if (HasTop(Pos))
            {
                Top.gameObject.SetActive(true);
                setColor(POS_TOP, COLOR_TRANS);
                for (int i = 60; i < 256; i += FadeSpeed)
                {
                    float a = (float)i / 255.0f;
                    Color col = new Color(Color.white.r, Color.white.g, Color.white.b, a);
                    setColor(POS_TOP, col);
                    Top.color = new Color(COLOR_TRANS.r, COLOR_TRANS.g, COLOR_TRANS.b, a - 56f / 255f);
                    yield return null;
                }
            }
            if (HasBottom(Pos))
            {
                Bottom.gameObject.SetActive(true);
                setColor(POS_BOTTOM, COLOR_TRANS);
                for (int i = 60; i < 256; i += FadeSpeed)
                {
                    float a = (float)i / 255.0f;
                    Color col = new Color(Color.white.r, Color.white.g, Color.white.b, a);
                    setColor(POS_BOTTOM, col);
                    Bottom.color = new Color(COLOR_TRANS.r, COLOR_TRANS.g, COLOR_TRANS.b, a - 56f / 255f);
                    yield return null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="fullyClose"> 是否是完全对话完毕或者跳过</param>
        /// <returns></returns>
        IEnumerator IFadeHide(int Pos, bool fullyClose)
        {
            if (HasTop(Pos))
            {
                setColor(POS_TOP, COLOR_WHITE);
                for (int i = 60; i < 256; i += FadeSpeed)
                {
                    float a = 1 - (float)i / 255.0f;
                    Color col = new Color(Color.white.r, Color.white.g, Color.white.b, a);
                    setColor(POS_TOP, col);
                    Top.color = new Color(COLOR_TRANS.r, COLOR_TRANS.g, COLOR_TRANS.b, a - 56f / 255f);
                    yield return null;
                }
            }
            if (HasBottom(Pos))
            {
                setColor(POS_BOTTOM, COLOR_WHITE);
                for (int i = 60; i < 256; i += FadeSpeed)
                {
                    float a = 1 - (float)i / 255.0f;
                    Color col = new Color(Color.white.r, Color.white.g, Color.white.b, a);
                    setColor(POS_BOTTOM, col);
                    Bottom.color = new Color(COLOR_TRANS.r, COLOR_TRANS.g, COLOR_TRANS.b, a - 56f / 255f);
                    yield return null;
                }
            }
            if (fullyClose)
            {
                Hide(true);
            }
        }
        public void Show(string[] talkData)
        {
            mTalkData = talkData;
            scriptLineIndex = 0;
            bTop = false;
            scriptLineIndex = 0;
            lastLineIndex = 0;
            bTyping = false;
            bStopType = false;
            TypeTime = ConstTable.SHOW_TEXT_TYPE_TIME();
            SoundManage.Instance.LowerBGM();
            gameObject.SetActive(true);
        }
    }
}