using UnityEngine.UI;

namespace RPG.UI
{
    public class UI_BattleTileInfo : IPanel
    {
        public Text Avoid;
        public Text PhysicalDefence;
        public Text MagicalDefence;
        public Text Recover;
        public Text TileName;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(ETileType TileID)
        {
            var v= FeTileData.TileInfos[TileID];
            Avoid.text =v.avoid.ToString();
            PhysicalDefence.text = v.phyDef.ToString();
            MagicalDefence.text= v.fireDef.ToString();
            Recover.text= v.recover.ToString();
            TileName.text = v.name.ToString();
            gameObject.SetActive(true);
        }
    }
}