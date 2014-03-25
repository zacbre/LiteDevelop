using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    public class SolutionFolder : SolutionNode, IDisposable
    {
        public static readonly Guid SolutionFolderGuid = new Guid("{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
        private bool _isLoaded;

        public SolutionFolder()
        {
            Nodes = new EventBasedCollection<SolutionNode>();
            Nodes.InsertedItem += new CollectionChangeEventHandler(Nodes_InsertedItem);
            Nodes.RemovedItem += new CollectionChangeEventHandler(Nodes_RemovedItem);
            TypeGuid = SolutionFolderGuid;
        }

        /// <summary>
        /// Gets a collection of sub nodes thi solution folder has.
        /// </summary>
        public EventBasedCollection<SolutionNode> Nodes 
        { 
            get;
            private set; 
        }

        /// <inheritdoc />
        public override bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <inheritdoc />
        public override void Load(IProgressReporter reporter)
        {
            try
            {
                foreach (var node in Nodes)
                    node.Load(reporter);
                _isLoaded = true;
                OnLoadComplete(new SolutionNodeLoadEventArgs());
            }
            catch (Exception ex)
            {
                _isLoaded = true;
                OnLoadComplete(new SolutionNodeLoadEventArgs(ex));
            }
        }

        /// <inheritdoc />
        public override void Save(IProgressReporter reporter)
        {
            foreach (var node in Nodes)
                node.Save(reporter);
        }

        private void Nodes_InsertedItem(object sender, CollectionChangedEventArgs e)
        {
            (e.TargetObject as SolutionNode).Parent = this;
        }

        private void Nodes_RemovedItem(object sender, CollectionChangedEventArgs e)
        {
            var node = e.TargetObject as SolutionNode;
            if (node.Parent == this)
                node.Parent = null;
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            foreach (var subnode in Nodes)
                if (subnode is IDisposable)
                    (subnode as IDisposable).Dispose();
        }
    }
}
