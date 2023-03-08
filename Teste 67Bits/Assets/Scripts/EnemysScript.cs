using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemysScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool morto;
    [SerializeField] private bool Pego;
    public bool ImpossivelDePegar;
    [SerializeField] private bool Sumir;
    [SerializeField] private GameObject Hips;
    [SerializeField] private GameObject[] Ossos;
    [SerializeField] private NavMeshAgent nav;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List <GameObject> TargetIA;

    [SerializeField] private float TimerPosicao;
    [SerializeField] private float TimerPosicaoC;
    [SerializeField] private GameObject Spawner;

    [SerializeField] private float TimerSumir;
    void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < Ossos.Length; i++)
        {
            Ossos[i].GetComponent<Rigidbody>().useGravity = false;
        }

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("TargetEnemy").Length; i++)
        {
            TargetIA.Add(GameObject.FindGameObjectsWithTag("TargetEnemy")[i]);
        }
        Spawner = GameObject.FindGameObjectsWithTag("Spawner")[0];
        nav = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimerPosicaoC -= Time.deltaTime;
        if(TimerPosicaoC <= 0)
        {
            TimerPosicaoC = TimerPosicao;
            if (!morto)
            {

                nav.SetDestination(TargetIA[Random.Range(0, 8)].transform.position);
            }
        }

        if(morto)
        {
            nav.SetDestination(gameObject.transform.position);
            nav.speed = 0;
            nav.enabled = false;
        }
        animator.SetFloat("Velocity", nav.desiredVelocity.magnitude);

        if (Pego)
        {
            Hips.transform.position = gameObject.transform.position;
        }

        if (Sumir)
        {
            TimerSumir -= Time.deltaTime;
        }

        if(TimerSumir <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage()
    {
        animator.enabled = false;
        morto = true;
        for (int i = 0; i < Ossos.Length; i++)
        {
            Ossos[i].GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void Vendido()
    {
        gameObject.transform.parent = null;
        Hips.GetComponent<Rigidbody>().isKinematic = false;
        Pego = false;
        Sumir = true;
        Spawner.GetComponent<SpawnerEnemyScript>().SubtractEnemy();
        for (int i = 0; i < Ossos.Length; i++)
        {
            try
            {
                Ossos[i].GetComponent<CapsuleCollider>().enabled = true;
            }
            catch (System.Exception)
            {
                try
                {
                    Ossos[i].GetComponent<BoxCollider>().enabled = true;
                }
                catch (System.Exception)
                {

                    Ossos[i].GetComponent<SphereCollider>().enabled = true;
                }


            }

        }
    }

    public void FoiPego()
    {
        Hips.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Hips.transform.eulerAngles = new Vector3(90, 0, 0);
        Pego = true;
        ImpossivelDePegar = true;
        for (int i = 0; i < Ossos.Length; i++)
        {
            try
            {
                Ossos[i].GetComponent<CapsuleCollider>().enabled = false;
            }
            catch (System.Exception)
            {
                try
                {
                    Ossos[i].GetComponent<BoxCollider>().enabled = false;
                }
                catch (System.Exception)
                {

                    Ossos[i].GetComponent<SphereCollider>().enabled = false;
                }
                

            }

        }
    }
}
