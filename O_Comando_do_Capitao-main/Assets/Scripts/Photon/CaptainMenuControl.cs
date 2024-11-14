using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Referência ao GameObject do menu de personalização
    public GameObject roleText;
    public Button toggleCustomizationButton; // Referência ao botão para ativar/desativar o menu de personalização
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    void Start()
    {
        CaptainCharacter.SetActive(false);
        roleText.SetActive(true);

        if (customizationMenu == null)
        {
            Debug.LogError("O GameObject customizationMenu não está atribuído!");
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O botão toggleCustomizationButton não está atribuído!");
        }
        else
        {
            // Adiciona o listener para chamar o método ToggleCustomizationMenu quando o botão é pressionado
            toggleCustomizationButton.onClick.AddListener(ToggleCustomizationMenu);
        }

        // Verifica se o jogador atual é o capitão
        if (IsCaptain())
        {
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capitão
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
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
        int playerIndex = PhotonNetwork.PlayerList.Length; // Ou outro método que defina o papel do capitão
        return playerIndex == 2; // Exemplo: assume que o capitão é o segundo jogador conectado
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
