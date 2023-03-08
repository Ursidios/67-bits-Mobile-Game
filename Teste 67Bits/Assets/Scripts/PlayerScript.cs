using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header ("Movimentação")]
    [SerializeField] private float Velocidade;
    [SerializeField] private VariableJoystick Joystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 AnguloRotacao;
    [SerializeField] private float TurnSpeed;

    [Header("Ataque")]
    [SerializeField] private Transform Areadano;
    [SerializeField] private LayerMask inimigoLayer;
    [SerializeField] private float range;

    [Header("Upgrades")]
    [SerializeField] private bool especial;
    [SerializeField] private bool especialAtivo;
    [SerializeField] private Material material;


    [Header("Carregar Corpo")]
    [SerializeField] private Transform[] PivotEnemy;
    [SerializeField] private List<GameObject> Enemys;
    [SerializeField] private Transform AreaPegada;
    [SerializeField] private float rangePegada;
    [SerializeField] private bool Pegou;
    [SerializeField] private int ContadorPegada;
    [SerializeField] private int MaxContadorPegada;

    [Header("UI")]
    [SerializeField] private Text QuantidadeCorpos;
    [SerializeField] private Text QTNEmpilhadeira;
    [SerializeField] private Text QTNValorVenda;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject ShopUIComponents;
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject EspecialButton;
    [SerializeField] private int ValorPVenda;


    [Header("Especial")]
    [SerializeField] private Transform AreaEspecial;
    [SerializeField] private float rangeEspecial;
    [SerializeField] private float TimerEspecial;
    [SerializeField] private float TimerEspecialC;


    public void Start()
    {
        TimerEspecialC = TimerEspecial;
    }
    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * Joystick.Vertical + Vector3.right * Joystick.Horizontal;
        
        animator.SetFloat("Velocity", rb.velocity.magnitude / 3f);
        

        rb.AddForce(direction * Velocidade * Time.fixedDeltaTime, ForceMode.VelocityChange);

        if(direction != Vector3.zero)
        {
            transform.forward = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    public void Update()
    {
        QuantidadeCorpos.text = ContadorPegada.ToString();
        QTNEmpilhadeira.text = MaxContadorPegada.ToString();
        QTNValorVenda.text = ValorPVenda.ToString();

        if (especial)
        {
            EspecialButton.SetActive(true);
        }
        else EspecialButton.SetActive(false);

        if (TimerEspecialC <= 0)
        {
            especialAtivo = false;
            TimerEspecialC = TimerEspecial;
        }
        if (especialAtivo)
        {
            EspecialButton.SetActive(false);
            TimerEspecialC -= Time.deltaTime;
            AreaEspecial.gameObject.GetComponent<MeshRenderer>().enabled = true;
            Collider[] Acertou = Physics.OverlapSphere(AreaEspecial.position, rangeEspecial, inimigoLayer);
            foreach (Collider inimigo in Acertou)
            {
                Debug.Log("Acertou " + inimigo.name);
                inimigo.GetComponent<EnemysScript>().TakeDamage();
              
            }
        }
        else
        {
            AreaEspecial.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        if (!especialAtivo && especial)
        {
            EspecialButton.SetActive(true);

        }
    }

    public void Punch()
    {
        animator.SetTrigger("Punch");

    }
    public void Catcher()
    {
        Pegou = true;
        Collider[] Acertou = Physics.OverlapSphere(AreaPegada.position, rangePegada, inimigoLayer);
        foreach (Collider inimigo in Acertou)
        {
            if(ContadorPegada < MaxContadorPegada && inimigo.GetComponent<EnemysScript>().ImpossivelDePegar == false)
            {
                if(inimigo.GetComponent<EnemysScript>().morto == true)
                {
                    inimigo.transform.parent = PivotEnemy[ContadorPegada];
                    inimigo.GetComponent<EnemysScript>().FoiPego();
                    inimigo.GetComponent<EnemysScript>().morto = false;
                    inimigo.transform.position = PivotEnemy[ContadorPegada].position;
                    Debug.Log("Pegou " + inimigo.name);
                    ContadorPegada++;
                    Enemys.Add(inimigo.gameObject);
                    animator.SetBool("Hold", true);
                }
            }
        }  
    }

    public void Ataque()
    {
        Collider[] Acertou = Physics.OverlapSphere(Areadano.position, range, inimigoLayer);
        foreach (Collider inimigo in Acertou)
        {
            Debug.Log("Acertou " + inimigo.name);
            inimigo.GetComponent<EnemysScript>().TakeDamage();
        }
    }

    public void Especial()
    {
        especialAtivo = true;
        material.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }

    public void Upgrade(int SelecaoUpgrade)
    {
        material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        if (SelecaoUpgrade == 1)
        {
            MaxContadorPegada += 2;
        }
        if(SelecaoUpgrade == 2)
        {
            ValorPVenda *= 2;
        }
        if(SelecaoUpgrade == 3)
        {
            especial = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Entrega"))
        {
            for (int i = 0; i < Enemys.Count; i++)
            {
                Enemys[i].GetComponent<EnemysScript>().Vendido();
                ShopUI.GetComponent<ShopScript>().Venda(ValorPVenda);
            }
            ContadorPegada = 0;
            Enemys.Clear();
            animator.SetBool("Hold", false);
        }

        if (other.CompareTag("Loja"))
        {
            ShopUIComponents.SetActive(true);
            MainUI.SetActive(false);
            Time.timeScale = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Areadano == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Areadano.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AreaPegada.position, rangePegada);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(AreaEspecial.position, rangeEspecial);
    }
}
