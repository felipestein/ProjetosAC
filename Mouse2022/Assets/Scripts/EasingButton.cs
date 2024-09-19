using UnityEngine;
using UnityEngine.UI;

public class EasingButton : MonoBehaviour
{
    public FollowCurve followCurve;
    public Button linearButton;
    public Button easeInButton;
    public Button easeOutButton;

    void Start()
    {
        linearButton.onClick.AddListener(SetLinearEasing);
        easeInButton.onClick.AddListener(SetEaseInEasing);
        easeOutButton.onClick.AddListener(SetEaseOutEasing);
    }

        void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void SetLinearEasing()
    {
        followCurve.easingType = FollowCurve.EasingType.Linear;
        Debug.Log("Easing definido para Linear");
        StartCoroutine(followCurve.GoByTheRoute(0));
    }

    public void SetEaseInEasing()
    {
        followCurve.easingType = FollowCurve.EasingType.EaseIn;
        Debug.Log("Easing definido para EaseIn");
        StartCoroutine(followCurve.GoByTheRoute(0));  
    }

    public void SetEaseOutEasing()
    {
        followCurve.easingType = FollowCurve.EasingType.EaseOut;
        Debug.Log("Easing definido para EaseOut");
        StartCoroutine(followCurve.GoByTheRoute(0));
    }
}