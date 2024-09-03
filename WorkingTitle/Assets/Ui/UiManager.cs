using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject pressAnyButtonPanel;
    public GameObject errorScreen;
    //public GameObject startMenuPanel;
    //public GameObject optionsMenu;

    public static UiManager Instance;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this; 
        //    DontDestroyOnLoad(gameObject); 
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        pressAnyButtonPanel.gameObject.SetActive(true);
        //startMenuPanel.gameObject.SetActive(false);
        //optionsMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (pressAnyButtonPanel.gameObject.activeInHierarchy)
        {
            if (Input.anyKey)
            {
                StartTheGame();
            }
        }
    }

        //    if(optionsMenu.gameObject.activeInHierarchy)
        //    {
        //        if(Input.GetKey(KeyCode.Escape))
        //        {
        //            optionsMenu.gameObject.SetActive(false);
        //            startMenuPanel.gameObject.SetActive(true);
        //        }
        //    }
        //}

        public void StartTheGame()
    {
        LoadScene(1);
    }

    public void LoadFirstScene()
    {
        LoadScene(2);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    //public void ToggleOptions()
    //{ 
    //    startMenuPanel.gameObject.SetActive(false);
    //    optionsMenu.gameObject.SetActive(true);
    //}
    
    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
