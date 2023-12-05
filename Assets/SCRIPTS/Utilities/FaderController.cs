using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderController : MonoBehaviour
{
    private Camera Camera;
    private Transform Target;
    private CharacterManager charManager;
    [SerializeField] private Vector3 TargetPositionOffset = Vector3.up;
    [SerializeField] private Vector3 CameraPostionOffset = Vector3.up;
    private RaycastHit[] Hits = new RaycastHit[10];
    [SerializeField] private LayerMask LayerMask;

    private List<GameObject> ObjectsBlockingView = new List<GameObject>();
    private Dictionary<string, Coroutine> RunningCoroutines = new Dictionary<string, Coroutine>();

    // Start is called before the first frame update
    void Start()
    {
        charManager = FindObjectOfType<CharacterManager>();
        Target = charManager.transform.GetChild(0);
        charManager.charChanged.AddListener(UpdateChar);

        Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRay();
    }

    private void UpdateChar()
    {
        Target = charManager.GetCurrentCharacter().transform;
    }

    private void CheckRay()
    {
        if (!Camera.isActiveAndEnabled) return;
        Debug.DrawRay(Camera.transform.position + CameraPostionOffset, (Target.transform.position + TargetPositionOffset - Camera.transform.position).normalized * (Vector3.Distance(Camera.transform.position + CameraPostionOffset , Target.transform.position + TargetPositionOffset)), Color.red);
        int hits = Physics.RaycastNonAlloc(
                Camera.transform.position + CameraPostionOffset,
                (Target.transform.position + TargetPositionOffset - Camera.transform.position).normalized,
                Hits,
                Vector3.Distance(Camera.transform.position + CameraPostionOffset, Target.transform.position + TargetPositionOffset),
                LayerMask
                );
        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                GameObject fadingObject = GetObjectFromHit(Hits[i]);

                if (fadingObject && !ObjectsBlockingView.Contains(fadingObject))
                {
                    if (RunningCoroutines.ContainsKey(fadingObject.name))
                    {
                        if (RunningCoroutines[fadingObject.name] != null)
                        {
                            StopCoroutine(RunningCoroutines[fadingObject.name]);
                        }

                        RunningCoroutines.Remove(fadingObject.name);
                    }

                    RunningCoroutines.Add(fadingObject.name, StartCoroutine(FadingCoroutine(fadingObject)));
                    ObjectsBlockingView.Add(fadingObject);
                }
            }
        }

        FadeObjectsNoLongerBeingHit();
        ClearHits();
    }

    [SerializeField] private float fadeTime = 1f;
    private IEnumerator FadingCoroutine(GameObject obj)
    {
        Debug.Log("Fading");

        float curTime = 0f;
        while(curTime < fadeTime)
        {
            Material[] materials = obj.GetComponentInChildren<Renderer>().materials;
            foreach (Material material in materials)
            {
                material.SetFloat("Fader0102", Mathf.Lerp(1, 0, curTime / fadeTime));
            }

            curTime += Time.deltaTime;
            yield return null;
        }
        //obj.GetComponentInChildren<MeshRenderer>().material.SetFloat("Fader0102", 0);

        if (RunningCoroutines.ContainsKey(obj.name))
        {
            StopCoroutine(RunningCoroutines[obj.name]);
            RunningCoroutines.Remove(obj.name);
        }
    }

    [SerializeField] private float unfadeTime = 1f;
    private IEnumerator UnfadingCoroutine(GameObject obj)
    {
        Debug.Log("Unfading");

        float curTime = 0f;
        while (curTime < fadeTime)
        {
            Material[] materials = obj.GetComponentInChildren<Renderer>().materials;
            foreach (Material material in materials)
            {
                material.SetFloat("Fader0102", Mathf.Lerp(0, 1, curTime / unfadeTime));
            }

            curTime += Time.deltaTime;
            yield return null;
        }
        //obj.GetComponentInChildren<MeshRenderer>().material.SetFloat("Fader0102", 1);

        if (RunningCoroutines.ContainsKey(obj.name))
        {
            StopCoroutine(RunningCoroutines[obj.name]);
            RunningCoroutines.Remove(obj.name);
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<GameObject> objectsToRemove = new List<GameObject>(ObjectsBlockingView.Count);

        foreach (GameObject fadingObject in ObjectsBlockingView)
        {
            bool objectIsBeingHit = false;
            for (int i = 0; i < Hits.Length; i++)
            {
                GameObject hitFadingObject = GetObjectFromHit(Hits[i]);
                if (hitFadingObject != null && fadingObject == hitFadingObject)
                {
                    objectIsBeingHit = true;
                    break;
                }
            }

            if (!objectIsBeingHit)
            {
                if (RunningCoroutines.ContainsKey(fadingObject.name))
                {
                    if (RunningCoroutines[fadingObject.name] != null)
                    {
                        StopCoroutine(RunningCoroutines[fadingObject.name]);
                    }
                    RunningCoroutines.Remove(fadingObject.name);
                }

                RunningCoroutines.Add(fadingObject.name, StartCoroutine(UnfadingCoroutine(fadingObject)));
                objectsToRemove.Add(fadingObject);
            }
        }

        foreach (GameObject removeObject in objectsToRemove)
        {
            ObjectsBlockingView.Remove(removeObject);
        }
    }

    private GameObject GetObjectFromHit(RaycastHit Hit)
    {
        return Hit.collider != null ? Hit.collider.gameObject : null;
    }

    private void ClearHits()
    {
        System.Array.Clear(Hits, 0, Hits.Length);
    }
}
