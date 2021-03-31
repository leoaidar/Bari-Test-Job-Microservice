using Bari.Test.Job.Domain.Commands.Contracts;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;

namespace Bari.Test.Job.Domain.Commands
{
    public class SendMessageCommand : Notifiable, ICommand,
                                            IRequest<ICommandResult>
    {
        public SendMessageCommand() { }

        public SendMessageCommand(string body, string serviceId)
        {
            Body = body;
            ServiceId = serviceId;
        }


        public string Body { get; set; }
        public string ServiceId { get; set; }


        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Body, 1, "Body", "Por favor, digite o conteudo da mensagem!")
                    .HasMinLen(ServiceId, 1, "ServiceId", "Por favor, digite o identificador do serviço!")
            );            
        }
    }
}