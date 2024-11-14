using UnityEngine;

namespace tarsakura {

    public class RandomCharacter:MonoBehaviour {

        public Character[] characters;

        public void Random() {
            for (int i = 0; i < characters.Length; i++) {
                var character = characters[i];
                character.gender = (Character.Gender)UnityEngine.Random.Range(0, 2);
                character.SkinColor = UnityEngine.Random.Range(0, 3);
                character.Dress = UnityEngine.Random.Range(0, 34);
                character.Hair = UnityEngine.Random.Range(0, 34);
                character.Eyebrows = UnityEngine.Random.Range(0, 10);
                character.Eyes = UnityEngine.Random.Range(0, 10);
                character.Beard = (UnityEngine.Random.Range(0, 2) == 0) ? 0 : UnityEngine.Random.Range(0, 8);
                character.Cheek = (UnityEngine.Random.Range(0, 2) == 0);
                character.Hat = UnityEngine.Random.Range(0, 33);
                character.Glasses = UnityEngine.Random.Range(0, 16);
            }
        }

    }

}
