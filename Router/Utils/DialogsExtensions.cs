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
            var pos = Mouse.GetPosition(null);
            var param = new DialogParameters
            {
                { NodeKey, node },
                { TopKey, pos.Y },
                { LeftKey, pos.X }
            };
            d.Show(nameof(NodeView), param, (r) => { }); ;

            return node;
        }
    }
}
