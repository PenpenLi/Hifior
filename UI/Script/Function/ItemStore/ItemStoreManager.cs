using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ItemStoreManager : MonoBehaviour
    {

        public ScrollRect ScrollRectComponent;
        public VerticalLayoutGroup LayoutGroup;
        public GameObject ItemPrefab;
        void Start()
        {
            for (int i = 0; i < 20; i++)
                addItem(i);
        }

        // Update is called once per frame
        void Update()
        {

        }
        void addItem(int itemID)//添加物体到Content下面
        {
            GameObject obj = Instantiate(ItemPrefab) as GameObject;
            obj.transform.SetParent(LayoutGroup.transform, false);//设为false否则会有问题
            obj.GetComponent<ItemElement>().SetItem(itemID);
        }
    }
}