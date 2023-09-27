using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Mouse point")]
    public GameObject lookAtTarget;

    [Header("Player Materials")]
    public Material normalMaterial;
    public Material damageMaterial;

    [Header("Player settings")]
    public PlayerController Player;
    private LevelController lc;
    private float distance;
    private int damageCounter = 0;
    private int damageLast = 5;
    private bool damaged = false;

    bool hitByLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 toObjectVector = transform.position - Camera.main.transform.position;
        Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
        distance = linearDistanceVector.magnitude;
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    private void Update()
    {
        MoveTarget();
    }

    private void FixedUpdate()
    {
        LookAtMouse();

        if (damaged)
        {
            if (damageCounter == damageLast)
            {
                this.GetComponent<MeshRenderer>().material = normalMaterial;
                damageCounter = 0;
                damaged = false;
            }
            else
            {
                damageCounter++;
            }
        }
    }

    void MoveTarget()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distance;
        lookAtTarget.transform.position += new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y"));

        Vector3 Temp = lookAtTarget.transform.position;
        Temp.x = Mathf.Clamp(Temp.x, -20, 20);
        Temp.z = Mathf.Clamp(Temp.z, -20, 20);
        lookAtTarget.transform.position = Temp;
    }

    void LookAtMouse()
    {
        transform.LookAt(new Vector3(lookAtTarget.transform.position.x, this.transform.position.y + 90, lookAtTarget.transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EBullet" || other.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            Player.LoseHealth(1);
            this.GetComponent<MeshRenderer>().material = damageMaterial;
            damaged = true;
            Player.hit.Play();
        }

        if(other.tag == "Enemy")
        {
            Player.LoseHealth(2);
            this.GetComponent<MeshRenderer>().material = damageMaterial;
            damaged = true;
            Player.hit.Play();
        }

        if(other.tag == "Laser" && !hitByLaser)
        {
            Player.LoseHealth(2);
            hitByLaser = true;
            damaged = true;
            Player.hit.Play();
            StartCoroutine(RefreshHit());
        }
    }

    IEnumerator RefreshHit()
    {
        yield return new WaitForSeconds(1f);

        hitByLaser = false;
    }
}
