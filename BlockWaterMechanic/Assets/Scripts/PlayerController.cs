using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public const float fMoveSpeed = 800f;
    public const float fJumpForce = 900f;

    public LayerMask cLayerMask;
    
    private Rigidbody2D cRigidbody;


    // Use this for initialization
    void Start()
    {
        cRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float fHor = Input.GetAxis("Horizontal");
        Vector2 vSize = gameObject.GetComponent<CapsuleCollider2D>().size;
        vSize.Scale(new Vector2(1f, 0.9f));


        if (Input.GetButtonDown("Jump"))
        {
            //If we input jump and the player is on a good block
            if (Physics2D.BoxCast(transform.position, gameObject.GetComponent<CapsuleCollider2D>().size, 0, Vector2.down, 0.05f, cLayerMask))
                cRigidbody.AddForce(new Vector2(0f, fJumpForce));
        }

        
        if (fHor > 0f)
        {
            if (!Physics2D.BoxCast(transform.position, vSize, 0, Vector2.right, 0.05f, cLayerMask))
                cRigidbody.velocity = new Vector2(fHor * fMoveSpeed * Time.deltaTime, cRigidbody.velocity.y);
        }
        else if (fHor < 0f)
            if (!Physics2D.BoxCast(transform.position, vSize, 0, Vector2.left, 0.05f, cLayerMask))
                cRigidbody.velocity = new Vector2(fHor * fMoveSpeed * Time.deltaTime, cRigidbody.velocity.y);
    }
}