using Prism.Commands;
using Prism.Mvvm;
using Router.Interfaces;

namespace Router.ViewModels
{
    public class GraphViewModel : BindableBase
    {
        public IGraphController GraphController { get; }
        public DelegateCommand InitializeController { get; }

        public GraphViewModel(IGraphController graphController)
        {
            GraphController = graphController;
            InitializeController = new DelegateCommand(GraphController.Initialize);
        }
    }
}
