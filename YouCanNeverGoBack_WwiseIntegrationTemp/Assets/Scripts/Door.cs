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
        if (moe && moe.hasKey(keyId)) {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
