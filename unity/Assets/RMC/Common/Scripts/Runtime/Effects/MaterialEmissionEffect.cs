using UnityEngine;

namespace RMC.Common.Effects
{
    /// <summary>
    /// Pulsate the material
    /// </summary>
    public class MaterialEmissionEffect : MonoBehaviour
    {
        // ------------------ Constants and statics
        private const string EmissionColor = "_EmissionColor";
        private const string Emission = "_EMISSION";
       

        // ------------------ Events

        // ------------------ Serialized fields and properties
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _toIntensity = 1;

        // ------------------ Non-serialized fields

        private float _currentIntensity;
        private Color _fromColor;
        private Material _material;

       

        // ------------------ Methods
        protected void Start ()
        {
            _material = _renderer.material;
            _fromColor = _material.GetColor(EmissionColor);

        }

       

        protected void Update()
        {
            _currentIntensity = Mathf.PingPong (Time.time * _speed, _toIntensity);
            _material.SetColor (EmissionColor, _fromColor * _currentIntensity);
            _material.EnableKeyword(Emission);

            DynamicGI.SetEmissive(_renderer, _fromColor * _currentIntensity);
            //DynamicGI.SetEmissive(_renderer, _fromColor * Mathf.LinearToGammaSpace (_currentIntensity));
            DynamicGI.UpdateMaterials(_renderer);
            DynamicGI.UpdateEnvironment();

        }


    }
}