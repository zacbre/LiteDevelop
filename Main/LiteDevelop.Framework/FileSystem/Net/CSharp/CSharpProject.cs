using System;
using System.Linq;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.FileSystem.Net.CSharp
{
    /// <summary>
    /// Represents a C# project which can be built using the C# compiler.
    /// </summary>
    public class CSharpProject : NetProject
    {
        private static CSharpProjectDescriptor _descriptor = ProjectDescriptor.GetDescriptor<CSharpProjectDescriptor>();

    	public CSharpProject(string name)
    		: base(name, _descriptor)
    	{
    	}

    	public CSharpProject(FilePath filePath)
    		: base(filePath)
    	{
    	}

        /// <inheritdoc />
        public override LanguageDescriptor Language
        {
            get { return LanguageDescriptor.GetLanguage<CSharpLanguage>(); }
        }

        /// <inheritdoc />
        public override ProjectDescriptor ProjectDescriptor
        {
            get { return _descriptor; }
        }

		/// <summary>
		/// Determines whether the project is supported to have unsafe code blocks.
		/// </summary>
    	public bool AllowUnsafeCode
    	{
    		get 
    		{
    			bool returnValue;
    			if (bool.TryParse(base.GetProperty("AllowUnsafeBlocks"), out returnValue))
    				return returnValue; 
    			return false;
    		}
    		set
    		{
    			base.SetProperty("AllowUnsafeBlocks", value.ToString().ToLower());
    		}
    	}
    }
}