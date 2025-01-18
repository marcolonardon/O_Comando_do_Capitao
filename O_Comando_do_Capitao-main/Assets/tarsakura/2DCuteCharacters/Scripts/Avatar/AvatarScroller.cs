using System.Collections.Generic;
using UnityEngine;

namespace tarsakura {

    public class AvatarScroller:MonoBehaviour {

        public string category = "";
        public bool toggleable = false;
        [SerializeField] DressSlot slotPrefeb;
        [SerializeField] RectTransform content;

        [HideInInspector]
        public AvatarManager manager;

        List<DressSlot> slots = new List<DressSlot>();
        DressSlot currentSlot;

        private void Start() {
            SetUpList();
        }

        public void SetUpList() {
            var length = 0;
            if (category == "face") length = 3;
            else if (category == "hair") length = 33;
            else if (category == "hat") length = 32;
            else if (category == "glasses") length = 16;
            else length = 33;

            if(length == slots.Count) return;
            int page, c, r, ii;
            float pageWidth = 215f * 4;
            DressSlot slot;
            for(int i = 0; i < length; i++) {
                page = Mathf.FloorToInt(i / 16f);
                ii = i - (page * 16);
                c = ii % 4;
                r = Mathf.FloorToInt(ii / 4f);
                slot = Instantiate(slotPrefeb, content);
                slot.index = i;
                slot.scroller = this;
                slot.LoadSprite(category + "/sprites");
                slot.SetPosition(new Vector2((95f + (pageWidth * page)) + (c * 215f), -85f - (r * 195f)));
                slots.Add(slot);
                if(category == "face") {
                    if(manager.currentFaceIndex == slot.index) currentSlot = slot;
                    else slot.Deselect();
                } else if(category == "hair") {
                    if(manager.character.Hair == slot.index+1) currentSlot = slot;
                    else slot.Deselect();
                } else if(category == "hat") {
                    if(manager.character.Hat == slot.index+1) currentSlot = slot;
                    else slot.Deselect();
                } else if(category == "glasses") {
                    if(manager.character.Glasses == slot.index+1) currentSlot = slot;
                    else slot.Deselect();
                } else if(category == "dress") {
                    if(manager.character.Dress == slot.index+1) currentSlot = slot;
                    else slot.Deselect();
                }
            }
            if(currentSlot) currentSlot.Select(false);
            Vector2 s = content.sizeDelta;
            int n = (length % 16);
            c = (n > 4) ? 4 : n;
            s.x = (Mathf.FloorToInt(length / 16f) * pageWidth) + (215f * c);
            content.sizeDelta = s;
        }

        public void ClearSlots() {
            while(slots.Count > 0) {
                Destroy(slots[0].gameObject);
                slots.RemoveAt(0);
            }
        }

        public void SelectSlot(DressSlot slot) {
            if(!toggleable && currentSlot && currentSlot == slot) return;
            if(currentSlot) currentSlot.Deselect();
            if(toggleable && currentSlot && currentSlot == slot) {
                currentSlot = null;
                manager.SetCharacter(category, 0);
            } else {
                currentSlot = slot;
                manager.SetCharacter(category, (category == "face") ? slot.index : slot.index + 1);
            }
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void Show() {
            gameObject.SetActive(true);
        }
    }
}
