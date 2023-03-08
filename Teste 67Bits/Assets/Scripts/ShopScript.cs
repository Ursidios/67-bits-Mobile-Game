using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private int Moedas;
    [SerializeField] private int ContadorEmpilhadeira;
    [SerializeField] private Text MoedasTxt;

    [SerializeField] private Text TxtPrecoEmpilhadeira;
    [SerializeField] private Text TxtPrecoVenda;
    [SerializeField] private Text TxtPrecoEspecial;

    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject Player;    
    
    [SerializeField] private GameObject BtnEmpilhadeira;
    [SerializeField] private GameObject BtnEspecial;

    [SerializeField] private int PrecoEmpilhadeira;
    [SerializeField] private int PrecoVenda;
    [SerializeField] private int PrecoEspecial;

    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        ContadorEmpilhadeira = 2;
    }

    // Update is called once per frame
    void Update()
    {
        MoedasTxt.text = Moedas.ToString();

        TxtPrecoEmpilhadeira.text = PrecoEmpilhadeira.ToString();
        TxtPrecoVenda.text = PrecoVenda.ToString();
        TxtPrecoEspecial.text = PrecoEspecial.ToString();

        if (ContadorEmpilhadeira == 8)
        {
            BtnEmpilhadeira.SetActive(false);
        }
    }

    public void Venda(int ValorPVenda)
    {
        Moedas += ValorPVenda;
    }

    public void Empilhadeira()
    {
        if(Moedas >= PrecoEmpilhadeira)
        {
            Player.GetComponent<PlayerScript>().Upgrade(1);
            Moedas -= PrecoEmpilhadeira;
            ContadorEmpilhadeira += 2;
        }
    }

    public void Valor()
    {
        if (Moedas >= PrecoVenda)
        {
            Player.GetComponent<PlayerScript>().Upgrade(2);
            Moedas -= PrecoVenda;
        }
    }

    public void Especial()
    {
        if (Moedas >= PrecoEspecial)
        {
            Player.GetComponent<PlayerScript>().Upgrade(3);
            Moedas -= PrecoEspecial;
            BtnEspecial.SetActive(false);
        }
    }

    public void CloseShop()
    {
        ShopUI.SetActive(false);
        MainUI.SetActive(true);
        Time.timeScale = 1;
    }
}
