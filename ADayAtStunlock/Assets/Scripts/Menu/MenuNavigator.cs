using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour {

    List<Canvas> canvasList = new List<Canvas>();
    Canvas[] canvases;
    [Header("Is there a default canvas?")]
    [SerializeField] private bool   m_hasDefaultCanvas;
    [Header("Assign Default Canvas")]
    [Tooltip("Choose a specific canvas to be the default or leave empty to get the first canvas in the hierarchy")]
    [SerializeField] private Canvas m_DefaultCanvas;

    private void Awake()
    {
        InitializeCanvases();
    }

    //
    //Canvas Management 
    //

    /// <summary>
    /// Initialize the scene with the available canvases and the default canvas if there is one.
    /// </summary>
    private void InitializeCanvases()
    {
        canvases = GetComponentsInChildren<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if(canvas != canvases[0])
            {
                canvasList.Add(canvas);
            }  
        }
        if (m_hasDefaultCanvas)
        {
            if (m_DefaultCanvas == null)
            {
                m_DefaultCanvas = canvasList[0];
            }
            m_DefaultCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hides all canvases except the default one (if there is one)
    /// </summary>
    private void HideAllCanvases()
    {
        foreach (Canvas canvas in canvasList)
        {
            canvas.gameObject.SetActive(false);
        }
        if(m_hasDefaultCanvas)
        {
            m_DefaultCanvas.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Go to a specific Canvas and close all others
    /// </summary>
    /// <param name="canvas"></param>
    public void GoToCanvas(Canvas canvas)
    {
        //go to targetCanvas, hide others
        HideAllCanvases();
        canvas.gameObject.SetActive(true);

    }

    /// <summary>
    /// Go to default canvas OR hides all canvases if there is no default canvas
    /// </summary>
    public void GoToDefaultCanvas()
    {

        HideAllCanvases();
        if (!m_hasDefaultCanvas)
        {
           GoToCanvas(m_DefaultCanvas);
        }
        
    }

    //
    //Scene movement
    //

    /// <summary>
    /// Quit Game
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("You have quit the application!");
        Application.Quit();
    }

    /// <summary>
    /// Go to scene by name
    /// </summary>
    /// <param name="sceneName"></param>
    public void GotoScene(Scene sceneName)
    {
        //goes to the game scene
        Debug.Log("You've started up the game, enjoy!");
        SceneManager.LoadScene(sceneName.buildIndex);
    }

    /// <summary>
    /// Go to scene by buildindex number
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void GotoScene(int sceneIndex)
    {
        //goes to the game scene
        Debug.Log("You've started up the game, enjoy!");
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Restart the current active scene
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
