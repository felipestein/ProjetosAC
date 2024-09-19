using System.Collections;
using UnityEngine;

public class FollowCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private float tParam;
    private float speedModifier;
    private bool isMoving;
    private Vector3 objectPosition;
    private Vector3 nextPosition;

    public enum EasingType { Linear, EaseIn, EaseOut }
    public EasingType easingType = EasingType.Linear;

    void Start()
    {
        tParam = 0f;
        speedModifier = 0.5f;
        isMoving = false;
    }

    public void StartMovement(int routeNum)
    {
        if (!isMoving) 
        {
            StartCoroutine(GoByTheRoute(routeNum));
        }
    }

    public IEnumerator GoByTheRoute(int routeNum)
    {
        isMoving = true;

        Vector3 p0 = routes[routeNum].GetChild(0).position;
        Vector3 p1 = routes[routeNum].GetChild(1).position;
        Vector3 p2 = routes[routeNum].GetChild(2).position;
        Vector3 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            tParam = Mathf.Clamp(tParam, 0, 1);

            float easedT = ApplyEasing(tParam);

            objectPosition = Mathf.Pow(1 - easedT, 3) * p0 + 3 * Mathf.Pow(1 - easedT, 2) * easedT * p1 + 3 * (1 - easedT) * Mathf.Pow(easedT, 2) * p2 + Mathf.Pow(easedT, 3) * p3;

            float nextT = Mathf.Clamp(tParam + 0.01f, 0, 1);
            float easedNextT = ApplyEasing(nextT);

            nextPosition = Mathf.Pow(1 - easedNextT, 3) * p0 + 3 * Mathf.Pow(1 - easedNextT, 2) * easedNextT * p1 + 3 * (1 - easedNextT) * Mathf.Pow(easedNextT, 2) * p2 + Mathf.Pow(easedNextT, 3) * p3;

            transform.position = objectPosition;
            transform.LookAt(nextPosition);

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        isMoving = false;
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

    private float EaseIn(float t)
    {
        return Mathf.Pow(t, 3); 
    }

    private float EaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}