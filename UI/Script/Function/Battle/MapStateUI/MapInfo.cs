using UnityEngine.UI;

namespace RPG.UI
{
    public class MapInfo : IPanel
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
        public void Show(int TileID)
        {
            MapTileDef def = ResourceManager.GetMapDef();
            Avoid.text = def.GetAvoid(TileID).ToString();
            PhysicalDefence.text = def.GetPhysicalDefense(TileID).ToString();
            MagicalDefence.text= def.GetMagicalDefense(TileID).ToString();
            Recover.text= def.GetRecover(TileID).ToString();
            TileName.text = def.GetName(TileID).ToString();
            gameObject.SetActive(true);
        }
    }
}