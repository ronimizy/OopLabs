using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.AccountPlans;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.AccountPlans
{
    public class DebitAccountPlanView : View
    {
        private readonly DebitAccountPlanViewModel _viewModel;

        public DebitAccountPlanView(DebitAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.PlanInfo.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
            => new Component[]
            {
                new MarkupComponent(new Markup(_viewModel.PlanInfo.Description)),
                new NavigationComponent(_viewModel.Navigator, _viewModel.UpdatePercentageElement),
            };
    }
}