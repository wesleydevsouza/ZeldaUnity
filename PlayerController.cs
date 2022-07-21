using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Controle
    private CharacterController controller;
    //Animação
    private Animator anim;

    //-----------Config Player-----------

    //Valor da velocidade inicial
    public float movementSpeed = 3f;

    //Variável de movimento
    private Vector3 direction;
    //Variavel de controle da animação d andar
    private bool isWalk;

    //-----------Input-----------

    //Variaveis para armazenar movimento horizontal e verical
    public float horizontal;
    public float vertical;

    //-----------Config Ataque-----------

    public ParticleSystem fxAttack;
    //Area de ataque
    public Transform hitBox;
    [Range(0.2f, 1f)]
    public float hitRange = 0.5f;
    //Máscara de layer
    public LayerMask hitMask;
    //Variavel para verificar se está atacando 
    private bool isAttack;
    //Colisor do ataque
    public Collider[] hitInfo;
    //Quantidade de hits
    public int amountDmg;


    


    void Start()
    {
        //armazenando o componente do charactercontroller dentro da variável
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //chamando o método de controle
        Inputs();

        //chamando o método de movimento
        MoveCharacter();

        //chamando o att animator
        UpdateAnimator();

    }

    #region Metodos

    void Inputs() {
        //Variaveis para armazenar movimento horizontal e verical
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //Verificando os inputs
        if (Input.GetButtonDown("Fire1") && isAttack == false) {
            //Método attack
            Attack();            

        }
        
    }

    void Attack() {
        isAttack = true;
        //Botão de ataque
        anim.SetTrigger("Attack");
        //Fazendo as particulas aparecerem ao atacar
        fxAttack.Emit(1);

        //Colisor do ataque
        hitInfo = Physics.OverlapSphere(hitBox.position, hitRange);

        //Enviando msg para a função gethit para registrar o dano
        foreach (Collider c in hitInfo) {
            c.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
        }



    }

    void MoveCharacter() {
        //Direções XYZ, normalizando a soma das direções
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        //verificando se o personagem está em movimento para rotacionar
        if (direction.magnitude > 0.1f) {
            //passando direção para frente e para trás e convertendo d radiano para grau
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //rotacionando de acordo com a direção
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            //Anim andando
            isWalk = true;

        } else {
            //Parar animação andando
            isWalk = false;

        }

        //Criando o movimento de acordo com a direção e a velocidade de movimento, compensando a taxa de frames maiores em dispositivos fortes
        controller.Move(direction * movementSpeed * Time.deltaTime);

    }

    void UpdateAnimator() {
        //Att o animator
        anim.SetBool("isWalk", isWalk);

    }

    void AttackIsDone() {
        isAttack = false;
    }

    #endregion

    private void OnDrawGizmosSelected() {
        //Verificando se o hitbox existe para desenhar o gizmo
        if (hitBox != null) {
            //Desenhando a espera da hitbox
            Gizmos.DrawWireSphere(hitBox.position, hitRange);

        }        

    }

}
