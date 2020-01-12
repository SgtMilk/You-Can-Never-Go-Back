using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int keyId;
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Moe moe = collision.gameObject.GetComponent<Moe>();
        if (moe && (keyId == -1 || moe.hasKey(keyId))) {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        GetComponent<Animator>().SetTrigger("OpenDoor");
        GetComponentInChildren<Animator>().SetTrigger("ChangeScene");
        yield return new WaitForSeconds(0.2f);
        //transform.Translate(new Vector2(-2f, 0));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}
