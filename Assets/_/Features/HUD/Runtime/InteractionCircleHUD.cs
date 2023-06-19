using Interaction.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractionCircleHUD : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Unity API

    private void Awake()
    {
        _baseScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _interactionCircle.m_onCircleOpened += OnCircleOpenedEventHandler;
        _interactionCircle.m_onCircleClosed += OnCircleClosedEventHandler;
    }

    private void OnDisable()
    {
        _interactionCircle.m_onCircleOpened -= OnCircleOpenedEventHandler;
        _interactionCircle.m_onCircleClosed -= OnCircleClosedEventHandler;
    }

    #endregion

    #region Main Methods

    private void OnCircleOpenedEventHandler(object sender, OnCircleOpenedEventArgs e)
    {
        if (e.m_interactions is null) return;

        GenerateCircle(e.m_interactions);
        transform.DOScale(_baseScale, _animationSpeed);
    }

    private void GenerateCircle(ObjectInteraction[] interactions)
    {
        if (interactions is null) return;

        _interactionBubbles = new GameObject[interactions.Length];
        for (int i = 0; i < _interactionBubbles.Length; i++)
        {
            _interactionBubbles[i] = Instantiate(_interactionBubblePrefab);
        }
    }

    private void OnCircleClosedEventHandler(object sender, EventArgs e)
    {
        transform.DOScale(Vector3.zero, _animationSpeed).OnComplete(OnClosedCompleted);
    }

    private void OnClosedCompleted()
    {
        ResetHUD();
    }

    private void ResetHUD()
    {
        foreach (var interactionBubble in _interactionBubbles)
        {
            Destroy(interactionBubble);
            _interactionBubbles = null;
        }
    }

    #endregion

    #region Utils

    #endregion

    #region Private and Protected Members

    [SerializeField] private InteractionCircle _interactionCircle;

    [SerializeField] private GameObject _interactionBubblePrefab;

    [Range(0f, 1f)]
    [SerializeField] private float _animationSpeed = 0.2f;

    private Vector3 _baseScale;
    private GameObject[] _interactionBubbles;

    #endregion
}