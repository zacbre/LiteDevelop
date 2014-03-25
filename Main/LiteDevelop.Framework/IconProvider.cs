
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Provides members for getting icons from an image list.
    /// </summary>
    public abstract class IconProvider
    {
        static IconProvider()
        {
            IconProviders = new EventBasedCollection<IconProvider>()
            {
                new AssemblyIconProvider(),
                new SolutionExplorerIconProvider(),
                new ErrorIconProvider(),
            };
        }

        /// <summary>
        /// Gets the icon provider by its type or null if the type is not registered.
        /// </summary>
        /// <typeparam name="T">The type of the icon provider to get.</typeparam>
        /// <returns>An icon provider of type <typeparamref name="T"/>, or null if none can be found.</returns>
        public static T GetProvider<T>() where T: IconProvider
        {
            return IconProviders.FirstOrDefault(x => x is T) as T;
        }

        /// <summary>
        /// Gets the icon provider by its type or null if the type is not registered.
        /// </summary>
        /// <param name="type">The type of the icon provider to get.</param>
        /// <returns>An icon provider of type <paramref name="type"/>, or null if none can be found.</returns>
        public static object GetProvider(Type type)
        {
            return IconProviders.FirstOrDefault(x => x.GetType() == type);
        }

        /// <summary>
        /// Gets a collection of registered icon providers.
        /// </summary>
        public static EventBasedCollection<IconProvider> IconProviders
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an icon from a sprite table image.
        /// </summary>
        /// <param name="spriteTable">The sprite table to get the icon from.</param>
        /// <param name="iconSize">The size of a single icon.</param>
        /// <param name="index">The index of the icon to get.</param>
        /// <returns>A copy of a piece of the given sprite table.</returns>
        public static Bitmap GetIconFromSpriteTable(Bitmap spriteTable, Size iconSize, int index)
        {
            var picture = new Bitmap(iconSize.Width, iconSize.Height);
            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.DrawImage(spriteTable, new Rectangle(new Point(), iconSize), new Rectangle(new Point(index * iconSize.Width, 0), iconSize), GraphicsUnit.Pixel);
            }
            return picture;
               
        }

        /// <summary>
        /// Gets the image list containing the icons this IconProvider holds.
        /// </summary>
        public abstract ImageList ImageList { get; }

        /// <summary>
        /// Gets the image index of a specific member for the image list of this IconProvider.
        /// </summary>
        /// <param name="member">The member to get the icon from.</param>
        /// <returns>A zero-based index of an image in the <see cref="ImageList"/> property.</returns>
        public abstract int GetImageIndex(object member);
        
    }
}
