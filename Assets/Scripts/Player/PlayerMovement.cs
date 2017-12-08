using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed  = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    //float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Transform closeEnmy = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Enemy"));
        Vector3 look;
        if (closeEnmy != null)
        {
            print("Gg");
            look = closeEnmy.transform.position;
        }
        else
        {
            print("ggmeu");
            look = transform.forward;
        }

        Move(h, v);
        Turning(look);
        Animating(h, v);
    }

    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject g in enemies)
        {
            float dist = Vector3.Distance(g.transform.position, currentPos);
            if (dist < minDist && g.GetComponent<EnemyHealth>().isDead == false)
            {
                tMin = g.transform;
                minDist = dist;
            }
        }
        return tMin;
    }


    void Move (float h, float v)
    {
        movement.Set(h, 0, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning (Vector3 look)
    {
        /*
        Ray camRay = Camera.main.ScreenPointToRay(look);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }*/
        Vector3 playerToLook = look - transform.position;
        playerToLook.y = 0f;
        Quaternion newRotation = Quaternion.LookRotation(playerToLook);
        playerRigidbody.MoveRotation(newRotation);
    }

    void Animating (float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}