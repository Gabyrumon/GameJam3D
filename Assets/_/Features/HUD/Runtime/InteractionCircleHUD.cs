using Interaction.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCircleHUD : MonoBehaviour
{
    #region Public Members

    #endregion

    #region Unity API

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
        throw new NotImplementedException();
    }

    private void OnCircleClosedEventHandler(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Utils

    #endregion

    #region Private and Protected Members

    [SerializeField] private InteractionCircle _interactionCircle;

    #endregion
}