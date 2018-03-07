using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuNavigator : MonoBehaviour
{

    Dictionary<string, Canvas> canvasList = new Dictionary<string, Canvas>();
    Canvas[] canvases;
    private Canvas m_mainMenu;

    private void Start()
    {
        //m_mainMenu = transform.GetChild(0).GetComponent<Canvas>();

        InitializeCanvases();
        HideAllCanvases();
        if(SceneManager.GetActiveScene().buildIndex == 0)
        GoToCanvas(canvases[0]);
        //HideOtherCanvases();
    }

    private void HideAllCanvases()
    {
        foreach (Canvas canvas in canvasList.Values)
        {
                canvas.gameObject.SetActive(false);
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GoToCanvas(canvases[0]);
        }
    }

    private void InitializeCanvases()
    {
        canvases = GetComponentsInChildren<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if (canvas != GetComponent<Canvas>())
                canvasList.Add(canvas.name, canvas);
        }
    }

    public void GoToCanvas(Canvas canvas)
    {
        //go to targetCanvas, hide others
        HideAllCanvases();
        canvas.gameObject.SetActive(true);

    }
    //public void HideOtherCanvases()
    //{
    //    //go to main menu, hide others.
    //    if (canvasList.ContainsKey(m_mainMenu.name))
    //    {
    //        HideAllCanvases();
    //        GoToCanvas(m_mainMenu);
    //    }
    //}
    public void ExitApplication()
    {
        Debug.Log("You have quit the application!");
        Application.Quit();
    }
    public void GotoGameScene()
    {
        SceneManager.LoadScene(1);
        //goes to the game scene
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}