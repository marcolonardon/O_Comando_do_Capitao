using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace tarsakura {

    public class AvatarManager:MonoBehaviour {

        public int currentFaceIndex = 0;
        public List<FaceSet> faceSets;

        public Character character;
        [SerializeField] List<Button> buttons;
        [SerializeField] AvatarScroller[] scrollers;

        string currentCategory = "";
        AvatarScroller currentScroller;

        void Awake() {
            for(int i = 0; i < buttons.Count; i++) {
                buttons[i].onClick.AddListener(onChooseCategory);
                scrollers[i].manager = this;
                if(i == 0) {
                    currentScroller = scrollers[i];
                    currentScroller.Show();
                } else scrollers[i].Hide();
            }
        }

        private void Start() {
            SetCharacter("face", currentFaceIndex);
        }

        private void onChooseCategory() {
            if(currentCategory == EventSystem.current.currentSelectedGameObject.name) return;
            Button b = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if(b == null) return;
            int i = buttons.IndexOf(b);
            if(i == -1) return;
            currentCategory = EventSystem.current.currentSelectedGameObject.name;
            if(currentScroller) currentScroller.Hide();
            currentScroller = scrollers[i];
            currentScroller.Show();
        }

        public void SetCharacter(string category, int index) {
            if(category == "face") {
                currentFaceIndex = index;
                FaceSet face = faceSets[index];
                character.gender = (Character.Gender)face.gender;
                character.Cheek = (character.gender == Character.Gender.female);
                character.Eyes = face.eye;
                character.Eyebrows = face.eyebrow;
                character.Beard = face.beard;
            } else if(category == "hair") {
                character.Hair = index;
            } else if(category == "hat") {
                character.Hat = index;
            } else if(category == "glasses") {
                character.Glasses = index;
            } else if(category == "dress") {
                character.Dress = index;
            }
        }

        public void ChangeSkinColor(int index) {
            character.SkinColor = index;
        }

    }

    [System.Serializable]
    public class FaceSet {
        public Sprite sprite;
        public int gender = 0, eye = 0, eyebrow = 0, beard = 0;
    }
}
