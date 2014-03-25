using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages.Net
{
    public abstract class NetSourceSnapshot : SourceSnapshot
    {
        public NetSourceSnapshot(string source)
            : base(source)
        {
        }

        public abstract NetSnapshotMember[] Namespaces { get; protected set; }
        public abstract NetSnapshotMember[] UsingNamespaces { get; protected set; }
        public abstract NetSnapshotMember[] Types { get; protected set; }
        public abstract NetSnapshotMember[] Fields { get; protected set; }
        public abstract NetSnapshotMember[] Methods { get; protected set; }
        public abstract NetSnapshotMember[] Properties { get; protected set; }
        public abstract NetSnapshotMember[] Events { get; protected set; }

        public Type GetTypeByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            foreach (var keyPair in (Language as NetLanguageDescriptor).TypeAliases)
            {
                if (string.Compare(keyPair.NewName, name, !Language.CaseSensitive) == 0)
                {
                    return Type.GetType(keyPair.OriginalType);
                }
            }

            foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = null;

                for (int i = 0; i < UsingNamespaces.Length; i++)
                {
                    type = asm.GetType(string.Format("{0}.{1}", UsingNamespaces[i].Name, name), false, !Language.CaseSensitive);
                    if (type != null)
                        break;
                }

                if (type == null)
                    type = asm.GetType(name, false, !Language.CaseSensitive);

                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
