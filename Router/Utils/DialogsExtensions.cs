using Prism.Ioc;
using Prism.Services.Dialogs;
using Router.Model;
using Router.Views;
using System.Windows.Input;

namespace Router.Utils
{
    public static class DialogsExtensions
    {
        public const string NodeKey = "node";
        public const string TopKey = "top";
        public const string LeftKey = "left";

        public static Node GetNewNode(this IDialogService d, string nodeName)
        {
            var node = new Node(nodeName);

            // both approaches (getting relative or ablolute position) lead to the same result
            //var window = ContainerLocator.Current.Resolve<MainWindow>();

            var point = Mouse.GetPosition(null/*window*/);
            double top = /*window.Top + */point.Y;
            double left = /*window.Left + */point.X;

            var param = new DialogParameters
            {
                { NodeKey, node },
                { TopKey, top },
                { LeftKey, left }
            };
            d.Show(nameof(NodeView), param, (r) => { }); ;

            return node;
        }
    }
}
