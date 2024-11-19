using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections; // Necess�rio para utilizar IEnumerator

[RequireComponent(typeof(AudioSource))]
public class SailorMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject sailor;
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
        sailor.SetActive(false);
        CoinsAnimation.SetActive(false);
        UnlockScreenButton.SetActive(false);
        CompleteMissionButton.SetActive(false);
        CaptainsButtons.SetActive(false);

        if (IsSailor())
        {
            CaptainCharacter.SetActive(false);
            BlockScreen.SetActive(true);
            sailor.SetActive(true);
        }
        else
        {
            BlockScreen.SetActive(false);
        }
    }

    public void CompleteMission()
    {
        if (!isMissionOneCompleted)
        {
            // Concluir miss�o 1
            UnlockScreenText.text = "Iniciar Miss�o 2";
            CoinsAnimation.SetActive(true); // Ativa CoinsAnimation ao concluir a miss�o
            PlayAudio(missionOneCompleteAudio); // Toca o �udio da conclus�o da miss�o 1
            StartCoroutine(DeactivateCoinsAnimationAfterDelay());
            isMissionOneCompleted = true;
            CompleteMissionButton.SetActive(false); // Oculta o bot�o ap�s a conclus�o da miss�o 1
        }
        else if (!isMissionTwoCompleted)
        {
            // Concluir miss�o 2
            CoinsAnimation.SetActive(true); // Ativa CoinsAnimation ao concluir a miss�o
            PlayAudio(missionTwoCompleteAudio); // Toca o �udio da conclus�o da miss�o 2
            StartCoroutine(DeactivateCoinsAnimationAfterDelay());
            isMissionTwoCompleted = true;
            UnlockScreenButton.SetActive(false); // Desativa o bot�o ao finalizar todas as miss�es
            CompleteMissionButton.SetActive(false);
        }
    }

    // M�todo chamado pelo Capit�o para mostrar o bot�o na tela do Sailor
    public void ShowStartButton()
    {
        if (IsCaptain())
        {
            photonView.RPC("ShowStartButtonRPC", RpcTarget.Others); // Chama o RPC para mostrar o bot�o no Sailor
        }
    }

    // RPC para mostrar o bot�o na tela do Sailor
    [PunRPC]
    private void ShowStartButtonRPC()
    {
        if (IsSailor())
        {
            UnlockScreenButton.SetActive(true);
            UnlockScreenText.text = "Iniciar Miss�o 1";
        }
    }

    public void StartMission()
    {
        if (IsSailor())
        {
            if (!isMissionOneCompleted)
            {
                // Iniciar miss�o 1
                BlockScreen.SetActive(false);
                CaptainCharacter.SetActive(true);
                CompleteMissionButton.SetActive(true); // Mostra o bot�o para finalizar miss�o 1
                CompleteMissionButtonText.text = "Finalizar Miss�o 1"; // Define o texto para Miss�o 1
                PlayAudio(missionOneStartAudio); // Toca o �udio da miss�o 1
                CaptainsButtons.SetActive(false); // Desativa os bot�es do Capit�o
            }
            else if (isMissionOneCompleted && !isMissionTwoCompleted)
            {
                // Iniciar miss�o 2
                BlockScreen.SetActive(false);
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
        yield return new WaitForSeconds(6); // Define o tempo que a anima��o ficar� ativa
        CoinsAnimation.SetActive(false); // Desativa CoinsAnimation ap�s o delay
    }

    private bool IsCaptain()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1];
    }

    // M�todo para identificar se o jogador atual � o Sailor (terceiro a conectar)
    private bool IsSailor()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2];
    }
}
