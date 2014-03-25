using System;
using System.Linq;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Framework.Extensions
{
    public interface IAppearanceMapProvider
    {
        AppearanceMap CurrentAppearanceMap { get; }

        AppearanceMap DefaultAppearanceMap { get; }
    }
}
