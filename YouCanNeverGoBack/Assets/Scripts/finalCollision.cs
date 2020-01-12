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
        if (collision.CompareTag("Finish"))
        {
            raftThere = true;
        }

        Moe moe = collision.gameObject.GetComponent<Moe>();

        if (moe && raftThere)
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        GetComponentInChildren<Animator>().SetTrigger("ChangeScene");
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}

