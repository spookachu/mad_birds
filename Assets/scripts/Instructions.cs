using UnityEngine;

// Disable the instructions text if not wanted
public class Instructions : MonoBehaviour
{
    public GameObject instructions;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && instructions != null)
        {
            instructions.SetActive(false); 
        }
    }
}
