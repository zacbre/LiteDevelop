using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDevelop.Framework.Languages.Web
{
    public abstract class WebLanguageDescriptor : LanguageDescriptor
    {
        /// <inheritdoc />
        public override string LanguageOrder
        {
            get { return "Web"; }
        }
    }
}
