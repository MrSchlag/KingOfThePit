using Microsoft.Xna.Framework;
using System;

namespace KingOfThePit
{
    public class InactiveComponent : IComponent
    {
        private PhysicObj _owner;
        

        private Random _rnd;
        private float _xVelocity;
        private float _yVelocity;
        private Vector2 _initialPosition;

        /// <summary>
        /// Frequency of direction change
        /// </summary>
        public float Amplitude { get; set; }
        private float _apmlitudeCounter;
        private int _goBackToInitialCounter;
        public int Speed { get; set; }
        /// <summary>
        /// Set whether the component is activated or desactivated 
        /// </summary>
        public bool Active { get; set; }

        public InactiveComponent(PhysicObj owner)
        {
            _rnd = new Random(Guid.NewGuid().GetHashCode());
            Speed = 2;
            Amplitude = 1.2f;
            Active = true;
            _owner = owner;
            _apmlitudeCounter = ((float)_rnd.Next(0, (int)Amplitude * 1000)) / 1000;
            _initialPosition = owner.position;
            _goBackToInitialCounter = 0;
        }

        public bool Update(float elapsed)
        {
            if (_apmlitudeCounter >= Amplitude)
            {
                if (_goBackToInitialCounter == 3)
                {
                    Vector2 posToNode = _initialPosition - _owner.position;
                    posToNode.Normalize();
                    _xVelocity = posToNode.X;
                    _yVelocity = posToNode.Y;
                }
                else
                {
                    _xVelocity = _rnd.Next(-Speed, Speed);
                    _yVelocity = _rnd.Next(-Speed, Speed);
                }
                _goBackToInitialCounter = 0;
                _apmlitudeCounter = 0;
            }
            _owner.body.LinearVelocity = new Vector2(_xVelocity * KingOfThePit.pixelToMeter, _yVelocity * KingOfThePit.pixelToMeter);
            _apmlitudeCounter += elapsed;
            _goBackToInitialCounter++;
           
            return true;
        }
    }
}
