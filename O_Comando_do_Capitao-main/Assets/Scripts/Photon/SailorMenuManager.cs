using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections; // Necessário para utilizar IEnumerator

[RequireComponent(typeof(AudioSource))]
public class SailorMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject sailor;
    public GameObject MissionButton;
    public TMP_Text MissionButtonText;
    public GameObject UnlockScreenButton;
    public TMP_Text UnlockScreenText;
    public GameObject BlockScreen;
    public GameObject CaptainCharacter;
    public GameObject CompleteMissionButton; // Botão para finalizar as missões
    public TMP_Text CompleteMissionButtonText; // Texto do botão para finalizar missões
    public GameObject CoinsAnimation;
    public GameObject CaptainsButtons;

    public AudioClip missionOneStartAudio;   // Áudio para o início da missão 1
    public AudioClip missionTwoStartAudio;   // Áudio para o início da missão 2
    public AudioClip missionOneCompleteAudio; // Áudio para quando a missão 1 é concluída
    public AudioClip missionTwoCompleteAudio; // Áudio para quando a missão 2 é concluída

    private AudioSource audioSource;
    private bool isMissionOneCompleted = false;
    private bool isMissionTwoCompleted = false;

    private void Start()
    {
        SetUI();
        audioSource = GetComponent<AudioSource>();
    }

    private void SetUI()
    {
        UnlockScreenText.text = "Desbloquear tablet";
        sailor.SetActive(false);
        CoinsAnimation.SetActive(false);
        MissionButton.SetActive(false); // Botão "Iniciar Missão 1" começa desativado
        CompleteMissionButton.SetActive(false);
        CaptainsButtons.SetActive(false);
        UnlockScreenButton.SetActive(false);

        if (IsSailor())
        {
            CaptainCharacter.SetActive(false);
            BlockScreen.SetActive(true);
            sailor.SetActive(true);
            UnlockScreenButton.SetActive(true); // Botão "Desbloquear tablet" ativo para o Sailor
        }
        else
        {
            BlockScreen.SetActive(false);
        }
    }

    public void UnlockSailorScreen()
    {
        if (IsSailor())
        {
            UnlockScreenButton.SetActive(false);  // Desativa o botão "Desbloquear tablet"
            MissionButton.SetActive(true);       // Ativa o botão "Iniciar Missão 1"
            MissionButtonText.text = "Iniciar Missão 1"; // Define o texto do botão
            BlockScreen.SetActive(false);        // Remove o bloqueio da tela
            CaptainCharacter.SetActive(true);    // Ativa o personagem do Capitão
        }
    }


    public void StartMissionButton()
    {
        if (IsSailor())
        {
            MissionButton.SetActive(false); // Desativa o botão "Iniciar Missão 1" após ser clicado
            StartMission(); // Inicia a lógica da missão
        }
    }

    private void StartMission()
    {
        if (IsSailor())
        {
            if (!isMissionOneCompleted)
            {
                CompleteMissionButtonText.text = "Finalizar Missão 1"; // Define o texto para Missão 1
                PlayAudio(missionOneStartAudio); // Toca o áudio da missão 1
            }
            else if (isMissionOneCompleted && !isMissionTwoCompleted)
            {
                CompleteMissionButton.SetActive(true); // Mostra o botão para finalizar missão 2
                CompleteMissionButtonText.text = "Finalizar Missão 2"; // Define o texto para Missão 2
                PlayAudio(missionTwoStartAudio); // Toca o áudio da missão 2
            }
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private IEnumerator DeactivateCoinsAnimationAfterDelay()
    {
        yield return new WaitForSeconds(7); // Define o tempo que a animação ficará ativa
        CoinsAnimation.SetActive(false); // Desativa CoinsAnimation após o delay
    }

    private bool IsCaptain()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1];
    }

    private bool IsSailor()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2];
    }
}
