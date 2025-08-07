using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject Books_Background;
    [SerializeField] private GameObject StartingSceneTransition;
    [SerializeField] private GameObject MenuSound;
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private AudioSource BooksMenuSound;
    [SerializeField] private GameObject SkipButton;

    [SerializeField] private GameObject OpenBook_1, OpenBook_2, OpenBook_3;

    [SerializeField] private GameObject Warning_1, Warning_2, Warning_3;

    [SerializeField] private GameObject Book_2, Book_3;

    [SerializeField] public GameObject[] padlocks_book1, padlocks_book2, padlocks_book3;

    [SerializeField] public static int LevelCompleted_1 = 1;
    [SerializeField] public static int LevelCompleted_2 = 0;
    [SerializeField] public static int LevelCompleted_3 = 1;
    [SerializeField] public static int first_cutscene;
    private bool isCutscenePlaying = false;

    public static bool book1_completed, book2_completed, book3_completed;

    public PlayableDirector playableDirector;

    [SerializeField] private int BookNumber;

    [SerializeField] CanvasGroup canvas_button_1;

    void Start()
    {

        if(first_cutscene==0)
        {
            Cutscene.SetActive(true);

            if (playableDirector != null)
            {
                playableDirector.stopped += OnCutsceneFinished;
                playableDirector.Play();
                isCutscenePlaying = true;
                
            }
        }

        if(first_cutscene==1)
        {
            Books_Background.SetActive(true);
            BooksMenuSound.Play();

        }

        Book_2.SetActive(true);
        Book_3.SetActive(true);


        for (int i = 1; i < padlocks_book1.Length+1; i++)
        {
            if (LevelCompleted_1 >= i)
            {
                padlocks_book1[i-1].SetActive(false);
            }
        }

        for (int i = 1; i < padlocks_book2.Length+1; i++)
        {
            if (LevelCompleted_2 >= i)
            {
                padlocks_book2[i-1].SetActive(false);
            }
        }

        for (int i = 1; i < padlocks_book3.Length+1; i++)
        {
            if (LevelCompleted_3 >= i)
            {
                padlocks_book3[i-1].SetActive(false);
            }
        }

        if(book1_completed)
        {

            for (int i = 1; i < padlocks_book1.Length+1; i++)
            {
                    padlocks_book1[i-1].SetActive(false);
            }

            Book_2.SetActive(true);
        }
        
        if(book2_completed)
        {

            for (int i = 1; i < padlocks_book2.Length+1; i++)
            {
                    padlocks_book2[i-1].SetActive(false);
            }
            Book_3.SetActive(true);
        }
    }
    
    public void Skipbutton()
    {
        SkipButton.SetActive(false);
        Cutscene.SetActive(false);
        Books_Background.SetActive(true);
        BooksMenuSound.Play();
        first_cutscene=1;
    }


    private void OnCutsceneLoaded(AsyncOperation asyncOp)
        {
            Debug.Log("Cutscene loaded");
        }


    public void Book1()
    {
        OpenBook_1.SetActive(true);
        BookNumber = 1;
    
    }

    public void Book2()
    {
        OpenBook_2.SetActive(true);
        BookNumber = 2;
    }

    public void Book3()
    {
        OpenBook_3.SetActive(true);
        BookNumber = 3;
    }

    public void CloseBook1()
    {
        OpenBook_1.SetActive(false);

        Warning_1.SetActive(false);

    }

    public void CloseBook2()
    {
        OpenBook_2.SetActive(false);
        Warning_2.SetActive(false);
    }

    public void CloseBook3()
    {
        OpenBook_3.SetActive(false);
        Warning_3.SetActive(false);

    }

    public void EnterLvl_1()
    {
        switch (BookNumber)
            {
                case 1:
                    SceneManager.LoadScene("Ch1");
                    if(LevelCompleted_1 < 1)
                    {
                        //SceneManager.LoadScene("Level0");
                        LevelCompleted_1 = 1;
                        //padlocks_book1[0].SetActive(false);
                        Debug.Log("You entered the level");
                    }
                    break;

                case 2:
                    SceneManager.LoadScene("Ch1_2");
                    if(LevelCompleted_2 < 1)
                    {
                        SceneManager.LoadScene("Level0");
                        Debug.Log(LevelCompleted_2);
                        LevelCompleted_2 = 0;
                        //padlocks_book2[0].SetActive(false);
                        Debug.Log("You entered the level");
                    }
                    break;

                case 3:
                    SceneManager.LoadScene("Ch1_3");
                    if(LevelCompleted_3 < 1)
                    {
                        LevelCompleted_3 = 1;
                        //padlocks_book3[0].SetActive(false);
                        Debug.Log("You entered the level");
                    }
                    break;
            }
        
    }


    public void EnterLvl_2()
    {
            switch (BookNumber)
            {
                case 1:
                    SceneManager.LoadScene("Ch2_1");
                    if (LevelCompleted_1 >= 1)
                        {
                            // SceneManager.LoadScene("Level2_1");
                            if(LevelCompleted_1 < 2)
                            {
                                LevelCompleted_1 = 1;
                                //padlocks_book1[1].SetActive(false);
                                Debug.Log("You entered the level");
                            }
                            else
                            {
                                // SceneManager.LoadScene("Level2_1");
                                Debug.Log("You entered the level woho");
                            }

                        }
                    else
                        {
                            Warning_1.SetActive(true);
                        }

                    break;

                case 2:

                    if (LevelCompleted_2 >= 1)
                            {
                                // SceneManager.LoadScene("Level2_2");
                                if(LevelCompleted_2 < 2)
                                {
                                    LevelCompleted_2 = 0;
                                    //padlocks_book2[1].SetActive(false);
                                    Debug.Log("You entered the level");
                                    break;
                                }
                                else
                                {
                                    // SceneManager.LoadScene("Level2_2");
                                    Debug.Log("You entered the level woho Book2");
                                }

                            }
                    else
                        {
                            Warning_2.SetActive(true);
                        }
                    break;

                case 3:
                    SceneManager.LoadScene("Boos");
                    if (LevelCompleted_3 >= 1)
                                {
                                    // SceneManager.LoadScene("Level2_3");
                                    if(LevelCompleted_3 < 2)
                                    {
                                        LevelCompleted_3 = 1;
                                        //padlocks_book3[1].SetActive(false);
                                        Debug.Log("You entered the level");
                                    }
                                    else
                                    {
                                        // SceneManager.LoadScene("Level2_3");
                                        Debug.Log("You entered the level woho Book2");
                                    }

                                }
                            
                            else
                                {
                                    Warning_3.SetActive(true);
                                }
                            break;
            
        }
    
    }

   public void EnterLvl_3()
    {
            switch (BookNumber)
            {
                case 1:
                    if (LevelCompleted_1 >= 2)
                        {
                            // SceneManager.LoadScene("Level3_1");
                            if(LevelCompleted_1 < 3)
                            {
                                LevelCompleted_1 = 2;
                                //padlocks_book1[2].SetActive(false);
                                //LevelCompleted_1 = 3;
                                //padlocks_book1[2].SetActive(false);
                                Debug.Log("You entered the level");
                            }
                            else
                            {
                                // SceneManager.LoadScene("Level3_1");
                                Debug.Log("You entered the level woho");
                            }

                        }
                    
                    else
                        {
                            Warning_1.SetActive(true);
                        }
                    break;

                case 2:

                    if (LevelCompleted_2 >= 2)
                            {
                                // SceneManager.LoadScene("Level3_2");
                                if(LevelCompleted_2 < 3)
                                {
                                    LevelCompleted_2 = 2;
                                    //LevelCompleted_2 = 3;
                                    //padlocks_book2[2].SetActive(false);
                                    Debug.Log("You entered the level");
                                }
                                else
                                {
                                    // SceneManager.LoadScene("Level3_2");
                                    Debug.Log("You entered the level woho Book2");
                                }
                            }
                        
                        else
                            {
                                Warning_2.SetActive(true);
                            }
                        break;

                case 3:

                    if (LevelCompleted_3 >= 2)
                                {
                                    // SceneManager.LoadScene("Level3_3");
                                    if(LevelCompleted_3 < 3)
                                    {
                                        LevelCompleted_3 = 2;
                                        //LevelCompleted_3 = 3;
                                        //padlocks_book3[2].SetActive(false);
                                        padlocks_book3[2].SetActive(false);
                                        Debug.Log("You entered the level");
                                    }
                                    else
                                    {
                                        // SceneManager.LoadScene("Level3_3");
                                        Debug.Log("You entered the level woho Book2");
                                    }

                                }
                            
                            else
                                {
                                    Warning_3.SetActive(true);
                                }
                            break;
            
        }
    
    }
    
    public void EnterLvl_4()
    {
            switch (BookNumber)
            {
                case 1:

                    if (LevelCompleted_1 >= 3)
                        {
                            // SceneManager.LoadScene("Level4_1");
                            if(LevelCompleted_1 < 4)
                            {
                                LevelCompleted_1 = 4;
                                padlocks_book1[3].SetActive(false);
                                Debug.Log("You entered the level");
                            }
                            else
                            {
                                // SceneManager.LoadScene("Level4_1");
                                Debug.Log("You entered the level woho");
                            }
                        }
                    
                    else
                        {
                            Warning_1.SetActive(true);
                        }
                    break;

                case 2:

                    if (LevelCompleted_2 >= 3)
                            {
                                // SceneManager.LoadScene("Level4_2");
                                if(LevelCompleted_2 < 4)
                                {
                                    LevelCompleted_2 = 4;
                                    padlocks_book2[3].SetActive(false);
                                    Debug.Log("You entered the level");
                                }
                                else
                                {
                                    // SceneManager.LoadScene("Level4_2");
                                    Debug.Log("You entered the level woho Book2");
                                }

                            }
                        
                    else
                        {
                            Warning_2.SetActive(true);
                        }
                    break;

                case 3:

                    if (LevelCompleted_3 >= 3)
                                {
                                    // SceneManager.LoadScene("Level4_3");
                                    if(LevelCompleted_3 < 4)
                                    {
                                        LevelCompleted_3 = 4;
                                        padlocks_book3[3].SetActive(false);
                                        Debug.Log("You entered the level");
                                    }
                                    else
                                    {
                                        // SceneManager.LoadScene("Level4_3");
                                        Debug.Log("You entered the level woho Book2");
                                    }
                                }
                            
                            else
                                {
                                    Warning_3.SetActive(true);
                                }
                            break;
            
        }
    
    }
    
        public void EnterLvl_5()
    {
            switch (BookNumber)
            {
                case 1:

                    if (LevelCompleted_1 >= 4)
                        {
                            // SceneManager.LoadScene("Level5_1");
                            if(LevelCompleted_1 < 5)
                            {
                                LevelCompleted_1 = 5;
                                Debug.Log("You entered the level");
                            }
                            else
                            {
                                // SceneManager.LoadScene("Level5_1");
                                Debug.Log("You entered the level woho");
                            }

                            book1_completed = true;
                            Book_2.SetActive(true);
                        }
                    
                    else
                        {
                            Warning_1.SetActive(true);
                        }
                    break;

                case 2:

                    if (LevelCompleted_2 >= 4)
                            {
                                // SceneManager.LoadScene("Level5_2");
                                if(LevelCompleted_2 < 5)
                                {
                                    LevelCompleted_2 = 5;
                                    Debug.Log("You entered the level");
                                }
                                else
                                {
                                    // SceneManager.LoadScene("Level5_2");
                                    Debug.Log("You entered the level woho Book2");
                                }
                                book2_completed = true;
                                Book_3.SetActive(true);

                            }
                        
                        else
                            {
                                Warning_2.SetActive(true);
                            }
                        break;

                case 3:

                    if (LevelCompleted_3 >= 4)
                                {
                                    // SceneManager.LoadScene("Level5_3");
                                    if(LevelCompleted_3 < 5)
                                    {
                                        LevelCompleted_3 = 5;
                                        Debug.Log("You entered the level");
                                    }
                                    else
                                    {
                                        // SceneManager.LoadScene("Level5_3");
                                        Debug.Log("You entered the level woho Book2");
                                    }

                                    book3_completed = true;
                                }
                            
                            else
                                {
                                    Warning_3.SetActive(true);
                                }
                            break;
            
        }
    
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Books_Background.SetActive(true);
        isCutscenePlaying = false;
        first_cutscene = 1;
    }


}
