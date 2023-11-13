using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Router.Model;
using Router.Utils;
using System;

namespace Router.ViewModels
{
    public class NodeViewModel : BindableBase, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;
        public DelegateCommand Confirm { get; private set; }
        public string Title => "New Node";

        private Node _node;
        public Node Node
        {
            get => _node;
            set => SetProperty(ref _node, value);
        }

        private double _top;
        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        private double _left;
        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        public NodeViewModel()
        {
            Confirm = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(null);
            });
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (!parameters.TryGetValue(DialogsExtensions.NodeKey, out Node node))
            {
                throw new ArgumentException(DialogsExtensions.NodeKey);
            };

            if (!parameters.TryGetValue(DialogsExtensions.TopKey, out double top))
            {
                throw new ArgumentException(DialogsExtensions.NodeKey);
            };

            if (!parameters.TryGetValue(DialogsExtensions.LeftKey, out double left))
            {
                throw new ArgumentException(DialogsExtensions.NodeKey);
            };

            Top = top;
            Left = left;
            Node = node;
        }
    }
}
