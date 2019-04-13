using UnityEngine;

namespace RPG.UI
{
    public class HPPanel : IPanel
    {

        public BattleHPPanel Player;
        public BattleHPPanel Enemy;

        protected override void Awake()
        {
            base.Awake();

            Hide();
        }
        public void Show(RPGCharacter ch)
        {
            Show();
            if (ch.GetCamp() == EnumCharacterCamp.Player)
                Player.Show(ch.GetCharacterName(), ch.Logic().GetCurrentHP(), ch.Logic().GetMaxHP());
            else
                Enemy.Show(ch.GetCharacterName(), ch.Logic().GetCurrentHP(), ch.Logic().GetMaxHP());
        }
        public void Change(RPGCharacter ch, int ChangedHP, float delayTime = 0.0f)
        {
            if (ch.GetCamp()== EnumCharacterCamp.Player)
                Player.Change(ChangedHP, delayTime);
            else
                Enemy.Change(ChangedHP, delayTime);
        }
        public void Hide(EnumCharacterCamp control)
        {
            if (control == EnumCharacterCamp.Player)
                Player.Hide();
            else
                Enemy.Hide();
        }
    }
}