// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using Tobii.G2OM;
using UnityEngine;

namespace Tobii.XR.Examples.GettingStarted
{
    //Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGaze : MonoBehaviour, IGazeFocusable
    {
        private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
        public Color highlightColor = Color.red;
        public float animationTime = 0.1f;

        private Renderer _renderer;
        private Color _originalColor;
        private Color _targetColor;
        private float timer = 0;

        private SelectableObject selectableObject;

        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                _targetColor = highlightColor;
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                _targetColor = _originalColor;
            }
        }

        private void Start()
        {
            selectableObject = transform.parent.gameObject.GetComponent<SelectableObject>();
            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
            _targetColor = _originalColor;
        }


        private void Update()
        {
            if(_renderer.material.color != Color.green)
            {
                //This lerp will fade the color of the object
                if (_renderer.material.HasProperty(_baseColor)) // new rendering pipeline (lightweight, hd, universal...)
                {
                    _renderer.material.SetColor(_baseColor, Color.Lerp(_renderer.material.GetColor(_baseColor), _targetColor, Time.deltaTime * (1 / animationTime)));
                }
                else // old standard rendering pipline
                {
                    _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));
                }

                if (_targetColor == highlightColor && selectableObject.testActive)
                {
                    timer += Time.deltaTime;
                    if (timer > TestDataStatic.visionDetectionTime)
                    {
                        _renderer.material.color = Color.green;
                        selectableObject.hasBeenSeen = true;
                        selectableObject.positionWhenSeen = transform.position;
                    }
                }
                else
                {
                    timer = 0;
                }
            }
        }

    }
}
