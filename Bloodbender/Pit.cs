using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KingOfThePit
{
    public class Pit
    {
        public Vector2 PogoCenter { get; set; }
        public float PogoRadius { get; set; }
        public int PogoSpotNumber { get; set; }

        private List<Vector2> _pogoSpots;

        public Pit()
        {
            PogoRadius = 250f;
            PogoSpotNumber = 20;
            _pogoSpots = new List<Vector2>();
        }

        public void GeneratePit()
        {
            CreatePogoSpots();
        }

        private void CreatePogoSpots()
        {
            var portion = 360 / PogoSpotNumber;
            var angle = 0;
            float angleRad;
            var centerToEdgePogoVec = new Vector2(PogoRadius, 0);
            
            for (int i = 0; i < PogoSpotNumber; i++)
            {
                _pogoSpots.Add(centerToEdgePogoVec);
                angle += portion;
                angleRad = angle * (float)Math.PI / 180;
                Console.WriteLine("centerToEdge : {0} deg : {1} rad : {2}", centerToEdgePogoVec, angle, angleRad);

                centerToEdgePogoVec = new Vector2(centerToEdgePogoVec.X * (float)Math.Cos(angleRad) - centerToEdgePogoVec.Y * (float)Math.Sin(angleRad),
                    centerToEdgePogoVec.X * (float)Math.Sin(angleRad) + centerToEdgePogoVec.Y * (float)Math.Cos(angleRad));
                
            }
        }
    }
}
