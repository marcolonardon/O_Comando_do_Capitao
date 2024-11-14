using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace tarsakura {

    public class DressSlot:MonoBehaviour {

        [SerializeField] Image image;
        [SerializeField] GameObject selected;

        [HideInInspector]
        public int index;
        [HideInInspector]
        public AvatarScroller scroller;

        public void LoadSprite(string pathCate) {
            image.sprite = Resources.Load<Sprite>(pathCate + "/" + (index+1));
        }

        public void SetPosition(Vector2 position) {
            ((RectTransform)transform).anchoredPosition = position;
        }

        public void Select(bool active = true) {
            selected.SetActive(true);
            if(active) scroller.SelectSlot(this);
        }

        public void Deselect() {
            selected.SetActive(false);
        }
    }
}
