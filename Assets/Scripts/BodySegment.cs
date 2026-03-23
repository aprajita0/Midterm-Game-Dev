using UnityEngine;
using UnityEngine.SceneManagement;

public class BodySegment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}