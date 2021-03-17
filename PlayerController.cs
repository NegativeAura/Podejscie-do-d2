    using UnityEngine;

public class PlayerController : MonoBehaviour
{


    //zmienne startowe
    private Rigidbody2D Rb;
    private Animator anim;
    private Collider2D coll;
    public int cherries=0;

    
    //FSM 
    private enum State {idle,running,jumping,falling} //statusy postaci
    private State state = State.idle; 
    
    
    

    
    //zmienne inspektora
    [SerializeField] private LayerMask ground; //Powierzchnia planszy
    [SerializeField] private float speed=5f;   //predkosc
    [SerializeField] private float jump = 30f; //skok


    private void Start() 
    {

        Rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D> ();
       
    }
        
    private void Update()
    {
        Movement();
        AnimationState();                     
        anim.SetInteger("state", (int)state); //zmiana statusu postaci na podstawie numeru statusu


    }

  

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
        }
    }



    
    
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");


        if (hDirection < 0)   //poruszanie w lewo
        {
            Rb.velocity = new Vector2(-speed, Rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }



        else if (hDirection > 0) //poruszanie w prawo

        {
            Rb.velocity = new Vector2(speed, Rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }





        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))   //skakanie
        {

            Rb.velocity = new Vector2(Rb.velocity.x, jump);
            state = State.jumping;
        }
    } //poruszanie sie 



    private void AnimationState()       //Status animacji postaci
    {
      if(state == State.jumping)
        {
            if(Rb.velocity.y < 1f) 
            {
                state = State.falling;  
            }
        }

        else if (state == State.falling)   //spadanie
        {
            if (coll.IsTouchingLayers(ground))  //sprawdzenie czy postac dodtyka ziemi
            
            {
                state = State.idle;
            }
        }
        
        
        else if(Mathf.Abs(Rb.velocity.x) > 2f)  
        {
            //poruszanie

            state = State.running;

        }
       
      else
        {
            state = State.idle;
        }



    }



}

