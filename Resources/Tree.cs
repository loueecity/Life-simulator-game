using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // Start is called before the first frame update
    public static Tree instance;
    public GameObject logPrefab;
    public int health = 10;
    //public Animator anim;

    void Awake()
    {
      if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Instantiate(logPrefab, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
