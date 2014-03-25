using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;

namespace LiteDevelop.Essentials.FormsDesigner.Services
{
    public class NameCreationService : INameCreationService
    {
        public string CreateName(IContainer container, Type dataType)
        {
            // generate base name by getting type name and make first char to lower.
            string baseName = char.ToLower(dataType.Name[0]) + dataType.Name.Substring(1);

            // add number in order to prevent variables with same name.
            int counter = 1;
            while (NameExists(container, baseName + counter.ToString()))
                counter++;

            // return variable name.
            return baseName + counter.ToString();
        }

        public bool IsValidName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char ch = name[i];
                var uc = Char.GetUnicodeCategory(ch);
                switch (uc)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        public void ValidateName(string name)
        {
            if (!IsValidName(name))
            {
                throw new ArgumentException(string.Format("The name {0} is not a valid identifier.", name));
            }
        }

        private bool NameExists(IContainer container, string name)
        {
            // iterate through existing components, and check their name.
            foreach (Component component in container.Components)
            {
                if (component.Site.Name == name)
                    return true;
            }
            return false;
        }
    }
}
