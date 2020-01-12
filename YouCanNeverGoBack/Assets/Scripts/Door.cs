using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int keyId;
    public string nextSceneName;

    public AK.Wwise.Event softMusic;
    public AK.Wwise.Event openDoor;
    public GameObject wwiseObj;

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
        openDoor.Post(wwiseObj);
        softMusic.Post(wwiseObj);
        GetComponent<Animator>().SetTrigger("OpenDoor");
        GetComponentInChildren<Animator>().SetTrigger("ChangeScene");
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}
