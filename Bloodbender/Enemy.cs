using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KingOfThePit
{
    public class Enemy : PhysicObj
    {
        Fixture playerBoundsFix;

        public Enemy(Vector2 position, int size) : base(position, PathFinderNodeType.CENTER)
        {
            KingOfThePit.ptr.shadowsRendering.addShadow(new Shadow(this));

            velocity = 50;

            //CircleShape circle = new CircleShape(size, 1);
            //Fixture playerBoundsFix = body.CreateFixture(circle, new AdditionalFixtureData(this, HitboxType.BOUND));
            Fixture playerBoundsFix = createOctogoneFixture(size, size, Vector2.Zero, new AdditionalFixtureData(this, HitboxType.BOUND));

            //add method to be called on collision, different denpending of fixture
            addFixtureToCheckedCollision(playerBoundsFix);
            body.LinearDamping = 2;

            addComponent(new InactiveComponent(this));
        }

        public override bool Update(float elapsed)
        {
            return base.Update(elapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
