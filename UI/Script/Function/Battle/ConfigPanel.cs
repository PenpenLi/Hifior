using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ConfigPanel : IPanel
    {
        public UnityEngine.UI.Button[] Buttons;

        private static Color select_color = new Color(0f / 255f, 159f / 255f, 87f / 255f, 1.0f);
        private static Color notselect_color = new Color(111f / 255f, 111f / 255f, 111f / 255f, 1.0f);
        public Slider Lattice;
        public Slider BGMVolume;
        public Slider EffectVolume;
        public Slider VoiceVolume;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        void OnDisable()//保存设置
        {
            onExitConfig();
            OptionConfig.SaveConfig();
        }
        public void init()//从存档获取是否有可用的设置，没有则使用初始设定
        {
            //如果存档没有可用信息
            if (OptionConfig.ReadConfig())
            {
                战斗动画设定(OptionConfig.CONFIG_ANIMATION);
                战斗视角设定(OptionConfig.CONFIG_VIEWPORT);
                游戏速度(OptionConfig.CONFIG_GAMESPEED);
                信息速度(OptionConfig.CONFIG_INFOSPEED);
                地形窗口(OptionConfig.CONFIG_TERRAININFO);
                地图上HP槽显示(OptionConfig.CONFIG_HPBARSHOW);
                自动回合结束(OptionConfig.CONFIG_AUTOFINISHTURN);
                信息效果声音(OptionConfig.CONFIG_INFOEFFECTSOUND);
                格子浓度(OptionConfig.CONFIG_LATTICE);
                BGM(OptionConfig.CONFIG_BGMSOUNDVOLUME);
                效果音(OptionConfig.CONFIG_EFFECTSOUNDVOLUME);
                人物音(OptionConfig.CONFIG_VOICESOUNDVOLUME);
            }
            else
            {
                setDefault();
            }
        }
        public override void Show()
        {
            init();
            this.gameObject.SetActive(true);
        }
        public void onExitConfig()
        {
            //结束时在进行数据赋值
            格子浓度((int)Lattice.value);
            BGM((int)BGMVolume.value);
            效果音((int)EffectVolume.value);
            人物音((int)VoiceVolume.value);
        }
        public void 战斗动画设定(int i)//改变点击后的Text颜色
        {
            OptionConfig.CONFIG_ANIMATION = i;
            refreshView(0, i);
        }
        public void 战斗视角设定(int i)
        {
            OptionConfig.CONFIG_VIEWPORT = i;
            refreshView(1, i);
        }
        public void 游戏速度(int i)
        {
            OptionConfig.CONFIG_GAMESPEED = i;
            refreshView(2, i);
        }
        public void 信息速度(int i)
        {
            OptionConfig.CONFIG_INFOSPEED = i;
            refreshView(3, i);
        }
        public void 地形窗口(int i)
        {
            OptionConfig.CONFIG_TERRAININFO = i;
            refreshView(4, i);
        }
        public void 地图上HP槽显示(int i)
        {
            OptionConfig.CONFIG_HPBARSHOW = i;
            refreshView(5, i);
        }
        public void 自动回合结束(int i)
        {
            OptionConfig.CONFIG_AUTOFINISHTURN = i;
            refreshView(6, i);
        }
        public void 信息效果声音(int i)
        {
            OptionConfig.CONFIG_INFOEFFECTSOUND = i;
            refreshView(7, i);
        }
        public void 格子浓度(int i)
        {
            OptionConfig.CONFIG_LATTICE = i;
            Lattice.value = i;
        }
        public void BGM(int i)//改变AudioSource的音量
        {
            OptionConfig.CONFIG_BGMSOUNDVOLUME = i;
            BGMVolume.value = i;
        }
        public void 效果音(int i)
        {
            OptionConfig.CONFIG_EFFECTSOUNDVOLUME = i;
            EffectVolume.value = i;
        }
        public void 人物音(int i)
        {
            OptionConfig.CONFIG_VOICESOUNDVOLUME = i;
            VoiceVolume.value = i;
        }
        public void setDefault()
        {
            战斗动画设定(0);
            战斗视角设定(0);
            游戏速度(0);
            信息速度(0);
            格子浓度(6);
            地形窗口(0);
            地图上HP槽显示(0);
            自动回合结束(0);
            信息效果声音(0);
            BGM(10);
            效果音(10);
            人物音(10);
        }
        public void refreshView(int buttonsIndex, int i)//从ConstTable中获取并改变显示
        {

            Buttons[buttonsIndex].transform.GetChild(i + 1).GetComponent<Text>().color = select_color;
            for (int j = 1; j < Buttons[buttonsIndex].transform.childCount; j++)
            {
                if (j == i + 1)
                    continue;
                Buttons[buttonsIndex].transform.GetChild(j).GetComponent<Text>().color = notselect_color;
            }
        }
    }
}