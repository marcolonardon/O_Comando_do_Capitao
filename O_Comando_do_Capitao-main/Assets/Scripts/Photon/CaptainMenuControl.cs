using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject Background;
    public GameObject ShowCaptain;
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Refer�ncia ao GameObject do menu de personaliza��o
    public GameObject roleText;
    public GameObject PlayButton;
    public Button toggleCustomizationButton; // Refer�ncia ao bot�o para ativar/desativar o menu de personaliza��o
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    // Vari�vel para armazenar o ActorNumber do capit�o
    private int captainActorNumber;

    void Start()
    {
        SetCaptain();
        SetUI();
    }

    private void SetCaptain()
    {
        // Verifique se h� ao menos dois jogadores conectados para definir o capit�o como o segundo jogador
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            // Define o ActorNumber do capit�o como o segundo jogador na lista
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
            Debug.LogError("O GameObject customizationMenu n�o est� atribu�do!");
        }
        else
        {
            customizationMenu.SetActive(false);
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O bot�o toggleCustomizationButton n�o est� atribu�do!");
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
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capit�o
            ShowCaptain.SetActive(!isCustomizationMenuActive);
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
            CaptainCharacter.SetActive(false);
            ShowCaptain.SetActive(false);
        }
    }

    // M�todo para ativar/desativar o menu de personaliza��o
    public void ToggleCustomizationMenu()
    {
        if (IsCaptain())
        {
            isCustomizationMenuActive = !isCustomizationMenuActive;
            customizationMenu.SetActive(isCustomizationMenuActive);
            CaptainCharacter.SetActive(true);
            PlayButton.SetActive(!isCustomizationMenuActive);
            Debug.Log("Menu de personaliza��o " + (isCustomizationMenuActive ? "ativado" : "desativado"));
        }
        else
        {
            Debug.LogWarning("Apenas o capit�o pode ver o menu de personaliza��o.");
        }

        TurnUIOff();
    }

    // M�todo que verifica se o jogador atual � o capit�o
    private bool IsCaptain()
    {
        bool isCaptain = PhotonNetwork.LocalPlayer.ActorNumber == captainActorNumber;
        Debug.LogWarning("Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber + " | Capit�o ID: " + captainActorNumber + " | � Capit�o: " + isCaptain);
        return isCaptain;
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
