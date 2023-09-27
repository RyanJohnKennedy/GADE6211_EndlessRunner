using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private LevelController lc;

    public int maxHealth;
    public int health;

    public int bulletSpeed;

    public Material damageMaterial;

    public bool bossBattle = false;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        
        health = maxHealth;
    }

    private void Update()
    {
        CheckHealth();
    }

    private void FixedUpdate()
    {
        MoveWithPlatforms(); 
    }

    void MoveWithPlatforms()
    {
        if (this.transform.position.z > 14)
        {
            transform.position += new Vector3(0f, 0f, -lc.gameSpeed);
        }
        else
        {
            bossBattle = true;
        }
    }

    public void CheckHealth()
    {
        if(health <= 0)
        {
            GameManager.Instance.score += 10;
            lc.EndLevel();
            lc.DestroyGameObject(this.gameObject);
        }
    }

    public void FlashRed(GameObject _object)
    {
        foreach (Transform child in _object.transform)
        {
            try
            {
                Material _materialTemp = child.gameObject.GetComponent<MeshRenderer>().material;
                child.gameObject.GetComponent<MeshRenderer>().material = damageMaterial;
                StartCoroutine(ReturnToColour(child.gameObject, _materialTemp));
            }
            catch { }
        }
    }

    IEnumerator ReturnToColour(GameObject _object, Material _material)
    {
        yield return new WaitForSeconds(0.1f);

        _object.GetComponent<MeshRenderer>().material = _material;
    }
}
