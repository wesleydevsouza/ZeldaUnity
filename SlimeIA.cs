using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeIA : MonoBehaviour
{
    //System
    //Permitindo acesso
    private Animator anim;
    public ParticleSystem hitEffect;
    private gameManager _GameManager;    
    
    //Atributos
    public int HP;


    //estados
    //alterador de estados
    public enemyState state;
    private bool isWalk;
    private bool isAlert;
    private bool isDead;    

    //IA
    //Variavel de navegação na mesh do mapa
    private NavMeshAgent agent; 
    //Vari�vel de posição
    private Vector3 destination;
    //Variavel dos id dos pontos do mapa
    private int idWaypoint;

    // Start is called before the first frame update
    void Start()
    {

        //Encontrando o objeto gameManage
        _GameManager = FindObjectOfType(typeof(gameManager)) as gameManager;

        //Acesso as animações do Animator
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        ChangeState(state);

    }

    // Update is called once per frame
    void Update()
    {
        StateManager(); 

        if (agent.desiredVelocity.magnitude >= 0.1f) {
            isWalk = true;
        } else {
            isWalk = false;
        }

        anim.SetBool("isWalk", isWalk);
      
    }
     
    IEnumerator Died() {
        isDead = true;
        //aguardar por 1 segundo para desaparecer o modelo
        yield return new WaitForSeconds(1.5f);
        //Destruindo o objeto
        Destroy(this.gameObject);
    } 

    

    #region Meus Métodos

    void GetHit(int amount) {

        //se essa condição for verdade, o return encerrará a execução, fazendo com que nada abaixo seja executado.
        if (isDead == true) {
            return;
        }

        HP -= amount;

        if (HP > 0) {
            //Alterando o estado para fúria ao ser atacado
            ChangeState(enemyState.FURY);
            //Pegando o trigger da animação de dano
            anim.SetTrigger("GetHit");
            hitEffect.Emit(15);
        } else {
            anim.SetTrigger("Die");
            StartCoroutine("Died");

        }       

    }

    void StateManager() {
        switch (state) {

            case enemyState.FOLLOW:

                break;

            case enemyState.FURY:
                destination = _GameManager.player.position;
                agent.destination = destination;


                break;

            case enemyState.PATROL:

                break;


        }
    }

    void ChangeState(enemyState newState) {

        //parando as corrotinas
        StopAllCoroutines();
        state = newState;
        //print(newState);

        switch (state) {
            case enemyState.IDLE:

                agent.stoppingDistance = 0;

                //destinho
                //Recebendo a própria posição que est para fazer o personagem ficar parado no estado IDLE
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");

                break;

            case enemyState.ALERT:

                break;

            case enemyState.PATROL:

                agent.stoppingDistance = 0;

                //Sorteando um id dos waypoints
                idWaypoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                //Gerando um destino com o id
                destination = _GameManager.slimeWayPoints[idWaypoint].position;
                //Fazendo o npc ir até o destido gerado
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
    
    IEnumerator IDLE() {
        //espera o tempo para trocar d estado
        yield return new WaitForSeconds(_GameManager.slimeIdleWaitTime);
        StayStill(30); //70% de chance d ficar parado ou entrar em patrulha

    }

    IEnumerator PATROL() {

        //espera até chegar ao destino para alterar a rota
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        //sorteia para ver se ele entrará em idle ou patrol
        StayStill(70); //30% de chance de ficar parado e 70 de continuar em patrulha


    }
    void StayStill(int yes) {

        if (Rand() <= yes) {
            ChangeState(enemyState.IDLE);
        } else {
            ChangeState(enemyState.PATROL);
        }

    }

    int Rand() {
        int rand = Random.Range(0, 100);
        return rand;
    }

    #endregion
}
