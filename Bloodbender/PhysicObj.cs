﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace KingOfThePit
{
    public enum HitboxType
    {
        BOUND = 0,
        ATTACK = 1
    }

    public enum PathFinderNodeType
    {
        OTHER = 0,
        CENTER = 1,
        OUTLINE = 2
    }

    public class PhysicObj : GraphicObj
    {
        public Body body;
        public float velocity;
        public float length;
        public PathFinderNodeType pathNodeType;


        public PhysicObj(Body body, Vector2 position) : base(OffSet.Center)
        {
            velocity = 0;
            this.body = body;
            this.body.Position = position * KingOfThePit.pixelToMeter;
            this.body.BodyType = BodyType.Dynamic;
            this.body.FixedRotation = true;
            this.body.LinearDamping = 1;
            this.body.AngularDamping = 1;
            this.length = 0;

            pathNodeType = PathFinderNodeType.OTHER;
        }

        public PhysicObj(Vector2 position, PathFinderNodeType nodeType = PathFinderNodeType.OTHER) : base(OffSet.Center)
        {
            velocity = 0;
            body = BodyFactory.CreateBody(KingOfThePit.ptr.world);
            body.Position = position * KingOfThePit.pixelToMeter;
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.LinearDamping = 0.02f;
            body.AngularDamping = 1;

            pathNodeType = nodeType;
        }

        public override bool Update(float elapsed)
        {
            position = body.Position * KingOfThePit.meterToPixel;

            return base.Update(elapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public bool collisionBounds(Fixture f1, Fixture f2, Contact contact)
        {
            if (isCollisionPossibleByHeight(f1, f2) == false)
            {
                return false;
            }
            addFixtureOnCollision(f1, f2);
            return true;
        }

        public void separationBounds(Fixture f1, Fixture f2)
        {
            removeFixtureOnSeparation(f1, f2);
        }

        public bool collisionSensor(Fixture f1, Fixture f2, Contact contact)
        {
            if (isCollisionPossibleByHeight(f1, f2) == false)
            {
                return false;
            }
            addFixtureOnCollision(f1, f2);
            return true;
        }

        public void separationSensor(Fixture f1, Fixture f2)
        {
            removeFixtureOnSeparation(f1, f2);
        }

        /* attribut les handlers de collision en fonction du type de la fixture */
        protected void addFixtureToCheckedCollision(Fixture fix)
        {
            if (fix.IsSensor == true)
            {
                fix.OnCollision += collisionSensor;
                fix.OnSeparation += separationSensor;
            }
            else
            {
                fix.OnCollision += collisionBounds;
                fix.OnSeparation += separationBounds;
            }
        }

        private bool isCollisionPossibleByHeight(Fixture f1, Fixture f2)
        {
            if ((AdditionalFixtureData)f1.UserData == null || (AdditionalFixtureData)f2.UserData == null)
                return true;

            PhysicObj p1 = ((AdditionalFixtureData)f1.UserData).physicParent;
            PhysicObj p2 = ((AdditionalFixtureData)f2.UserData).physicParent;

            float p1Top = p1.height + p1.length;
            float p1Down = p1.height;
            float p2Top = p2.height + p2.length;
            float p2Down = p2.height;

            if (p1Top < p2Down || p2Top < p1Down)
                return false;
            return true;
        }

        private void addFixtureOnCollision(Fixture f1, Fixture f2)
        {
            ((AdditionalFixtureData)f1.UserData).isTouching = true;
            ((AdditionalFixtureData)f1.UserData).fixInContactList.Add(f2);
        }

        private void removeFixtureOnSeparation(Fixture f1, Fixture f2)
        {
            AdditionalFixtureData f1data = (AdditionalFixtureData)f1.UserData;
            AdditionalFixtureData f2data = (AdditionalFixtureData)f2.UserData;
            Fixture fixToRemove = null;

            if (f1data == null || f2data == null)
                return;

            foreach (Fixture fixSearched in f1data.fixInContactList)
            {
                if (fixSearched == f2)
                {
                    fixToRemove = fixSearched;
                    break;
                }
            }

            f1data.fixInContactList.Remove(fixToRemove);
            if (f1data.fixInContactList.Count == 0)
                f1data.isTouching = false;
        }

        public void setLinearDamping(float damping)
        {
            body.LinearDamping = damping;
        }

        public void isRotationFixed(bool state)
        {
            body.FixedRotation = state;
        }

        public void setBodyType(BodyType type)
        {
            body.BodyType = type;
        }

        public Fixture createRectangleFixture(float width, float height, Vector2 transalationVector, AdditionalFixtureData userData = null)
        {
            //Create rectangles shapes
            Vertices rectangleVertices = PolygonTools.CreateRectangle((width / 2) * KingOfThePit.pixelToMeter, (height / 2) * KingOfThePit.pixelToMeter);
            PolygonShape rectangleShape = new PolygonShape(rectangleVertices, 1);
            //Transalte rectangles shapes to set there positions
            rectangleShape.Vertices.Translate(transalationVector * KingOfThePit.pixelToMeter);
            //Bind body to shpes (create a compound body) and return
            return (body.CreateFixture(rectangleShape, userData));
        }

        public Fixture createOctogoneFixture(float width, float height, Vector2 translationVector, AdditionalFixtureData userData = null)
        {
            float dh = height / 3f;
            float dw = width / 3f;

            /* Création des vertices correspondant à un octogone inscrit dans le rectangle passé en param */
            Vertices octogonVertices = new Vertices();
            octogonVertices.Add(new Vector2(0, dh) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(0, dh * 2f) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(dw, 0) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(dw * 2, 0) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(dw, height) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(dw * 2, height) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(width, dh) * KingOfThePit.pixelToMeter);
            octogonVertices.Add(new Vector2(width, dh * 2) * KingOfThePit.pixelToMeter);

            /* création de la shape et transaltion pour prendre en compte la position définie par le centre du body */
            octogonVertices.Translate(new Vector2(-width / 2f, -height / 2f) * KingOfThePit.pixelToMeter);
            octogonVertices.Translate(translationVector);
            PolygonShape octogonShape = new PolygonShape(octogonVertices, 20);

            Fixture octogonFix = body.CreateFixture(octogonShape, userData);
            return (octogonFix);
        }

        public override void Dispose()
        {
            
            //KingOfThePit.ptr.world.RemoveBody(body);
            
            body.Dispose();
            base.Dispose();
        }

        protected void createOutlinePathNodes()
        {
            PolygonShape polyShape;
            HitboxType hitboxType;

            if (pathNodeType != PathFinderNodeType.OUTLINE)
                return;

            foreach (Fixture fix in body.FixtureList)
            {
                hitboxType = HitboxType.BOUND;
                if (fix.UserData != null)
                    hitboxType = ((AdditionalFixtureData)fix.UserData).type;
                if (hitboxType == HitboxType.BOUND && fix.Shape is PolygonShape)
                {
                    Console.WriteLine("[PathFinder] one fixture");
                    polyShape = (PolygonShape)fix.Shape;
                    int index = 0;
                    foreach (Vector2 vertex in polyShape.Vertices)
                    {
                        centerToVertexNodeCreate(vertex, polyShape);

                        index++;
                    }
                }
            }
        }

        private void centerToVertexNodeCreate(Vector2 vertex, PolygonShape shape)
        {
          
        }


        public Fixture getBoundsFixture()
        {
            foreach (Fixture fix in body.FixtureList)
            {
                if (((AdditionalFixtureData)fix.UserData).type == HitboxType.BOUND)
                    return fix;
            }
            return null;
        }

        public bool isTouching(PhysicObj obj)
        {
            AdditionalFixtureData fixData;
            foreach (Fixture fix in body.FixtureList)
            {
                fixData = (AdditionalFixtureData)fix.UserData;
                if (fixData.isTouching == true)
                {
                    foreach (Fixture fixtureTouching in fixData.fixInContactList)
                    {
                        if (((AdditionalFixtureData)fixtureTouching.UserData).physicParent.Equals(obj))
                            return true;
                    }
                }
            }
            return false;
        }

        public bool isPointInside(Vector2 point)
        {
            Vector2 vec = body.Position - point;

            if (vec.Length() < maxLenghtCentroidVertex())
                return true;
            return false;
        }

        public float maxLenghtCentroidVertex()
        {
            float maxLenght = 0f;
            float lenght;
            Vertices vertices = ((PolygonShape)getBoundsFixture().Shape).Vertices;

            if (vertices == null)
                return 0f;

            foreach (Vector2 vertex in vertices)
            {
                lenght = (vertex - vertices.GetCentroid()).Length();
                maxLenght = lenght > maxLenght ? lenght : maxLenght;
            }

            return maxLenght;
        }

    }
}
