using FarseerPhysics.Dynamics;
using KingOfThePit;
using System.Collections.Generic;

namespace KingOfThePit
{
    public class AdditionalFixtureData
    {
        public PhysicObj physicParent;
        public HitboxType type;
        public bool isTouching;
        public List<Fixture> fixInContactList;

        public AdditionalFixtureData(PhysicObj parent, HitboxType type)
        {
            physicParent = parent;
            this.type = type;
            isTouching = false;
            fixInContactList = new List<Fixture>();
        }
    }
}
