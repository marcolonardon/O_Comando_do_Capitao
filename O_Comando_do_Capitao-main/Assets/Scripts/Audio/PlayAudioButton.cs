using UnityEngine;

public class PlayAudioButton : MonoBehaviour
{
    public AudioSource audioSource; // Arraste o AudioSource aqui pelo Inspector

    public void PlayAudio()
    {
        if (audioSource != null)
        {
            if (!audioSource.isPlaying) // Garante que o áudio não será sobreposto
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("AudioSource não foi atribuído no Inspector!");
        }
    }
}
