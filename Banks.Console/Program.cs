using System;
using Banks.Console.Tools;
using Banks.Console.ViewModels;
using Banks.Console.Views;
using Banks.Entities;
using Banks.Services;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;
using Spectre.Mvvm;
using Spectre.Mvvm.Views;

var chronometer = new SettableChronometer()
{
    CurrentDateTime = DateTime.UtcNow,
};

DbContextOptions mailingContextOptions = new DbContextOptionsBuilder()
    .UseSqlite("Filename=Mailing.db")
    .Options;
DbContextOptions banksContextOptions = new DbContextOptionsBuilder()
    .UseSqlite("Filename=Banks.db")
    .Options;

var mailingContext = new MailingDatabaseContext(mailingContextOptions);
var mailingService = new MailingService(mailingContext);

var banksContext = new BanksDatabaseContext(banksContextOptions, chronometer, mailingService);
var centralBank = new CentralBank(banksContext);

banksContext.Load();

var navigationView = new NavigationView(n => new MenuView(new MenuViewModel(centralBank, mailingService, n, chronometer)));
var window = new Window(navigationView);

window.Run(20);