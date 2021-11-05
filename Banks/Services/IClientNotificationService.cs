using Banks.Entities;
using Banks.Models;

namespace Banks.Services
{
    public interface IClientNotificationService
    {
        void Notify(Bank bank, Client client, Message message);
    }
}