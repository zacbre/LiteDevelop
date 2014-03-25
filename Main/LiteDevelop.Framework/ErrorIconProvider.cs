using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework
{
    public class ErrorIconProvider : IconProvider 
    {
        private ImageList _imageList;

        /// <inheritdoc />
        public override ImageList ImageList
        {
            get 
            {
                if (_imageList == null)
                {
                    _imageList = new ImageList()
                    {
                        ColorDepth = ColorDepth.Depth32Bit,
                        ImageSize = new Size(16,16),
                    };
                    _imageList.Images.AddRange(new Image[]
                    {
                        Properties.Resources.error,
                        Properties.Resources.info,
                        Properties.Resources.warning,
                        Properties.Resources.correct,
                    });
                }

                return _imageList;
            }
        }

        /// <inheritdoc />
        public override int GetImageIndex(object member)
        {
            if (member is MessageSeverity)
            {
                var severity = (MessageSeverity)member;
                switch (severity)
                {
                    case MessageSeverity.Error:
                        return 0;
                    case MessageSeverity.Message:
                        return 1;
                    case MessageSeverity.Warning:
                        return 2;
                    case MessageSeverity.Success:
                        return 3;
                }
            }
            throw new NotSupportedException();
        }
    }
}
