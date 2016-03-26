using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ValueBar : AbstractUI
    {
        public Image ParentImage;
        public Image CurrentImage;
        public Image ExtraImage;
        private RectTransform rPar;
        private RectTransform rChi;
        private RectTransform rExt;
        private float MaxAbility = 60.0f;
        private float barX;
        private float barY;
        Image hp;

        protected override void Awake()
        {
            base.Awake();

            rPar = ParentImage.GetComponent<RectTransform>();
            rChi = CurrentImage.GetComponent<RectTransform>();
            rExt = ExtraImage.GetComponent<RectTransform>();
            barX = rPar.sizeDelta.x;
            barY = rPar.sizeDelta.y;
        }

        // Update is called once per frame
        public void initBar(int Max, int Cur, int Extra)
        {
            rPar.sizeDelta = new Vector2((Max / MaxAbility) * barX, barY);
            rChi.sizeDelta = new Vector2((Cur / MaxAbility) * barX, barY);
            rExt.sizeDelta = new Vector2(((Cur + Extra) / MaxAbility) * barX, barY);
        }
        public void setMax(float f)
        {
            MaxAbility = f;
        }
    }
}