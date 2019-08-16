using System.Collections.Generic;
using System.Linq;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;

namespace Quaver.Shared.Screens.Selection.UI.Mapsets
{
    public class DrawableMapMetadataContainer : IDrawableMapMetadata
    {
        /// <summary>
        /// </summary>
        private List<DrawableMapMetadata> Metadata { get; }

        /// <summary>
        /// </summary>
        public DrawableMapMetadataContainer(Drawable parent, List<DrawableMapMetadata> metadata)
        {
            Metadata = metadata;

            for (var i = 0; i < Metadata.Count; i++)
            {
                var m = Metadata[i];
                m.Parent = parent;
                m.Alignment = Alignment.MidRight;
                m.X = -100 * i - 18;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Open() => Metadata.ForEach(x => x.Open());

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public void Close() => Metadata.ForEach(x => x.Close());
    }
}