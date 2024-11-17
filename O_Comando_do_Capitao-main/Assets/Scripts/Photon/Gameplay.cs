using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Gameplay : MonoBehaviourPunCallbacks
{
    //public string[] speechTexts;
    //private int currentSpeechIndex = 0;

    public GameObject Background;
    public GameObject CaptainCharacter;
    public GameObject CaptainPlayButton;
    public GameObject NextSpeechButton;
    public GameObject WelcomeButton;
    //public GameObject CaptainSpeechBubble;
    //public TMP_Text CaptainSpeechText;

    public GameObject CaptainsCommandsButtons;

    void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        //if (CaptainSpeechBubble != null) CaptainSpeechBubble.SetActive(false);
        if(WelcomeButton != null) WelcomeButton.SetActive(false);
        //if(Background != null) Background.SetActive(false);
        if(NextSpeechButton != null) NextSpeechButton.SetActive(false);
        if(CaptainsCommandsButtons != null) CaptainsCommandsButtons.SetActive(false);
    }

    public void CaptainButton()
    {
        if (IsCaptain() || IsSailor())
        {
            photonView.RPC("ShowSpeechBubble", RpcTarget.All, "Mensagem do Capitão");
            CaptainPlayButton.SetActive(false); // Desativa localmente o botão do Capitão
        }
    }

    // Método RPC que é chamado para mostrar a speech bubble para o Capitão e o Sailor
    [PunRPC]
    private void ShowSpeechBubble(string message)
    {
        if (IsCaptain())
        {
            Background.SetActive(true);
            CaptainCharacter.SetActive(true);
            WelcomeButton.SetActive(true);  
            //CaptainSpeechBubble.SetActive(true);
            NextSpeechButton.SetActive(true);
            //CaptainSpeechText.text = speechTexts[currentSpeechIndex];
        }
        else if(IsSailor()) 
        {
            Background.SetActive(true);
            CaptainCharacter.SetActive(false);

            NextSpeechButton.SetActive(false);
            WelcomeButton.SetActive(false);
            
        }
        else
        {
            Background.SetActive(false);
            CaptainCharacter.SetActive(false);

            NextSpeechButton.SetActive(false);
            WelcomeButton.SetActive(false);
        }
    }

    public void NextSpeechText()
    {
      // int size = speechTexts.Length;
      // if (currentSpeechIndex < size - 1)
      // {
      //     currentSpeechIndex++;
      // }
      // else
      // {
      //     currentSpeechIndex = 0;
      // }
      //
      // ShowSpeechBubble(speechTexts[currentSpeechIndex]);
    }

    public void CaptainsCommands()
    {
        SetUI();
        CaptainsCommandsButtons.SetActive(true);
        Background.SetActive(true);
    }

    // Método para identificar se o jogador atual é o Capitão (segundo a conectar)
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
