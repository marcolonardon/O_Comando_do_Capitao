using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject Background;
    public GameObject ShowCaptain;
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Referência ao GameObject do menu de personalização
    public GameObject roleText;
    public GameObject PlayButton;
    public Button toggleCustomizationButton; // Referência ao botão para ativar/desativar o menu de personalização
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    // Variável para armazenar o ActorNumber do capitão
    private int captainActorNumber;

    void Start()
    {
        SetCaptain();
        SetUI();
    }

    private void SetCaptain()
    {
        // Verifique se há ao menos dois jogadores conectados para definir o capitão como o segundo jogador
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            // Define o ActorNumber do capitão como o segundo jogador na lista
            captainActorNumber = PhotonNetwork.PlayerList[1].ActorNumber;
        }
    }

    private void SetUI()
    {
        CaptainCharacter.SetActive(false);
        roleText.SetActive(true);
        Background.SetActive(true); 


        if (customizationMenu == null)
        {
            Debug.LogError("O GameObject customizationMenu não está atribuído!");
        }
        else
        {
            customizationMenu.SetActive(false);
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O botão toggleCustomizationButton não está atribuído!");
        }
        else
        {
            toggleCustomizationButton.onClick.AddListener(ToggleCustomizationMenu);
        }

        if (PlayButton != null)
        {
            PlayButton.SetActive(false);
        }

        if (IsCaptain())
        {
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capitão
            ShowCaptain.SetActive(!isCustomizationMenuActive);
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
            CaptainCharacter.SetActive(false);
            ShowCaptain.SetActive(false);
        }
    }

    // Método para ativar/desativar o menu de personalização
    public void ToggleCustomizationMenu()
    {
        if (IsCaptain())
        {
            isCustomizationMenuActive = !isCustomizationMenuActive;
            customizationMenu.SetActive(isCustomizationMenuActive);
            CaptainCharacter.SetActive(true);
            PlayButton.SetActive(!isCustomizationMenuActive);
            Debug.Log("Menu de personalização " + (isCustomizationMenuActive ? "ativado" : "desativado"));
        }
        else
        {
            Debug.LogWarning("Apenas o capitão pode ver o menu de personalização.");
        }

        TurnUIOff();
    }

    // Método que verifica se o jogador atual é o capitão
    private bool IsCaptain()
    {
        bool isCaptain = PhotonNetwork.LocalPlayer.ActorNumber == captainActorNumber;
        Debug.LogWarning("Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber + " | Capitão ID: " + captainActorNumber + " | É Capitão: " + isCaptain);
        return isCaptain;
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
