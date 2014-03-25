using System;
using System.Linq;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net.VisualBasic
{
    /// <summary>
    /// Represents a Visual Basic project which can be built using the Visual Basic compiler.
    /// </summary>
    public class VisualBasicProject : NetProject
    {
        private static VisualBasicProjectDescriptor _descriptor =  ProjectDescriptor.GetDescriptor<VisualBasicProjectDescriptor>();
    	
        public VisualBasicProject(string name)
            : base(name, _descriptor)
    	{
            OptionStrict = true; // let's learn visual basic coders to work with option strict turned on :3
            OptionExplicit = true;
    	}

    	public VisualBasicProject(FilePath filePath)
    		: base(filePath)
    	{
    	}

        /// <inheritdoc />
        public override LanguageDescriptor Language
        {
            get { return LanguageDescriptor.GetLanguage<VisualBasicLanguage>(); }
        }

        /// <inheritdoc />
        public override ProjectDescriptor ProjectDescriptor
        {
            get { return _descriptor; }
        }

        /// <summary>
        /// Determines whether option strict is turned on.
        /// </summary>
        public bool OptionStrict
        {
            get 
            {
                return base.GetProperty("OptionStrict").Equals("On", StringComparison.OrdinalIgnoreCase);
            }
            set
            {
                base.SetProperty("OptionStrict", value ? "On" : "Off");
            }
        }

        /// <summary>
        /// Determines whether option explicit is turned on.
        /// </summary>
        public bool OptionExplicit
        {
            get 
            {
                return base.GetProperty("OptionExplicit").Equals("On", StringComparison.OrdinalIgnoreCase);
            }
            set
            {
                base.SetProperty("OptionExplicit", value ? "On" : "Off");
            }
        }
    }
}