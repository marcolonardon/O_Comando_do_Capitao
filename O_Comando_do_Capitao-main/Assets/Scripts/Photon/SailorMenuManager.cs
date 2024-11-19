using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections; // Necessário para utilizar IEnumerator

[RequireComponent(typeof(AudioSource))]
public class SailorMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject sailor;
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
            // Concluir missão 1
            UnlockScreenText.text = "Iniciar Missão 2";
            CoinsAnimation.SetActive(true); // Ativa CoinsAnimation ao concluir a missão
            PlayAudio(missionOneCompleteAudio); // Toca o áudio da conclusão da missão 1
            StartCoroutine(DeactivateCoinsAnimationAfterDelay());
            isMissionOneCompleted = true;
            CompleteMissionButton.SetActive(false); // Oculta o botão após a conclusão da missão 1
        }
        else if (!isMissionTwoCompleted)
        {
            // Concluir missão 2
            CoinsAnimation.SetActive(true); // Ativa CoinsAnimation ao concluir a missão
            PlayAudio(missionTwoCompleteAudio); // Toca o áudio da conclusão da missão 2
            StartCoroutine(DeactivateCoinsAnimationAfterDelay());
            isMissionTwoCompleted = true;
            UnlockScreenButton.SetActive(false); // Desativa o botão ao finalizar todas as missões
            CompleteMissionButton.SetActive(false);
        }
    }

    // Método chamado pelo Capitão para mostrar o botão na tela do Sailor
    public void ShowStartButton()
    {
        if (IsCaptain())
        {
            photonView.RPC("ShowStartButtonRPC", RpcTarget.Others); // Chama o RPC para mostrar o botão no Sailor
        }
    }

    // RPC para mostrar o botão na tela do Sailor
    [PunRPC]
    private void ShowStartButtonRPC()
    {
        if (IsSailor())
        {
            UnlockScreenButton.SetActive(true);
            UnlockScreenText.text = "Iniciar Missão 1";
        }
    }

    public void StartMission()
    {
        if (IsSailor())
        {
            if (!isMissionOneCompleted)
            {
                // Iniciar missão 1
                BlockScreen.SetActive(false);
                CaptainCharacter.SetActive(true);
                CompleteMissionButton.SetActive(true); // Mostra o botão para finalizar missão 1
                CompleteMissionButtonText.text = "Finalizar Missão 1"; // Define o texto para Missão 1
                PlayAudio(missionOneStartAudio); // Toca o áudio da missão 1
                CaptainsButtons.SetActive(false); // Desativa os botões do Capitão
            }
            else if (isMissionOneCompleted && !isMissionTwoCompleted)
            {
                // Iniciar missão 2
                BlockScreen.SetActive(false);
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
        yield return new WaitForSeconds(6); // Define o tempo que a animação ficará ativa
        CoinsAnimation.SetActive(false); // Desativa CoinsAnimation após o delay
    }

    private bool IsCaptain()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1];
    }

    // Método para identificar se o jogador atual é o Sailor (terceiro a conectar)
    private bool IsSailor()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2];
    }
}
