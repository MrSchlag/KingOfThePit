using FarseerPhysics;
using FarseerPhysics.DebugView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfThePit
{
    public class DebugView : DebugViewXNA
    {
        ParticuleSystem ps;

        public DebugView() : base(KingOfThePit.ptr.world)
        {
            ps = new ParticuleSystem();

            AppendFlags(DebugViewFlags.PathFinding);
        }

        public void Update(float elapsed)
        {
            ps.Update(elapsed);
        }


        public override void RenderDebugData(ref Matrix projection, ref Matrix view)
        {
            if (!Enabled)
                return;

            //Nothing is enabled - don't draw the debug view.
            if (Flags == 0)
                return;

            _device.RasterizerState = RasterizerState.CullNone;
            _device.DepthStencilState = DepthStencilState.Default;

            _primitiveBatch.Begin(ref projection, ref view);

            DrawDebugData();

            _primitiveBatch.End();

            if ((Flags & DebugViewFlags.PerformanceGraph) == DebugViewFlags.PerformanceGraph)
            {
                _primitiveBatch.Begin(ref _localProjection, ref _localView);
                DrawPerformanceGraph();
                _primitiveBatch.End();
            }

            // begin the sprite batch effect
            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // draw any strings we have
            for (int i = 0; i < _stringData.Count; i++)
            {
                _batch.DrawString(_font, _stringData[i].Text, _stringData[i].Position, _stringData[i].Color);
            }

            // end the sprite batch effect
            _batch.End();


            //draw selected paths
            KingOfThePit.ptr.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, KingOfThePit.ptr.camera.GetView());

            ps.Draw(KingOfThePit.ptr.spriteBatch);

            KingOfThePit.ptr.spriteBatch.End();

            _stringData.Clear();
        }
    }
}
