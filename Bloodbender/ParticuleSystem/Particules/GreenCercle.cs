using KingOfThePit;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfThePit
{
    class GreenCercle : Particule
    {
        public GreenCercle()
        {
            lifeTime = 1.1f;
            timer = lifeTime;

            Texture2D texture = KingOfThePit.ptr.Content.Load<Texture2D>("cercle_vert");
            addAnimation(new Animation(texture));
        }
    }
}
