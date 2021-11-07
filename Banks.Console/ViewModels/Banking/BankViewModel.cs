using System;
using System.Linq;
using Banks.Builders.DepositAccountPlanBuilder;
using Banks.Console.ViewModels.Banking.AccountPlans;
using Banks.Console.ViewModels.Banking.Accounts;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Banks.Console.Views.Banking;
using Banks.Console.Views.Banking.AccountPlans;
using Banks.Entities;
using Banks.Plans;
using Banks.Tools;
using Spectre.Console;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class BankViewModel
    {
        private readonly CentralBank _centralBank;

        private readonly Bank _bank;
        private readonly Client _client;

        public BankViewModel(CentralBank centralBank, Bank bank, Client client, INavigator navigator)
        {
            _bank = bank.ThrowIfNull(nameof(bank));
            _client = client.ThrowIfNull(nameof(client));
            Navigator = navigator.ThrowIfNull(nameof(navigator));
            _centralBank = centralBank.ThrowIfNull(nameof(centralBank));
        }

        public INavigator Navigator { get; }
        public string BankName => _bank.Name;
        public bool ClientIsOwner => _bank.Owner.Equals(_client);

        public AccountsViewModel AccountsViewModel => new AccountsViewModel(_client, _bank, Navigator);

        public SelectorViewModel<DebitAccountPlan> EnrollDebitAccountSelectorViewModel
            => new (Navigator, _bank.DebitAccountPlans, "Enroll Debit Account", EnrollDebitAccountAction);

        public SelectorViewModel<CreditAccountPlan> EnrollCreditAccountSelectorViewModel
            => new (Navigator, _bank.CreditAccountPlans, "Enroll Credit Account", EnrollCreditAccountAction);

        public SelectorViewModel<DepositAccountPlan> EnrollDepositAccountSelectorViewModel
            => new (Navigator, _bank.DepositAccountPlans, "Enroll Deposit Account", EnrollDepositAccountAction);

        public RegisterDebitAccountPlanViewModel RegisterDebitAccountPlanViewModel => new (RegisterDebitAccountPlan);
        public RegisterDepositAccountPlanViewModel RegisterDepositAccountPlanViewModel => new (Navigator, RegisterDepositAccountPlan);
        public RegisterCreditAccountPlanViewModel RegisterCreditAccountPlanViewModel => new (RegisterCreditAccountPlan);

        public SelectorViewModel<DebitAccountPlan> EditDebitAccountPlanViewModel
            => new (Navigator, _bank.DebitAccountPlans, "Edit Debit Account Plan", EditDebitAccountPlan);

        public SelectorViewModel<DepositAccountPlan> EditDepositAccountPlanViewModel
            => new (Navigator, _bank.DepositAccountPlans, "Edit Deposit Account Plan", EditDepositAccountPlan);

        public SelectorViewModel<CreditAccountPlan> EditCreditAccountPlanViewModel
            => new (Navigator, _bank.CreditAccountPlans, "Edit Credit Account Plan", EditCreditAccountPlan);

        private void RegisterDebitAccountPlan(RegisterDebitAccountPlanViewModel viewModel)
        {
            IBuilder<DebitAccountPlan> builder = DebitAccountPlan.BuildPlan
                .WithDebitPercentage(viewModel.Percentage);
            _bank.RegisterDebitAccountPlan(_client, builder);
            Navigator.PopView();
        }

        private void RegisterDepositAccountPlan(RegisterDepositAccountPlanViewModel viewModel)
        {
            IDepositPercentageLevelSelector levelSelector = DepositAccountPlan.BuildPlan;
            viewModel.Levels.ToList().ForEach(l => levelSelector = levelSelector.WithLevel(l));

            _bank.RegisterDepositAccountPlan(_client, levelSelector.Builder);
            Navigator.PopView();
        }

        private void RegisterCreditAccountPlan(RegisterCreditAccountPlanViewModel viewModel)
        {
            IBuilder<CreditAccountPlan> builder = CreditAccountPlan.BuildPlan
                .LimitedTo(viewModel.Limit)
                .WithCommissionPercent(viewModel.Percent);
            _bank.RegisterCreditAccountPlan(_client, builder);
            Navigator.PopView();
        }

        private NavigationElement EnrollDebitAccountAction(DebitAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                _bank.EnrollDebitAccount(_client, plan);
                n.PopView();
            });
        }

        private NavigationElement EnrollCreditAccountAction(CreditAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                _bank.EnrollCreditAccount(_client, plan);
                n.PopView();
            });
        }

        private NavigationElement EnrollDepositAccountAction(DepositAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                var enrollDepositAccountViewModel = new EnrollDepositAccountViewModel(_centralBank.Chronometer, plan);
                enrollDepositAccountViewModel.EnteringCompleted += EnrollDepositAccount;

                n.PushView(new EnrollDepositAccountView(enrollDepositAccountViewModel));
            });
        }

        private void EnrollDepositAccount(EnrollDepositAccountViewModel viewModel)
        {
            _bank.EnrollDepositAccount(
                _client,
                viewModel.Plan,
                new DateTime(viewModel.UnlockYear, viewModel.UnlockMonth, viewModel.UnlockDay),
                viewModel.Deposit);
            Navigator.PopView();
            Navigator.PopView();
        }

        private NavigationElement EditDebitAccountPlan(DebitAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                var viewModel = new DebitAccountPlanViewModel(_client, _bank, plan, n);
                n.PushView(new DebitAccountPlanView(viewModel));
            });
        }

        private NavigationElement EditDepositAccountPlan(DepositAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                var viewModel = new DepositAccountPlanViewModel(_client, _bank, plan, n);
                n.PushView(new DepositAccountPlanView(viewModel));
            });
        }

        private NavigationElement EditCreditAccountPlan(CreditAccountPlan plan)
        {
            return new NavigationElement(plan.ToString().EscapeMarkup(), n =>
            {
                var viewModel = new CreditAccountPlanViewModel(_client, _bank, plan, n);
                n.PushView(new CreditAccountPlanView(viewModel));
            });
        }
    }
}