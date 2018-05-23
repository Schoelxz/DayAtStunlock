using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuNavigator : MonoBehaviour
{
    /// <summary>
    /// Singleton behaviour variable.
    /// </summary>
    public static GameObject s_menuHolder;

    /// <summary>
    /// Canvas array description:
    /// <para> canvases[0] = Parent Canvas for all menu related canvases </para>
    /// <para> canvases[1] = Main Menu Canvas                            </para>
    /// <para> canvases[2] = Config Canvas                               </para>
    /// <para> canvases[3] = Credits Canvas                              </para>
    /// <para> canvases[4] = IngameMenu Canvas                           </para>
    /// <para> canvases[5] = GameHud Canvas                              </para>
    /// <para> canvases[6] = EndGame Canvas                              </para>
    /// </summary>
    Canvas[] canvases;

    private GameObject m_InGameMenu;
    private Canvas m_DefaultCanvas;

    public GameObject splashScreen;

    private void Awake()
    {
        // if copies of this script is created, destroy the copy (aswell as the gameobject it sits on).
        if (s_menuHolder == null)
            s_menuHolder = gameObject;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitializeCanvases();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DAS.TimeSystem.ResumeTime();
        if (scene.name == "MainMenu")
        {
            DAS.TimeSystem.TimePassedSeconds = 0;
            GotoMainMenu();
        }
        else
        {
            DAS.TimeSystem.TimePassedSeconds = 0;
            GotoGameCanvas();
            splashScreen.SetActive(true);
            DAS.TimeSystem.PauseTime();
        }


        canvases[4].gameObject.SetActive(false);
    }

    private void Start()
    {
        if (m_DefaultCanvas == null)
            Debug.LogAssertion("m_DefaultCanvas is null, maybe check if the script is in the correct spot?");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            //StartCoroutine(ToggleInGameMenu(0.07f));
            ToggleInGameMenu();
        }
    }

    #region Canvas Management
    /// <summary>
    /// Initialize the scene with the available canvases and the default canvas if there is one.
    /// </summary>
    private void InitializeCanvases()
    {
        canvases = GetComponentsInChildren<Canvas>(true);

        if (m_DefaultCanvas == null)
        {
            m_DefaultCanvas = canvases[0];
        }

        m_InGameMenu = canvases[4].gameObject;
    }

    private void HideMainMenuMenus()
    {
        canvases[1].gameObject.SetActive(false);
        canvases[2].gameObject.SetActive(false);
        canvases[3].gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Hides all canvases except the default one (if there is one)
    /// </summary>
    public void HideAllCanvases()
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }
        if (!m_DefaultCanvas.gameObject.activeInHierarchy)
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
    /// Hides all canvases and shows the main menu canvas.
    /// </summary>
    public void GotoMainMenu()
    {
        HideAllCanvases();
        canvases[1].gameObject.SetActive(true);
    }

    public void GotoGameCanvas()
    {
        HideAllCanvases();
        canvases[5].gameObject.SetActive(true);
        DAS.TimeSystem.TimePassedSeconds = 0;
    }

    /// <summary>
    /// Go to default canvas OR hides all canvases if there is no default canvas
    /// </summary>
    public void GoToDefaultCanvas()
    {
        HideAllCanvases();
        if (m_DefaultCanvas != null)
        {
            GoToCanvas(m_DefaultCanvas);
            // Need to show main menu canvas aswell. Code is starting to get weird ;/.
            canvases[1].gameObject.SetActive(true);
        }
        else
            Debug.LogAssertion("Default canvas is null?. Fix it!");
    }

    /// <summary>
    /// Opens or Closes the Game Menu
    /// </summary>
    public void ToggleInGameMenu()
    {
        m_InGameMenu.gameObject.SetActive(!m_InGameMenu.gameObject.activeSelf);
        if (!m_InGameMenu.gameObject.activeSelf)
        {
            DAS.TimeSystem.ResumeTime();
        }
        else
        {
            DAS.TimeSystem.PauseTime();
        }
    }
    /// <summary>
    /// Opens or Closes the Game Menu but with a delay if coroutined.
    /// </summary>
    public IEnumerator ToggleInGameMenu(float waitTime)
    {
        if(Time.timeScale != 0)
            yield return new WaitForSeconds(waitTime);

        m_InGameMenu.gameObject.SetActive(!m_InGameMenu.gameObject.activeSelf);
        if (!m_InGameMenu.gameObject.activeSelf)
        {
            DAS.TimeSystem.ResumeTime();
        }
        else
        {
            DAS.TimeSystem.PauseTime();
        }

        yield return new WaitForSeconds(waitTime);
    }
    #endregion

    #region Scene Management
    /// <summary>
    /// Quit Game
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("You have quit the application!");
        Application.Quit();
    }

    /// <summary>
    /// Go to scene by scene
    /// </summary>
    /// <param name="scene"></param>
    public void GotoScene(Scene scene)
    {
        //goes to the game scene
        Debug.Log("You've started scene: " + scene.name);
        SceneManager.LoadScene(scene.buildIndex);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
    public void GotoScene(string sceneName)
    {
        //goes to the game scene
        Debug.Log("You've started scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// Go to scene by buildindex number
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void GotoScene(int sceneIndex)
    {
        //goes to the game scene
        Debug.Log("You've started scene: " + SceneManager.GetSceneAt(sceneIndex).name);
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Restart the current active scene
    /// </summary>
    public void RestartScene()
    {
        Debug.Log("You've restarted scene: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DAS.TimeSystem.ResetTime();
    }
    #endregion
}
