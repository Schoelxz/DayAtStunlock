using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPointer
    {
        public GameObject targetObject;
        //2D canvas-positioning of a 3D object-world
        [HideInInspector]
        public Vector2 convertedTargetObjectPosition;

        [HideInInspector]
        public GameObject arrowPointer;

        public bool extraFunction = false;
        [HideInInspector]
        public GameObject exclamationMark;
        public Font textFont;
        public Material textMaterial;
        [HideInInspector]
        public bool coroutineRunning = false;
        [HideInInspector]
        public Coroutine coroutine;
    }

    #region Variables
    private static ArrowPointer s_myInstance;

    public List<ObjectPointer> listOfPointingAt = new List<ObjectPointer>();
    public Sprite typeOfSprite;
    private Canvas m_myCanvas;
    private Vector3 m_middlePoint;
    #endregion
    
    private void Start()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);

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
            item.arrowPointer = new GameObject("Arrow Pointer for: " + item.targetObject.name);
            item.arrowPointer.transform.parent = gameObject.transform;
            Image tempImage;
            item.arrowPointer.AddComponent<RectTransform>();
            tempImage = item.arrowPointer.AddComponent<Image>();
            tempImage.sprite = typeOfSprite;
            tempImage.color = Color.yellow;

            if (item.extraFunction)
            {
                item.exclamationMark = new GameObject("Exclamation Mark");
                item.exclamationMark.transform.parent = gameObject.transform;
                TextMesh tempTextMesh = item.exclamationMark.AddComponent<TextMesh>();
                item.exclamationMark.GetComponent<MeshRenderer>().material = item.textMaterial;
                tempTextMesh.text = "!";
                tempTextMesh.font = item.textFont;
                tempTextMesh.alignment = TextAlignment.Center;
                tempTextMesh.anchor = TextAnchor.MiddleCenter;
                tempTextMesh.characterSize = 0.06f;
                tempTextMesh.fontSize = 500;
                tempTextMesh.color = Color.yellow;

                item.exclamationMark.SetActive(false);
            }
        }
    }

    private void UpdateAllArrowObjects()
    {
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
    }

    private void RotateArrowTowardsTarget(Vector2 targetPos, RectTransform arrow)
    {
        // Rotation of the Icons
        Vector3 diff = (Vector3)targetPos - arrow.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private IEnumerator Blink(GameObject objectToBlink)
    {
        objectToBlink.SetActive(true);

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

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
    #endregion
}
