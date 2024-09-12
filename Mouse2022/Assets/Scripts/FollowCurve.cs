using System.Collections;
using UnityEngine;

public class FollowCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;
    private float tParam;
    private float speedModifier;
    private bool coroutineAllowed;
    private Vector3 objectPosition;
    private Vector3 nextPosition;
    
    public enum EasingType { Linear, EaseIn, EaseOut } // escolher o easing
    public EasingType easingType = EasingType.Linear; 

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            tParam = Mathf.Clamp(tParam, 0, 1);
            float easedT = ApplyEasing(tParam);

            easedT = Mathf.Clamp(easedT, 0, 1);

            objectPosition = Mathf.Pow(1 - easedT, 3) * p0 + 3 * Mathf.Pow(1 - easedT, 2) * easedT * p1 + 3 * (1 - easedT) * Mathf.Pow(easedT, 2) * p2 + Mathf.Pow(easedT, 3) * p3;

            float nextT = Mathf.Clamp(tParam + 0.01f, 0, 1);
            nextPosition = Mathf.Pow(1 - nextT, 3) * p0 + 3 * Mathf.Pow(1 - nextT, 2) * nextT * p1 + 3 * (1 - nextT) * Mathf.Pow(nextT, 2) * p2 + Mathf.Pow(nextT, 3) * p3;

            transform.position = objectPosition;

            transform.LookAt(nextPosition); // LookAt

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
    }

    private float ApplyEasing(float t)
    {
        switch (easingType)
        {
            case EasingType.EaseIn:
                return EaseIn(t);
            case EasingType.EaseOut:
                return EaseOut(t);
            case EasingType.Linear:
            default:
                return t;
        }
    }

    private float EaseIn(float t) // começa devagar e acelera
    {
        return t * t;
    }

    private float EaseOut(float t) // começa rápido e desacelera
    {
        return 1 - Mathf.Pow(1 - t, 0.5f);
    }
}