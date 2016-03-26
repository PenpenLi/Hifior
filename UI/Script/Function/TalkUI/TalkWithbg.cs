using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TalkWithbg : IPanel
    {
        private float TypeTime = 0.1f;

        public Image MainBackground;
        public Text TextBox;
        public Text TextNameBox;
        public Image arrow;

        public Image CharTalk_0;
        public Image CharTalk_1;
        public Image CharTalk_2;
        public Image CharTalk_3;


        Image[] CharTalk;
        private Color COLOR_DARK = new Color(0.7f, 0.7f, 0.7f, 1f);
        private Color COLOR_LIGHT = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private int scriptLineIndex = 0;
        private int lastLineIndex = 0;
        private bool bTyping = false;
        private bool bStopType = false;
        private bool bAutoText = false;
        private float updateStartTime = 2.0f;
        /*
       +8,1,0 设置人物id位置和头像，split获取数组，判断数组长度，若为3则新建并激活位置，为1则激活某个位置 <pp Portrait,Position
        #0 激活pos处的人物头像 <ap Active,Position
        -0 删除pos处的人物头像 <dp Delete,Position
        *1 更改激活位置pos头像 <cp Change,Position
        @0 抖动此处的头像      <sp Shake,Portrait
        %12,0 切换为12的背景,以方式0切换，默认为0，淡入淡出，还有其他卷轴式的切换方式 <cb Change,Background
        &1 切换音乐为1 <cm Change,Music
        ( 降低音量 <lv Lower,Volume
        ) 还原音量 <rv Reset,Volume
        =1,2 弹出YesNo面板,确定事件id,取消事件id <yn Yes,No 接收一个反馈的操作,传入一个函数,点击yes,执行一段什么,动态生成一个函数
        =fd=1,fd=2 弹出ListButton面板,前面显示,后面触发的事件 <lb List,Button
       */
        private string[] mTalkData = {
		//i代表人物id，p代表显示的位置，f代表显示的头像id
		"+0,1,0",
        "你好啊，我是米卡娅",//a代表要激活的位置
		"+1,2,0",
        "我叫艾克，是一名剑客",
        "*0",
        "你用剑啊，粗鲁的男人",
        "*2",
        "咕~~(╯﹏╰)b，好无辜的感觉啊"
    };


        public struct Portrait
        {
            public int position;
            public int charID;
            public Image image;
            public bool bActive;

            public Portrait(int position, int charID, Image image, bool bActive)
            {
                //如果要在结构中使用构造函数则必须给所有的变量赋值（在构造函数中赋值）
                this.position = position;
                this.charID = charID;
                this.image = image;
                this.bActive = bActive;
            }
        };
        Portrait[] portrait = {
        new Portrait (0, 0, null, false),
        new Portrait (1, 0, null, false),
        new Portrait (2, 0, null, false),
        new Portrait (3, 0, null, false)
    };

        static void DebugParamError(string div)
        {
            Debug.LogError("<" + div + "  参数数量不对应");
        }
        private void SetAllColor(Color col)
        {
            MainBackground.color = col;
            CharTalk_0.color = col;
            CharTalk_1.color = col;
            CharTalk_2.color = col;
            CharTalk_3.color = col;
        }
        protected override void Awake()
        {
            base.Awake();

            arrow.gameObject.SetActive(false);
            CharTalk = new Image[] { CharTalk_0, CharTalk_1, CharTalk_2, CharTalk_3 };
            for (int i = 0; i < 4; i++)
            {
                portrait[i].image = CharTalk[i].GetComponent<Image>(); //将portrait中的image赋值为对话框中的image
                CharTalk[i].gameObject.SetActive(false);//初始的四个对话框不显示
            }
            SetAllColor(Color.black);
            MainBackground.gameObject.SetActive(false);
        }
        void OnDisable()
        {
            SetTextFrameActive(false);
        }
        private void SetTextFrameActive(bool active)
        {
            TextNameBox.gameObject.SetActive(active);
            TextBox.gameObject.SetActive(active);
        }
        /*
        void Update()
        {
            updateStartTime -= Time.deltaTime;
            if (updateStartTime > 0f)
                return;
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
                SLGLevel.SLG._sound.NormalBGM();
                //StartCoroutine(fadeHide());
                Util.GameUtils.ScreenNormalToDark(1.0f, Hide);
            }
        }
        bool AnalyseOneLine(string str)
        {
            if (str.StartsWith("<"))
            {
                if (str.Length < 3)
                {
                    Debug.LogError("无法解析对话文本数据，指令长度过短");
                    return false;
                }
                string[] paramsStr = str.Substring(3).Trim().Split(',');
                switch (str.Substring(1, 2).ToLower())
                {
                    case "pp"://+8,1,0 设置人物id位置和头像，split获取数组，判断数组长度，若为3则新建并激活位置，为1则激活某个位置 Position,Portrait
                        if (paramsStr.Length != 3)
                            DebugParamError("pp");
                        else
                            ShowCharacter(int.Parse(paramsStr[0]), int.Parse(paramsStr[1]), int.Parse(paramsStr[2]));
                        break;
                    case "ap"://激活pos处的人物头像 Active,Position
                        if (paramsStr.Length != 1)
                            DebugParamError("ap");
                        else
                            ActivePosition(int.Parse(paramsStr[0]));
                        break;
                    case "rp"://删除pos处的人物头像 Delete,Position
                        if (paramsStr.Length != 1)
                            DebugParamError("rp");
                        else
                            HideCharacter(int.Parse(paramsStr[0]));
                        break;
                    case "sp"://抖动此处的头像 Shake,Portrait 
                        if (paramsStr.Length != 1)
                            DebugParamError("sp");
                        else
                            ShakePosition(int.Parse(paramsStr[0]));
                        break;
                    case "su"://抖动整个UI界面且
                        break;
                    case "cm"://更换音乐 Change,Music
                        if (paramsStr.Length != 1)
                            DebugParamError("cm");
                        else
                            SLGLevel.SLG._sound.PlayBGMImmediate(int.Parse(paramsStr[0]));
                        break;
                    case "lv"://降低音量 Lower,Volume
                        SLGLevel.SLG._sound.LowerBGM();
                        break;
                    case "rv"://重置音量 Reset,Volume
                        SLGLevel.SLG._sound.NormalBGM();
                        break;
                    case "cb"://以某种过渡方式更换背景 Change,Background
                        if (paramsStr.Length == 1)
                        {
                            //MainBackground.sprite=
                        }
                        else if (paramsStr.Length == 2)
                        {

                        }
                        else
                            DebugParamError("cb");
                        break;
                    case "at"://自动播放文本 Auto Text
                        bAutoText = true;
                        break;
                    case "sa"://停止自动文本显示 Stop Auto
                        bAutoText = false;
                        break;
                }
                return false;
            }

            else
            {
                if (lastLineIndex != scriptLineIndex)//翻译的不是同一行
                {
                    StartCoroutine(Typing(str));
                }
                return (!bAutoText);
            }
        }

        #region
        public void Show(string[] talkData)
        {
            mTalkData = talkData;
            SLGLevel.SLG._sound.LowerBGM();
            SLGLevel.SLG.cameraController.FadeOutDark(0.5f);
            gameObject.SetActive(true);
            Util.GameUtils.ScreenDarkToNormal();
        }
        void SetTalkName(int i) //Config NameBox Content
        {
            TextNameBox.text = Table._GameCharTable.getName(i);
        }
        void ShowCharacter(int characterID, int position, int faceID)
        {//此处通过Id更换头像和名称
            portrait[position].charID = characterID; //设置ID
            CharTalk[position].gameObject.SetActive(true);
            /// 通过id和faceID获取头像image
            portrait[position].image.sprite = GameCharIconGroup.GetTalkPortrait(characterID, faceID);
            ///
            ActivePosition(position);  //显示后再设置一下头像激活状态
        }
        void ActivePosition(int position)//设置位置的明暗度
        {
            for (int i = 0; i < 4; i++)
            {
                portrait[i].bActive = false;
                portrait[i].image.color = COLOR_DARK;
            }
            portrait[position].bActive = true;
            portrait[position].image.color = COLOR_LIGHT;
            SetTalkName(portrait[position].charID);
            SetTextFrameActive(true);
        }

        void HideCharacter(int position)
        {
            CharTalk[position].gameObject.SetActive(false);
        }

        void ShakePosition(int position)
        {
            CharTalk[position].GetComponent<Animation>().Play("UIChar_Shake");
        }
        #endregion

        void PrintTalk(string sContent)
        {
            arrow.gameObject.SetActive(false);
            TextBox.text = sContent;
        }
        IEnumerator Typing(string s)
        {
            int len = s.Length;
            bTyping = true;
            for (int i = 1; i <= len; i++)
            {
                if (bStopType)
                    break;
                TextBox.text = s.Substring(0, i);
                yield return new WaitForSeconds(TypeTime);
            }
            PrintTalk(s);
            bTyping = false;
        }
        IEnumerator fadeShow()
        {
            for (int i = 0; i < 256; i += 4)
            {
                float c = (float)i / 255.0f;
                Color col = new Color(c, c, c);
                setAllColor(col);
                yield return null;
            }
        }
        IEnumerator fadeHide()
        {
            for (int i = 255; i >= 0; i -= 4)
            {
                float c = (float)i / 255.0f;
                Color col = new Color(c, c, c);
                setAllColor(col);
                yield return null;
            }
            setAllColor(Color.black);
            MainBackground.gameObject.SetActive(false);
        }*/
    }
}