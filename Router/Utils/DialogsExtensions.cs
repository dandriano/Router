using Prism.Services.Dialogs;
using Router.Model;
using Router.Views;

namespace Router.Utils
{
    public static class DialogsExtensions
    {
        public const string NodeKey = "node";

        public static Node GetNewNode(this IDialogService d, string nodeName)
        {
            var node = new Node(nodeName);
            var param = new DialogParameters
            {
                { NodeKey, node }
            };
            d.Show(nameof(NodeView), param, (r) => { }); ;

            return node;
        }
    }
}
