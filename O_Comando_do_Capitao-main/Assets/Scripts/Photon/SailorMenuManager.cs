using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections; // Necess�rio para utilizar IEnumerator

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
    public GameObject CompleteMissionButton; // Bot�o para finalizar as miss�es
    public TMP_Text CompleteMissionButtonText; // Texto do bot�o para finalizar miss�es
    public GameObject CoinsAnimation;
    public GameObject CaptainsButtons;

    public AudioClip missionOneStartAudio;   // �udio para o in�cio da miss�o 1
    public AudioClip missionTwoStartAudio;   // �udio para o in�cio da miss�o 2
    public AudioClip missionOneCompleteAudio; // �udio para quando a miss�o 1 � conclu�da
    public AudioClip missionTwoCompleteAudio; // �udio para quando a miss�o 2 � conclu�da

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
        MissionButton.SetActive(false); // Bot�o "Iniciar Miss�o 1" come�a desativado
        CompleteMissionButton.SetActive(false);
        CaptainsButtons.SetActive(false);
        UnlockScreenButton.SetActive(false);

        if (IsSailor())
        {
            CaptainCharacter.SetActive(false);
            BlockScreen.SetActive(true);
            sailor.SetActive(true);
            UnlockScreenButton.SetActive(true); // Bot�o "Desbloquear tablet" ativo para o Sailor
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
            UnlockScreenButton.SetActive(false);  // Desativa o bot�o "Desbloquear tablet"
            MissionButton.SetActive(true);       // Ativa o bot�o "Iniciar Miss�o 1"
            MissionButtonText.text = "Iniciar Miss�o 1"; // Define o texto do bot�o
            BlockScreen.SetActive(false);        // Remove o bloqueio da tela
            CaptainCharacter.SetActive(true);    // Ativa o personagem do Capit�o
        }
    }


    public void StartMissionButton()
    {
        if (IsSailor())
        {
            MissionButton.SetActive(false); // Desativa o bot�o "Iniciar Miss�o 1" ap�s ser clicado
            StartMission(); // Inicia a l�gica da miss�o
        }
    }

    private void StartMission()
    {
        if (IsSailor())
        {
            if (!isMissionOneCompleted)
            {
                CompleteMissionButtonText.text = "Finalizar Miss�o 1"; // Define o texto para Miss�o 1
                PlayAudio(missionOneStartAudio); // Toca o �udio da miss�o 1
            }
            else if (isMissionOneCompleted && !isMissionTwoCompleted)
            {
                CompleteMissionButton.SetActive(true); // Mostra o bot�o para finalizar miss�o 2
                CompleteMissionButtonText.text = "Finalizar Miss�o 2"; // Define o texto para Miss�o 2
                PlayAudio(missionTwoStartAudio); // Toca o �udio da miss�o 2
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
        yield return new WaitForSeconds(7); // Define o tempo que a anima��o ficar� ativa
        CoinsAnimation.SetActive(false); // Desativa CoinsAnimation ap�s o delay
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
