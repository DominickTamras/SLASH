using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;


public class Movement : MonoBehaviour
{
    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    private int directionDash;
    public bool isDashing;
    bool isAttacking;
    bool onCD;
    public float dashCD;
    private float currentCD;
    
    public float movementSpeed;
    
    
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 direction;
    Vector2 lastmousePos;
    private Vector2 screenPos;

    public Transform arrow;
    public Transform target;

    [Header("VFX")]
    public GameObject explosion;
    public GameObject dustEffect;
    public AudioSource death;
    public AudioSource dash;

    private GameObject cam;
    private bool movedUp;
    private bool movedDown;
    private Vector3 clampedY;
    public float clampMin;
    Transform newz;

    

   public GameObject deathScreen;




    private void Start()
    {
       

        newz = gameObject.transform;
   
        cam = Camera.main.gameObject;
        dashTime = startDashTime;
    }
    private void Update()
    {
       /* clampedY = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float edgeSize = 30; // Compares edge of screen to player
        if (screenPos.x > Screen.width - edgeSize)
        {
            cam.transform.DOMove(new Vector3(cam.transform.position.x + 15, cam.transform.position.y, cam.transform.position.z), 0.2f);
        }
        if (screenPos.x < edgeSize)
        {
            cam.transform.DOMove(new Vector3(cam.transform.position.x - 15, cam.transform.position.y, cam.transform.position.z), 0.2f);
        }

        if (screenPos.y > Screen.height - 10)
        {
            cam.transform.DOMove(new Vector3(cam.transform.position.x, cam.transform.position.y + 10,cam.transform.position.z), 0.2f);
            movedUp = true;

           
        }
        if (screenPos.y < edgeSize)
        {
          

            cam.transform.DOMove(new Vector3(cam.transform.position.x, cam.transform.position.y - 10, cam.transform.position.z), 0.2f); // mess with these values



        }*/

       /* clampedY.y = Mathf.Clamp(clampedY.y, clampMin, 20);

        cam.transform.position = clampedY;*/




        FaceMouse();
        movement.x = Input.GetAxisRaw("Horizontal") * movementSpeed;


        if (Input.GetKeyDown(KeyCode.Mouse0) && isDashing == false)
        {
            StartCoroutine(Dash());
            rb.gravityScale = 0;
            currentCD = 0;
            Camera.main.DOShakePosition(0.2f, 0.4f, fadeOut: true);
            GameObject dustEffectClone = Instantiate(dustEffect, new Vector2(transform.position.x, transform.position.y - 1.5f), Quaternion.identity);
            dash.Play();
            Destroy(dustEffectClone, 3f);



        }

        if (currentCD < dashCD && isDashing)
        {
            currentCD += Time.deltaTime;
   
        }

        

        else
        {
            isDashing = false;
           
        }

     



    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
 

   
     
    }

 

    void FaceMouse() // Mess Vector3.posDirection to have obj face unique directions (Up, left, right, down, etc.)
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition ) - arrow.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.rotation = Quaternion.Slerp(arrow.rotation, rotation, 5 * Time.deltaTime);

    }
    IEnumerator Dash()
    {
        Vector3 dashDir = target.position - transform.position;

        dashDir.Normalize();

        float time = 0;

        while(time < dashTime)
        {
            rb.AddForce(dashDir * dashSpeed);
            time += Time.deltaTime;
            isDashing = true;
            isAttacking = true;
            yield return null;
        }
        isAttacking = false;
        rb.gravityScale = 50;
        //isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        rb.velocity = Vector3.ProjectOnPlane(rb.velocity, other.contacts[0].normal);
        if (isAttacking == true)
        {
            if(other.gameObject.name == "Enemy")
            {
                other.gameObject.SetActive(false);
                death.Play();
                Camera.main.DOShakePosition(0.5f, 1, fadeOut: true);
            }
        }
    }

    private void OnDisable()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        
        Camera.main.DOShakePosition(0.5f, 1, fadeOut: true);

        deathScreen.SetActive(true);


    }



}
