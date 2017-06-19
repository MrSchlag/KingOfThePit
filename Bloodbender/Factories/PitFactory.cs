using Microsoft.Xna.Framework;
using System;

namespace KingOfThePit.Factories
{
    public class PitFactory
    {
        public static void AddPeople(int numberPeople, int pitWidth, int pitHeight, Tuple<int, int> peopleSizeSpread, Vector2 relativePos)
        {
            Random rnd = new Random();
            
            int x = 0;
            int y = 0;

            for (uint i = 0; i < numberPeople; i++)
            {
                var size = rnd.Next(peopleSizeSpread.Item1, peopleSizeSpread.Item2);
                Enemy enemy = new Enemy(new Vector2(x + relativePos.X, y + relativePos.Y), size);
                enemy.addAnimation(new Animation(KingOfThePit.ptr.bouleRouge));
                KingOfThePit.ptr.listGraphicObj.Add(enemy);
                x += peopleSizeSpread.Item2;
                if (x > pitWidth)
                {
                    x = 0;
                    y += peopleSizeSpread.Item2;
                }
            }
            
        }

    }
}
