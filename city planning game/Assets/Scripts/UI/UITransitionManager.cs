using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class UITransitionManager : MonoBehaviour
{
    CanvasGroup cGroup;

    [System.Serializable]
    private class TransitionInstance {
        
        [SerializeField] public string name;
        [SerializeField] public float transitionTime;

        [Space(10)]

        [SerializeField] public bool includePosition;
        [SerializeField] public Vector3 positionEnd;

        [Space(10)]

        [SerializeField] public bool includeScale;
        [SerializeField] public float scaleEnd;

        [Space(10)]

        [SerializeField] public bool includeAlpha;
        [SerializeField] public float alphaEnd;

        [Space(10)]

        [SerializeField] public bool includePostCallback;
        [SerializeField] public EventTrigger.TriggerEvent onTransitionFinish;
    }

    [SerializeField] private TransitionInstance[] _transitionList;

    // Start is called before the first frame update
    void Awake()
    {
        cGroup = GetComponent<CanvasGroup>();
    }

    public void TriggerTransition(string TransitionName) {
        TransitionInstance transition = null;
        foreach(TransitionInstance i in _transitionList) {
            if (i.name == TransitionName) {
                transition = i;
            }
        }
        if (transition == null) {
            return;
        }
        if (transition.includePosition) {
            transform.DOLocalMove(transition.positionEnd, transition.transitionTime);
        }
        if (transition.includeScale) {
            transform.DOScale(transition.scaleEnd, transition.transitionTime);
        }
        if (transition.includeAlpha) {
            cGroup.DOFade(transition.alphaEnd, transition.transitionTime);
        }
        if (transition.includePostCallback) {
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            eventData.selectedObject = this.gameObject;
            DOVirtual.DelayedCall(transition.transitionTime, ()=> transition.onTransitionFinish.Invoke(eventData));
        }
    }

    public void SetActiveCallback(bool setActive) {
        Transform[] children = transform.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in children) {
            if (child.parent == transform) {
                child.gameObject.SetActive(setActive);
            }
        }
    }
}
