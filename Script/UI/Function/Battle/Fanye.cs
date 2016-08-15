using UnityEngine;

namespace RPG.UI
{
    public class Fanye : MonoBehaviour
    {
        private int index = 0;
        public GameObject fanye0;
        public GameObject fanye1;
        private Animator anim0;
        private Animator anim1;
        void Start()
        {
            anim0 = fanye0.GetComponent<Animator>();
            anim1 = fanye1.GetComponent<Animator>();
        }
        public void ToLeft()
        {
            if (index == 0)
            {
                index = 1;
                anim0.Play("FanyeMidToLeft");
                anim1.Play("FanyeRightToMid");
                return;
            }
            if (index == 1)
            {
                index = 0;
                anim1.Play("FanyeMidToLeft");
                anim0.Play("FanyeRightToMid");
                return;
            }
        }
        public void ToRight()
        {
            if (index == 0)
            {
                index = 1;
                anim0.Play("FanyeMidToRight");
                anim1.Play("FanyeLeftToMid");
                return;
            }
            if (index == 1)
            {
                index = 0;
                anim1.Play("FanyeMidToRight");
                anim0.Play("FanyeLeftToMid");
                return;
            }
        }
    }
}