using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Refer�ncia ao GameObject do menu de personaliza��o
    public GameObject roleText;
    public Button toggleCustomizationButton; // Refer�ncia ao bot�o para ativar/desativar o menu de personaliza��o
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    void Start()
    {
        CaptainCharacter.SetActive(false);
        roleText.SetActive(true);

        if (customizationMenu == null)
        {
            Debug.LogError("O GameObject customizationMenu n�o est� atribu�do!");
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O bot�o toggleCustomizationButton n�o est� atribu�do!");
        }
        else
        {
            // Adiciona o listener para chamar o m�todo ToggleCustomizationMenu quando o bot�o � pressionado
            toggleCustomizationButton.onClick.AddListener(ToggleCustomizationMenu);
        }

        // Verifica se o jogador atual � o capit�o
        if (IsCaptain())
        {
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capit�o
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
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
        int playerIndex = PhotonNetwork.PlayerList.Length; // Ou outro m�todo que defina o papel do capit�o
        return playerIndex == 2; // Exemplo: assume que o capit�o � o segundo jogador conectado
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
