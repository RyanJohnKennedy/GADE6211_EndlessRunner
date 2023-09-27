using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    FireRate,
    Shield,
    Health
}

public abstract class PickUpController : MonoBehaviour
{
    private PickUpType type;

    public PickUpType Type
    {
        get { return type; }
        set { type = value; }
    }

    private LevelController lc;

    public LevelController LC
    {
        get { return lc; }
        set { lc = value; }
    }

    public abstract void Start();

    public void Awake()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    public void Update()
    {
    }

    private void FixedUpdate()
    {
        RotatePickUp();
        MoveWithPlatforms();
    }

    void MoveWithPlatforms()
    {
        transform.position += new Vector3(0f, 0f, -lc.gameSpeed);
    }

    public void RotatePickUp()
    {
        transform.Rotate(0f, 2f, 0f);
    }

    public abstract void Effect(GameObject Player);

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Effect(other.gameObject);
            LC.DestroyGameObject(this.gameObject);
            GameManager.Instance.pickUpsCollected++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            this.transform.position = new Vector3(Random.Range(-10, 11), 0.5f, this.transform.position.z);
        }
    }
}
