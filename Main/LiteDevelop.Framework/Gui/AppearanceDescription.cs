using System;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Gui
{
    public sealed class AppearanceDescription : IXmlSerializable
    {
        public event EventHandler ForeColorChanged;
        public event EventHandler BackColorChanged;
        public event EventHandler FontStyleChanged;

        private KnownColor? _foreColorSystem, _backColorSystem;
        private Color _foreColor, _backColor;
        private FontStyle _fontStyle;

        public AppearanceDescription()
        {

        }

        public AppearanceDescription(string name, Color foreColor, Color backColor, FontStyle fontStyle)
        {
            Text = name;
            ForeColor = foreColor;
            BackColor = backColor;
            FontStyle = fontStyle;
        }

        public string ID
        {
            get;
            private set;
        }

        public string Text
        {
            get;
            private set;
        }

        public KnownColor? ForeColorSystem
        {
            get { return _foreColorSystem; }
            set
            {
                if (_foreColorSystem != value)
                {
                    if (value.HasValue)
                        ForeColor = Color.FromKnownColor(value.Value);

                    _foreColorSystem = value;
                }
            }
        }

        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (_foreColor != value)
                {
                    _foreColor = value;
                    if (ForeColorChanged != null)
                        ForeColorChanged(this, EventArgs.Empty);
                }
            }
        }

        public KnownColor? BackColorSystem
        {
            get { return _backColorSystem; }
            set
            {
                if (_backColorSystem != value)
                {
                    if (value.HasValue)
                        BackColor = Color.FromKnownColor(value.Value);

                    _backColorSystem = value;
                }
            }
        }

        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    if (BackColorChanged != null)
                        BackColorChanged(this, EventArgs.Empty);
                }
            }
        }

        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set
            {
                if (_fontStyle != value)
                {
                    _fontStyle = value;
                    if (FontStyleChanged != null)
                        FontStyleChanged(this, EventArgs.Empty);
                }
            }
        }

        public void CopyTo(AppearanceDescription destination)
        {
            destination.ID = ID;
            destination.Text = Text;
            destination.BackColorSystem = BackColorSystem;
            destination.BackColor = BackColor;
            destination.ForeColorSystem = ForeColorSystem;
            destination.ForeColor = ForeColor;
            destination.FontStyle = FontStyle;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            this.Text = reader.GetAttribute("Name");
            this.ID = reader.GetAttribute("Id");

            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (!isEmpty)
            {
                KnownColor? knownColor;
                Color color;

                ReadColorProperty(reader, out knownColor, out color);

                if (knownColor.HasValue)
                    ForeColor = Color.FromKnownColor(knownColor.Value);
                else
                    ForeColor = color;

                ReadColorProperty(reader, out knownColor, out color);

                if (knownColor.HasValue)
                    BackColor = Color.FromKnownColor(knownColor.Value);
                else
                    BackColor = color;

                FontStyle = ReadFontStyleProperty(reader);
                
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Name", this.Text);
            writer.WriteAttributeString("Id", this.ID);

            writer.WriteStartElement("ForeColor");
            WriteColorProperty(writer, ForeColorSystem, ForeColor);
            writer.WriteEndElement();

            writer.WriteStartElement("BackColor");
            WriteColorProperty(writer, BackColorSystem, BackColor);
            writer.WriteEndElement();

            writer.WriteStartElement("FontStyle");
            WriteFontStyleProperty(writer, FontStyle);
            writer.WriteEndElement();
        }

        #endregion

        private static void ReadColorProperty(System.Xml.XmlReader reader, out KnownColor? knownColor, out Color color)
        {
            knownColor = null;
            color = default(Color);

            bool useSystemName = bool.Parse(reader.GetAttribute("UseSystemName"));

            string rawValue = reader.ReadElementString();

            if (useSystemName)
                knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), rawValue);
            else
                color = Color.FromArgb(int.Parse(rawValue, System.Globalization.NumberStyles.HexNumber ));

        }

        private static void WriteColorProperty(System.Xml.XmlWriter writer, KnownColor? knownColor, Color color)
        {
            writer.WriteAttributeString("UseSystemName", knownColor.HasValue.ToString());
            writer.WriteString(knownColor.HasValue ? knownColor.Value.ToString() : color.ToArgb().ToString("x6"));
        }

        private static FontStyle ReadFontStyleProperty(System.Xml.XmlReader reader)
        {
            return (FontStyle)Enum.Parse(typeof(FontStyle), reader.ReadElementString());
        }

        private static void WriteFontStyleProperty(System.Xml.XmlWriter writer, FontStyle font)
        {
            writer.WriteString(font.ToString());
        }
    }
}
