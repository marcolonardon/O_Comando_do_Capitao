using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioButtonControl : MonoBehaviour
{
    public AudioClip audioClip; // O áudio MP3 que será tocado
    public Button playButton;   // O botão que controla o áudio

    private AudioSource audioSource;

    void Start()
    {
        // Configura o AudioSource e atribui o clipe de áudio
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;

        // Adiciona o listener para o botão
        if (playButton != null)
        {
            playButton.onClick.AddListener(TogglePlayAudio);
        }
        else
        {
            Debug.LogError("Botão de play não está atribuído!");
        }
    }

    void TogglePlayAudio()
    {
        // Se o áudio está tocando, pare e reinicie do início
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.time = 0; // Reinicia o áudio
        }
        else
        {
            audioSource.Play(); // Inicia o áudio
        }
    }
}
