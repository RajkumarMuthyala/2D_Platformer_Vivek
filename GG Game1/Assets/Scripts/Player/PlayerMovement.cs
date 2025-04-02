using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce; // Separate variable for jump force
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxcollider;
    private float wallJumpCoolDown;
    private float horizontalInput;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        // Grab references for rigidbody and animator from game object.
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("grounded", isgrounded());
        anim.SetBool("Jump", !isgrounded());

        //wallJump logic
        if (wallJumpCoolDown > 0.2f)
        {
           

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isgrounded())
            {

                body.gravityScale = 0;
                body.velocity = Vector2.zero;

            }
            else { body.gravityScale = 5; 
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
                if(Input.GetKeyDown(KeyCode.Space) && isgrounded() )
                    SoundManager.instance.PlaySound(jumpSound);


            }
        }
        else
        {
            wallJumpCoolDown += Time.deltaTime;
        }

    }


    private void Jump()
    {
       
        if (isgrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce); // Use jumpForce
            anim.SetBool("Jump", true);
        }else if(onWall() && !isgrounded())
        {

            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCoolDown = 0;
            
        }
    }

   
    private bool isgrounded()
    {
        RaycastHit2D raycastHit  = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, new Vector2(transform.localScale.x,0) ,0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isgrounded() && !onWall();
    }

}
