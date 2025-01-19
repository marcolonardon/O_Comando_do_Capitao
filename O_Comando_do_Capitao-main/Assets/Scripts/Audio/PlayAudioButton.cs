using UnityEngine;

public class PlayAudioButton : MonoBehaviour
{
    public AudioSource audioSource; // Arraste o AudioSource aqui pelo Inspector

    public void PlayAudio()
    {
        if (audioSource != null)
        {
            if (!audioSource.isPlaying) // Garante que o �udio n�o ser� sobreposto
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("AudioSource n�o foi atribu�do no Inspector!");
        }
    }
}
