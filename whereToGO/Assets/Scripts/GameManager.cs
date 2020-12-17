using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public enum Stances
    {
        Fire,
        Frost,
        Normal
    }

    public static GameManager instance;
    public Stances currentStance = Stances.Normal;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            player = GameObject.FindGameObjectWithTag("alive");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentStance== Stances.Fire)
        {

        }
    }
}
