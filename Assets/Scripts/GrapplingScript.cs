using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrapplingScript : MonoBehaviour
{
    public MovementScript movementScript;
    public Animator animator;
    public Rigidbody2D rb2D;
    public GameObject gizmoLine;
    public GameObject _gizmoLine;
    public Transform drawPosition;
    public LayerMask playerLayerMask;
    public bool isHolding;
    public bool canGrapple;
    public bool isGrappling;
    public RaycastHit2D hit;
    public float offset;
    public float maxGrapplingSpeed;
    public Camera cam;

    public Vector2 grapplingDir;
    public Vector2 _grapplingDir;
    public float grapplingForce;
    public bool isGrapplingJump;
    public bool hitPlatform;

    public Button grapplingButton;
    public EventTrigger trigger;

    public float coolDownGrappling = .5f;
    public float timerGrappling;
    public bool startCooldown;
    public GameObject grapplingHead;
    public GameObject _grapplingHead;

    private void Awake()
    {
        movementScript = transform.GetComponent<MovementScript>();
        animator = transform.GetComponent<Animator>();
        rb2D = transform.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(startCooldown)
        {
            timerGrappling-= Time.fixedDeltaTime;
            if(timerGrappling < 0 )
            {
                grapplingButton.interactable = true;
                trigger.enabled = true;
                timerGrappling = coolDownGrappling;
                startCooldown = false;
            }
        }

        if (movementScript.isWallSliding)
        {
            canGrapple = false;
        }

        if (isGrappling)    //GrapplingCode
        {
            grapplingButton.interactable = false;
            trigger.enabled = false;
            rb2D.velocity = new Vector2(0f, Mathf.Clamp(rb2D.velocity.y, 0f, maxGrapplingSpeed));
            movementScript.enabled = false;
            animator.enabled = false;
            rb2D.AddForce(grapplingForce * Time.fixedDeltaTime * _grapplingDir); //Dont get component in update //5000f for grappling force
            float distance = (hit.point.y + offset)- transform.position.y;
            if(distance <= 0.001f || hitPlatform)
            {
                movementScript.enabled = true;
                animator.enabled = true;
                isGrappling = false;
                isGrapplingJump = false;
                hitPlatform = false;
                startCooldown = true;
            }
            cam.transform.position = Vector3.Lerp(new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z),
            new Vector3(cam.transform.position.x, transform.position.y + 5f, transform.position.z - 18f), 2.2f * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                movementScript.enabled = true;
                animator.enabled = true;
                isGrappling = false;
                movementScript.GrapplingJump();
            }
        }
        else
        {
            grapplingButton.interactable = true;
            trigger.enabled = true;
            Destroy(_grapplingHead);
        }

        //else
        //{
        //    player.GetComponent<MovementScript>().enabled = true; //WAS I FUCKING CRAZY???????? now the movementscript cannot be disabled lmao
        //}

        //if (isGrapplingJump)
        //{
        //    isGrappling = false;
        //    player.GetComponent<MovementScript>().GrapplingJump(); //Dont get component in update
        //    //Find a way to stop GrapplingJump after hit a wall or landed or afterGraplingJump
        //}
    }

    private void Update()
    {
        Debug.DrawRay(drawPosition.position, Vector2.up);
        //Debug.DrawRay(drawPosition.position, grapplingDir,Color.red); //Draw Direction of grappling
        if (isHolding)
        {
            hit = Physics2D.Raycast(drawPosition.position, Vector2.up, Mathf.Infinity, ~playerLayerMask);
            if (/*Physics2D.Raycast(drawPosition.position, new Vector3(player.transform.localScale.x, 0f), Mathf.Infinity)*/ hit )
            {
                if (hit.transform == null)
                {
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
                else if (movementScript.isWallSliding)
                {
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
                else if (hit.transform.CompareTag("LevelSeperation"))
                {
                    grapplingDir = new Vector2(0f, hit.point.y - transform.position.y + offset).normalized;
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.green;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.green;
                    canGrapple = true;
                }
                else
                {
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
            }
        }
    }

    public void DrawGizmos()
    {
        _gizmoLine = Instantiate(gizmoLine, drawPosition.position, Quaternion.identity);
        Vector3 localscale = transform.localScale;
        _gizmoLine.transform.localScale = localscale;
        _gizmoLine.transform.SetParent(transform);
    }
    public void IsHoldingButton()
    {
        isHolding = true;
    }

    public void ReleaseButton()
    {
        isHolding = false;
        if (!canGrapple)
        {
            Destroy(_gizmoLine);
        }
        else
        {
            _grapplingHead = Instantiate(grapplingHead, hit.point, Quaternion.identity);    
            isGrappling = true;
            _grapplingDir = grapplingDir;
            Destroy(_gizmoLine);
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (isGrappling)
    //    {
    //        if (collision.transform.CompareTag("LevelSeperation"))
    //        {
    //            isGrappling = false;
    //            isGrapplingJump = false;
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") && isGrappling)
        {
            hitPlatform = true;
        }
    }
}
