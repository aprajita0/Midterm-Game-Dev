using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    //These are the player's Variables, the raw info that defines them
    
    //The Rigidbody2D is a component that gives the player physics, and is what we use to move
    public Rigidbody2D RB;

    //TextMeshPro is a component that draws text on the screen.
    //We use this one to show our score.
    public TextMeshPro ScoreText;
    
    //This will control how fast the player moves
    public float Speed = 3;
    public List<Vector3> positions = new List<Vector3>();
    public float followDistance = 0.3f;
    
    //This is how many points we currently have
    public int Score = 0;
    
    // Snake body
    public GameObject BodySegmentPrefab;
    public List<Transform> bodyParts = new List<Transform>();


    
    //Start automatically gets triggered once when the objects turns on/the game starts
    void Start()
    {
        //During setup we call UpdateScore to make sure our score text looks correct
        positions.Clear();
        positions.Add(transform.position);
        UpdateScore();
    }

    //Update is a lot like Start, but it automatically gets triggered once per frame
    //Most of an object's code will be called from Update--it controls things that happen in real time
    void Update()
    {
        
        
        Vector2 vel = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.RightArrow)) vel.x = Speed;
        if (Input.GetKey(KeyCode.LeftArrow)) vel.x = -Speed;
        if (Input.GetKey(KeyCode.UpArrow)) vel.y = Speed;
        if (Input.GetKey(KeyCode.DownArrow)) vel.y = -Speed;

        transform.position += (Vector3)(vel * Time.deltaTime);
        if (vel != Vector2.zero)
        {
            positions.Insert(0, transform.position);
        }
    }

    void LateUpdate()
    {
        // Move each body part
        for (int i = 0; i < bodyParts.Count; i++)
        {
            int index = (i + 1) * 15;
            if (index >= positions.Count)
                {
                    index = positions.Count - 1;
                }
            bodyParts[i].position = positions[index];
        }

        // list might get too larde
        if (positions.Count > 1000)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    void Grow()
    {
        GameObject newPart = Instantiate(BodySegmentPrefab);

        // Just spawn at head — spacing will be handled automatically
        newPart.transform.position = transform.position;

        bodyParts.Add(newPart.transform);
    }

    //This gets called whenever you bump into another object, like a wall or coin.
    private void OnTriggerEnter2D(Collider2D other)
    {
        //This checks to see if the thing you bumped into had the Hazard tag
        //If it does...
        if (other.gameObject.CompareTag("Hazard") || other.gameObject.CompareTag("Wall"))
        {
            //Run your 'you lose' function!
            Die();
        }
        
        //This checks to see if the thing you bumped into has the CoinScript script on it
        CoinScript coin = other.gameObject.GetComponent<CoinScript>();
        RareCoin rare = other.GetComponent<RareCoin>();
        //If it does, run the code block belows
        if (coin != null)
        {
            //Tell the coin that you bumped into them so they can self destruct or whatever
            coin.GetBumped();
            //Make your score variable go up by the coin's value
            Score += coin.Value;
            //And then update the game's score text
            UpdateScore();
            Grow();
            if (Score >= 10)
            {
                Win();
            }
        }
        else if (rare != null)
        {
            rare.GetBumped();
            Score += rare.Value;
            UpdateScore();
            Grow();
            if (Score >= 10)
            {
                Win();
            }
        }
    }

    //This function updates the game's score text to show how many points you have
    //Even if your 'score' variable goes up, if you don't update the text the player doesn't know
    public void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }

    //If this function is called, the player character dies. The game goes to a 'Game Over' screen.
    public void Die()
    {
        SceneManager.LoadScene("Game Over");
    }

     public void Win()
    {
        SceneManager.LoadScene("Win");
    }
}
