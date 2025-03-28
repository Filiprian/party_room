using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject wall;
    public Animator rightDoor;
    public Animator leftDoor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wall.SetActive(true);
            Invoke("CloseDoors", 0.5f);
            Invoke("LoadNextLevel", 3.5f);
        }
    }

    void CloseDoors()
    {
        rightDoor.SetTrigger("Close");
        leftDoor.SetTrigger("Close");
    }

    void LoadNextLevel()
    {
        Debug.Log("Next level");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
