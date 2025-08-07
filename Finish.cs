using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    public MenuManager script_menu;
    public GameObject congratulations;
    [SerializeField] PlayerActions playeractions;
    [SerializeField] PlayerMovement1 movement;
    private Rigidbody2D player_rigidbody;
    private Animator player_animator;
    private GameObject player;
    public AudioSource Menuagain;

    void Start()
    {
        player = GameObject.Find("Player");
        playeractions = player.GetComponent<PlayerActions>();
        movement = player.GetComponent<PlayerMovement1>();
        player_rigidbody = player.GetComponent<Rigidbody2D>();
        player_animator = player.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {

            CompleteLevel_Tutorial();
        }
    }

    public void CompleteLevel_FullGame()
    {
        MenuManager.LevelCompleted_1 = 1;

        //MenuManager.padlocks_book1[0].SetActive(false);
    }

    public void CompleteLevel_Tutorial()
    {
        congratulations.SetActive(true);
        movement.enabled = false;
        playeractions.enabled = false;
        player_rigidbody.velocity = Vector2.zero;
        player_animator.SetBool("run", false);
    }
    
    public void ReturnToRoom()
    {
        SceneManager.LoadScene("MenuBooks");
        movement.enabled = true;
        playeractions.enabled = true;

    }
}
