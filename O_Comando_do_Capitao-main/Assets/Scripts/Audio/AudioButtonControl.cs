using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioButtonControl : MonoBehaviour
{
    public AudioClip audioClip; // O �udio MP3 que ser� tocado
    public Button playButton;   // O bot�o que controla o �udio

    private AudioSource audioSource;

    void Start()
    {
        // Configura o AudioSource e atribui o clipe de �udio
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;

        // Adiciona o listener para o bot�o
        if (playButton != null)
        {
            playButton.onClick.AddListener(TogglePlayAudio);
        }
        else
        {
            Debug.LogError("Bot�o de play n�o est� atribu�do!");
        }
    }

    void TogglePlayAudio()
    {
        // Se o �udio est� tocando, pare e reinicie do in�cio
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.time = 0; // Reinicia o �udio
        }
        else
        {
            audioSource.Play(); // Inicia o �udio
        }
    }
}
