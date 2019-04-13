using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AttackStatePanel : IPanel
    {
        public Text tName0;
        public Text tWeaponName0;
        public Image iWeaponIcon0;
        public Text tHP0;
        public Text tDamage0;
        public Text tHit0;
        public Text tCrit0;

        public Text tName1;
        public Text tWeaponName1;
        public Image iWeaponIcon1;
        public Text tHP1;
        public Text tDamage1;
        public Text tHit1;
        public Text tCrit1;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public new void Init()
        {
            /*RPGPlayer ch = SLGLevel.SLG.getCurrentSelectGameChar();
            RPGPlayer en = SLGLevel.SLG.getCurrentSelectEnemy();
            tName0.text = ch.attribute.Name;
            WeaponItem item = ch.ItemGroup.getEquipItem();
            int itemID =item.ID;
            tWeaponName0.text = Table._ItemTable.getName(itemID);
            iWeaponIcon0.sprite = item.Icon;
            tHP0.text = ch.attribute.CurHP.ToString();
            tDamage0.text = GetDamage(ch, en);
            tHit0.text = GetHit(ch, en);
            tCrit0.text = GetCritical(ch, en);

            tName1.text = en.attribute.Name;
            item = en.ItemGroup.getEquipItem();
            itemID =item.ID;
            tWeaponName1.text = Table._ItemTable.getName(itemID);
            iWeaponIcon1.sprite = item.Icon;
            tHP1.text = en.attribute.CurHP.ToString();
            int len = Mathf.Abs(ch.TileCoords.x - en.TileCoords.x) + Mathf.Abs(ch.TileCoords.y - en.TileCoords.y);
            if (len >= Table._ItemTable.getRangeMin(en.ItemGroup.getEquipItem().ID) && len <= Table._ItemTable.getRangeMax(en.ItemGroup.getEquipItem().ID))
            {
                tDamage1.text = GetDamage(en, ch);
                tHit1.text = GetHit(en, ch);
                tCrit1.text = GetCritical(en, ch);
            }
            else
            {
                tDamage1.text = "--";
                tHit1.text = "--";
                tCrit1.text = "--";
            }*/

        }
        public override void Show()
        {
            Init();
            gameObject.SetActive(true);
        }

        public string GetDamage(RPGCharacter player, RPGCharacter enemy)
        {
            /*string s;
            int damage = 0;
            int t =player.Item.GetEquipItem().GetDefinition().WeaponType;
            if (t > 0 && t <= 4) damage = ch.getAttack() - enemy.attribute.DEFALL;
            if (t > 4 && t <= 8) damage = ch.getAttack() - enemy.attribute.RESALL;
            if (t > 8)
                damage = (ch.getAttack() - enemy.attribute.DEFALL) + (ch.getAttack() - enemy.attribute.RESALL);
            if (damage < 0) damage = 0;
            s = damage.ToString();
            if (ch.getAttackSpeed() - enemy.getAttackSpeed() > SLGBattle.ZHUIJI_SPEED)
                s = damage + " X 2";
            return s;*/
            return null;
        }
        public string GetHit(RPGCharacter ch, RPGCharacter enemy)
        {
            int hit = ch.Logic().GetHit() - enemy.Logic().GetAvoid();
            if (hit < 0) hit = 0;
            return hit.ToString();
        }
        public string GetCritical(RPGCharacter ch, RPGCharacter enemy)
        {
            int cri = ch.Logic().GetCritical();
            int criavoid = enemy.Logic().GetCriticalAvoid();
            int critical = cri - criavoid;
            if (critical < 0) critical = 0;
            return critical.ToString();
        }
    }
}
