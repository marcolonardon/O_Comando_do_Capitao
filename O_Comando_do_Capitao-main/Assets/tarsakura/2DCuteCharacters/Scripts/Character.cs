using System.Collections.Generic;
using UnityEngine;

namespace tarsakura
{
    public class Character : MonoBehaviour
    {
        public SpriteRenderer bodySp, headSp, handRSp, handLSp, legRSp, legLSp;
        public Transform dressTf, headTf, hairTf, eyebrowRTf, eyebrowLTf, eyeRTf, eyeLTf, mouthTf, handRTf, handLTf, legRTf, legLTf;
        public enum Gender { male, female };
        public Gender gender = Gender.male;
        public CharacterPreset defaultPreset;
        public List<BodySpriteSet> bodySets;

        GameObject dress, hair, handR, handL, legR, legL, hat, beard, cheek, eyebrowR, eyebrowL, eyeR, eyeL, glasses;
        int dressIndex, hairIndex, skinColorIndex, hatIndex, beardIndex, eyebrowsIndex, eyesIndex, glassesIndex;
        bool cheeckValue;

        private void Start()
        {
            InitializePlayerPrefsDefaults();
            LoadCharacter();
            SetCharacter();
        }

        private void InitializePlayerPrefsDefaults()
        {
            PlayerPrefs.GetInt("Hair", 0);
            PlayerPrefs.GetInt("Hat", 0);
            PlayerPrefs.GetInt("Glasses", 0);
            PlayerPrefs.GetInt("SkinColor", 0);
            PlayerPrefs.GetInt("Eyebrows", 0);
            PlayerPrefs.GetInt("Eyes", 0);
            PlayerPrefs.GetInt("Beard", 0);
            PlayerPrefs.GetInt("Cheek", 0);
            PlayerPrefs.GetInt("Dress", 0);
        }

        private void LoadCharacter()
        {
            // Carregar valores salvos no PlayerPrefs ou usar valores padrão
            hairIndex = PlayerPrefs.GetInt("Hair", defaultPreset.hair);
            hatIndex = PlayerPrefs.GetInt("Hat", defaultPreset.hat);
            glassesIndex = PlayerPrefs.GetInt("Glasses", defaultPreset.glasses);
            skinColorIndex = PlayerPrefs.GetInt("SkinColor", defaultPreset.body);
            eyebrowsIndex = PlayerPrefs.GetInt("Eyebrows", defaultPreset.eyebrow);
            eyesIndex = PlayerPrefs.GetInt("Eyes", defaultPreset.eye);
            beardIndex = PlayerPrefs.GetInt("Beard", defaultPreset.beard);
            cheeckValue = PlayerPrefs.GetInt("Cheek", defaultPreset.cheek ? 1 : 0) == 1;
            dressIndex = PlayerPrefs.GetInt("Dress", defaultPreset.dress);
        }

        public void SetCharacter()
        {
            SkinColor = skinColorIndex;
            Dress = dressIndex;
            Hair = hairIndex;
            Eyebrows = eyebrowsIndex;
            Eyes = eyesIndex;
            Beard = beardIndex;
            Cheek = cheeckValue;
            Hat = hatIndex;
            Glasses = glassesIndex;
        }

        public int Hair
        {
            set
            {
                hairIndex = value;
                PlayerPrefs.SetInt("Hair", value); // Salvar no PlayerPrefs
                if (value != 0)
                {
                    if (hair != null) Destroy(hair); hair = null;
                    hair = Instantiate(Resources.Load<GameObject>("hair/" + value), hairTf);
                }
            }
            get { return hairIndex; }
        }

        public int Hat
        {
            set
            {
                hatIndex = value;
                PlayerPrefs.SetInt("Hat", value); // Salvar no PlayerPrefs
                if (value > 0)
                {
                    var obj = Resources.Load<GameObject>("hat/" + value);
                    if (obj == null) return;
                    if (hat != null) Destroy(hat); hat = null;
                    hat = Instantiate(obj, headTf);
                }
                else if (hat != null)
                {
                    Destroy(hat); hat = null;
                }
            }
            get { return hatIndex; }
        }

        public int Glasses
        {
            set
            {
                glassesIndex = value;
                PlayerPrefs.SetInt("Glasses", value); // Salvar no PlayerPrefs
                if (value > 0)
                {
                    var obj = Resources.Load<GameObject>("glasses/" + value);
                    if (obj == null) return;
                    if (glasses != null) Destroy(glasses); glasses = null;
                    glasses = Instantiate(obj, headTf);
                }
                else if (glasses != null)
                {
                    Destroy(glasses); glasses = null;
                }
            }
            get { return glassesIndex; }
        }

        public int SkinColor
        {
            set
            {
                skinColorIndex = value;
                PlayerPrefs.SetInt("SkinColor", value); // Salvar no PlayerPrefs
                var bodySet = bodySets[value];
                bodySp.sprite = bodySet.sprites[0];
                handRSp.sprite = handLSp.sprite = bodySet.sprites[1];
                headSp.sprite = bodySet.sprites[2];
                legRSp.sprite = legLSp.sprite = bodySet.sprites[3];
            }
            get { return skinColorIndex; }
        }

        public int Eyebrows
        {
            set
            {
                eyebrowsIndex = value;
                PlayerPrefs.SetInt("Eyebrows", value); // Salvar no PlayerPrefs
                var obj = Resources.Load<GameObject>("face/" + gender + "/eyebrow" + value);
                if (obj == null) return;
                if (eyebrowR != null) Destroy(eyebrowR); eyebrowR = null;
                if (eyebrowL != null) Destroy(eyebrowL); eyebrowL = null;
                eyebrowR = Instantiate(obj, eyebrowRTf);
                eyebrowL = Instantiate(obj, eyebrowLTf);
            }
            get { return eyebrowsIndex; }
        }

        public int Eyes
        {
            set
            {
                eyesIndex = value;
                PlayerPrefs.SetInt("Eyes", value); // Salvar no PlayerPrefs
                var obj = Resources.Load<GameObject>("face/" + gender + "/eyeR" + value);
                if (obj)
                {
                    if (eyeR != null) Destroy(eyeR); eyeR = null;
                    eyeR = Instantiate(obj, eyeRTf);
                }
                obj = Resources.Load<GameObject>("face/" + gender + "/eyeL" + value);
                if (obj)
                {
                    if (eyeL != null) Destroy(eyeL); eyeL = null;
                    eyeL = Instantiate(obj, eyeLTf);
                }
            }
            get { return eyesIndex; }
        }

        public int Beard
        {
            set
            {
                beardIndex = value;
                PlayerPrefs.SetInt("Beard", value); // Salvar no PlayerPrefs
                if (value > 0)
                {
                    var obj = Resources.Load<GameObject>("face/male/beard" + value);
                    if (obj == null) return;
                    if (beard != null) Destroy(beard); beard = null;
                    beard = Instantiate(obj, mouthTf);
                }
                else if (beard != null)
                {
                    Destroy(beard); beard = null;
                }
            }
            get { return beardIndex; }
        }

        public bool Cheek
        {
            set
            {
                cheeckValue = value;
                PlayerPrefs.SetInt("Cheek", value ? 1 : 0); // Salvar no PlayerPrefs
                if (value)
                {
                    if (cheek != null) Destroy(cheek); cheek = null;
                    cheek = Instantiate(Resources.Load<GameObject>("face/cheek"), headTf);
                }
                else if (cheek != null)
                {
                    Destroy(cheek); cheek = null;
                }
            }
            get { return cheeckValue; }
        }

        public int Dress
        {
            set
            {
                dressIndex = value;
                PlayerPrefs.SetInt("Dress", value); // Salvar no PlayerPrefs
                if (value != 0)
                {
                    GameObject obj;
                    if (dress != null) Destroy(dress); dress = null;
                    if (handR != null) Destroy(handR); handR = null;
                    if (handL != null) Destroy(handL); handL = null;
                    if (legR != null) Destroy(legR); legR = null;
                    if (legL != null) Destroy(legL); legL = null;
                    dress = Instantiate(Resources.Load<GameObject>("dress/" + value + "/dress"), dressTf);
                    obj = Resources.Load<GameObject>("dress/" + value + "/hand");
                    handR = Instantiate(obj, handRTf);
                    handL = Instantiate(obj, handLTf);
                    obj = Resources.Load<GameObject>("dress/" + value + "/leg");
                    legR = Instantiate(obj, legRTf);
                    legL = Instantiate(obj, legLTf);
                }
            }
            get { return dressIndex; }
        }
    }

    [System.Serializable]
    public class BodySpriteSet
    {
        public List<Sprite> sprites;
    }

    [System.Serializable]
    public class CharacterPreset
    {
        public int body = 0, face = 0, hair = 0, dress = 0, hat = 0, glasses = 0, eye = 0, eyebrow = 0, beard = 0;
        public bool cheek = false;
    }
}