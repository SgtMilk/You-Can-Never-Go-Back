using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finalCollision : MonoBehaviour
{

    public string nextSceneName;

    private bool raftThere = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        Moe moe = collision.gameObject.GetComponent<Moe>();

        if (moe)
        {
            SceneManager.LoadScene(nextSceneName);

        }
    }
}

