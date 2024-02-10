using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeIA : MonoBehaviour
{
    private GameManager _GameManager;

    private Animator anim;
    public ParticleSystem hitEffect;
    public int HP;

    private bool isDie;

    public enemyState state;


    //IA do slime
    private bool isWalk;
    private bool isAlert;
    private NavMeshAgent agent;
    private int idWayPoint;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();

        if(agent.desiredVelocity.magnitude >= 0.1f)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        anim.SetBool("isWalk", isWalk);
        
    }



    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject); 
    }

    #region MEUS METODOS

    void GetHit(int amount)
    {
        if(isDie == true) { return; }

        HP -= amount;

        if(HP > 0)
        {
            ChangeState(enemyState.FURY);
            anim.SetTrigger("GetHit");
        }
        else // <=
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }


        anim.SetTrigger("GetHit");
        hitEffect.Emit(25);
    }


    void StateManager()
    {
        switch (state)
        {
            
            case enemyState.FOLLOW:
                //COMPORTAMENTO QUANDO ESTIVER SEGUINDO
                break;

            case enemyState.FURY:
                //COMPORTAMENTO QUANDO ESTIVER EM FURIA

                destination = _GameManager.player.position;
                agent.destination = destination;         

                break;

            case enemyState.PATROL:
                //COMPORTAMENTO QUANDO ESTIVER PATRULHANDO
                break;
        }
    }

    void ChangeState(enemyState newState)
    {
        StopAllCoroutines(); //ENCERRA TODAS AS CORROUTINAS
        state = newState;
        print(newState);

        switch (state)
        {
            case enemyState.IDLE:

                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");

                break;

            case enemyState.ALERT:
                
                break;

            case enemyState.PATROL:

                agent.stoppingDistance = 0;
                idWayPoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                destination = _GameManager.slimeWayPoints[idWayPoint].position;
                agent.destination = destination;                

                
                StartCoroutine("PATROL");

                break;

            case enemyState.FURY:

                destination = transform.position;
                agent.stoppingDistance = _GameManager.slimeDistanceToAttack;
                agent.destination = destination;

                break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);

        StayStill(50); //50% de chance de ficar parado ou entrar em patrulha
    }

    IEnumerator PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);

        StayStill(30); // 30% de chance de ficar parado e 70% de ficar em patrulha

    }

    void StayStill(int yes)
    {
        if(Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else // CASO NO
        {
            ChangeState(enemyState.PATROL);
        }
    }

    int Rand()
    {
        int rand = Random.Range(0, 100); // 0, 1, 2, 3... 99.
        return rand;
    }



    #endregion
}
