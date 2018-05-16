using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPointer
    {
        /*
         Class too much public.
         Should be changed with properties to control usage
         and making data controlling easier/clearer.
            -J.Å
        */

        [Tooltip("Editor window only")]
        public bool removeMe = false;
        public int secondsTilRemoval = 30;

        [Tooltip("Object which the arrow will follow and point at.")]
        public GameObject targetObject;
        //2D canvas-positioning of a 3D object-world
        [HideInInspector]
        public Vector2 convertedTargetObjectPosition;
        [HideInInspector]
        public GameObject arrowPointer;
        public GameObject extraSpritePointer;
        public Sprite arrowSprite = null;
        public Sprite extraSprite = null;
        public Color colorOfArrow = Vector4.zero;
        
        [Header("Extra:")]
        public bool extraFunction = false;
        [HideInInspector]
        public GameObject exclamationMark;
        public Font textFont;
        public Material textMaterial;
        [HideInInspector]
        public bool coroutineRunning = false;
        [HideInInspector]
        public Coroutine coroutine;
        public string textDisplay = string.Empty;
        public Color colorOfText = Vector4.zero;

        public IEnumerator RemoveMe(int secondsUntilRemoved)
        {
            yield return new WaitForSeconds(secondsUntilRemoved);
            listOfStuffToRemove.Add(this);
        }
    }

    #region Variables
    private static ArrowPointer s_myInstance;
    public static ArrowPointer MyInstance
    {
        get { return s_myInstance; }
    }

    public List<ObjectPointer> listOfPointingAt = new List<ObjectPointer>();
    protected static List<ObjectPointer> listOfStuffToRemove = new List<ObjectPointer>();
    private Canvas m_myCanvas;
    private Vector3 m_middlePoint;
    public Sprite defaultTypeOfSprite;
    public Font defaultTextFont;
    public Material defaultTextMaterial;
    #endregion

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        m_myCanvas = GetComponent<Canvas>();

        InitializeVariables();
    }

    private void Update()
    {
        // Point in the middle of the screen
        m_middlePoint = new Vector3(Screen.width / 2, Screen.height / 2);

        UpdateAllArrowObjects();
    }

    #region Functions
    private void InitializeVariables()
    {
        foreach (var item in listOfPointingAt)
        {
            StandardInitObjectPointer(item);

            ExtraInitObjectPointer(item);
        }
    }

    private void StandardInitObjectPointer(ObjectPointer item)
    {
        if (item.arrowPointer != null)
            return;
        item.arrowPointer = new GameObject("Arrow Pointer for: " + item.targetObject.name);
        item.arrowPointer.transform.parent = gameObject.transform;

        Image tempImage;
        item.arrowPointer.AddComponent<RectTransform>();
        tempImage = item.arrowPointer.AddComponent<Image>();
        if (item.arrowSprite == null)
            tempImage.sprite = defaultTypeOfSprite;
        else
            tempImage.sprite = item.arrowSprite;
        if (item.colorOfArrow == new Color(0, 0, 0, 0))
            tempImage.color = Color.yellow;
        else
            tempImage.color = item.colorOfArrow;

        //If an extra sprite is wanted.
        if (item.extraSprite != null)
        {
            item.extraSpritePointer = new GameObject("Extra Sprite");
            item.extraSpritePointer.transform.SetParent(item.arrowPointer.transform);
            item.extraSpritePointer.AddComponent<RectTransform>();
            Image tempImageEX = item.extraSpritePointer.AddComponent<Image>();
            tempImageEX.sprite = item.extraSprite;
            tempImageEX.color = new Color(1, 1, 1, 0.7f);
        }

    }

    private void ExtraInitObjectPointer(ObjectPointer item)
    {
        if (item.extraFunction)
        {
            item.exclamationMark = new GameObject("Exclamation Mark");
            item.exclamationMark.transform.parent = gameObject.transform;
            TextMesh tempTextMesh = item.exclamationMark.AddComponent<TextMesh>();
            if (item.textDisplay == string.Empty)
                tempTextMesh.text = "!";
            else
                tempTextMesh.text = item.textDisplay;

            if (item.textMaterial != null)
                item.exclamationMark.GetComponent<MeshRenderer>().material = item.textMaterial;
            else
                item.exclamationMark.GetComponent<MeshRenderer>().material = defaultTextMaterial;
            
            if (item.textFont != null)
                tempTextMesh.font = item.textFont;
            else
                tempTextMesh.font = defaultTextFont;

            tempTextMesh.alignment = TextAlignment.Center;
            tempTextMesh.anchor = TextAnchor.MiddleCenter;
            tempTextMesh.characterSize = 0.06f;
            tempTextMesh.fontSize = 500;
            if (item.colorOfText == new Color(0, 0, 0, 0))
                tempTextMesh.color = Color.yellow;
            else
                tempTextMesh.color = item.colorOfText;

            item.exclamationMark.SetActive(false);
        }
    }

    private void UpdateAllArrowObjects()
    {
        if (listOfStuffToRemove.Count != 0)
        {
            foreach (var item in listOfStuffToRemove)
            {
                //Destroy gameobject which holds ArrowRelated stuff
                Destroy(item.arrowPointer);
                //Destroy gameobject which holds TextMesh stuff
                Destroy(item.exclamationMark);
                //Remove item from list
                listOfPointingAt.Remove(item);
            }
            //listOfStuffToRemove.Clear();
        }

        foreach (var item in listOfPointingAt)
        {
            //Transform 3D-position to a 2D-canvas local postion
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_myCanvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(item.targetObject.transform.position),
                m_myCanvas.worldCamera,
                out item.convertedTargetObjectPosition);

            //Positioning of the Icons
            item.convertedTargetObjectPosition += new Vector2(Screen.width / 2f, Screen.height / 2f); // add some fixing offset.

            //Set allowed position to keep inside
            Vector3 allowedPos = (Vector3)item.convertedTargetObjectPosition - m_middlePoint;
            allowedPos = Vector3.ClampMagnitude(allowedPos, Screen.height / (2f * 1.1f));

            //Set arrow position to the Vector2 Position it should have but clamping it within a .
            item.arrowPointer.transform.position = m_middlePoint + allowedPos;

            //Rotate the arrow towards the object it follows
            RotateArrowTowardsTarget(item.convertedTargetObjectPosition, item.arrowPointer.transform as RectTransform);

            if (item.extraSpritePointer != null)
            {
                item.extraSpritePointer.transform.localPosition = new Vector3(0, -100, 0);
                item.extraSpritePointer.transform.rotation = Quaternion.identity;
            }

            if (item.extraFunction)
            {
                ExtraFunction(item);
            }
            //Hide arrows when destination is reached.
            if ((Vector2)item.arrowPointer.transform.position == item.convertedTargetObjectPosition)
                item.arrowPointer.gameObject.SetActive(false);
            else
                item.arrowPointer.gameObject.SetActive(true);
        }

        foreach (var item in listOfPointingAt)
        {
            if(item.removeMe)
            {
                StartCoroutine(item.RemoveMe(item.secondsTilRemoval));
            }
        }
    }

    private void RotateArrowTowardsTarget(Vector2 targetPos, RectTransform arrow)
    {
        // Rotation of the Icons
        Vector3 diff = (Vector3)targetPos - arrow.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    /// <summary>
    /// Corotine function. Switches between active false/true.
    /// </summary>
    private IEnumerator Blink(GameObject objectToBlink)
    {
        objectToBlink.SetActive(true);

        while (true && objectToBlink != null)
        {
            yield return new WaitForSeconds(0.5f);
            if (objectToBlink == null)
                break;

            if (objectToBlink.activeSelf)
                objectToBlink.SetActive(false);
            else
                objectToBlink.SetActive(true);
        }
    }

    private void ExtraFunction(ObjectPointer item)
    {
        if ((Vector2)item.arrowPointer.transform.position == item.convertedTargetObjectPosition)
        {
            if (!item.coroutineRunning)
            {
                //item.exclamationMark.SetActive(true);
                item.exclamationMark.transform.position = item.targetObject.transform.position + new Vector3(0, 4, 0);
                item.coroutine = StartCoroutine(Blink(item.exclamationMark));
                item.coroutineRunning = true;
            }
            item.exclamationMark.transform.forward = Camera.main.transform.forward;
        }
        else
        {
            if (item.coroutine != null)
                StopCoroutine(item.coroutine);
            item.exclamationMark.SetActive(false);
            item.coroutineRunning = false;
        }
    }

    /// <summary>
    /// Adds an arrow to point at the "targetToPoint", extra function true will give exclamation mark and more on the arrow aswell.
    /// This will have all default values.
    /// </summary>
    /// <param name="targetToPoint"></param>
    /// <param name="extraFunction"></param>
    public void AddObjectToPointAt(GameObject targetToPoint, bool extraFunction)
    {
        ObjectPointer temp = new ObjectPointer();
        temp.targetObject = targetToPoint;
        StandardInitObjectPointer(temp);

        if (extraFunction)
        {
            temp.extraFunction = true;
            ExtraInitObjectPointer(temp);
        }

        listOfPointingAt.Add(temp);
    }
    /// <summary>
    /// Potential for customizing your arrow and its visuals, but untested so be careful.
    /// With this you can change values to not be the defaults.
    /// </summary>
    /// <param name="targetToPoint"></param>
    /// <param name="customData"></param>
    public void AddObjectToPointAt(GameObject targetToPoint, ObjectPointer customData)
    {
        customData.targetObject = targetToPoint;
        StandardInitObjectPointer(customData);

        //Does not need to check if extra function, the parameter, since custom data can set this to either true or false. Then the function under here checks wether it is false or true before initializing it.
        ExtraInitObjectPointer(customData);

        listOfPointingAt.Add(customData);
    }
    public void RemoveObjectToPointAt(GameObject targetToRemove)
    {
        foreach (var item in listOfPointingAt)
        {
            if (item.targetObject == targetToRemove)
            {
                listOfStuffToRemove.Add(item);
            }
        }
    }
    #endregion
}
