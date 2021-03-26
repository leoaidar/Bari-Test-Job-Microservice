using System;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using Bari.Test.Job.Domain..Commands.Contracts;

namespace Wooza.Gateway.Tim.Domain.Commands
{
    public class CreateContactCommand : Notifiable, ICommand,
                                            IRequest<ICommandResult>
    {
        public CreateContactCommand() { }

        public CreateContactCommand(string name, string gender, DateTime birth)
        {
            Name = name;
            Gender = gender;
            Birth = birth;
        }

        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Birth { get; set; }


        public void Validate()
        {
            DateTime dateBirth;

            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(Name, 5, "Name", "Por favor, digite o nome do contato!")
                    .HasMinLen(Gender, 4, "Gender", "Por favor, digite o genero!")
                    .IsTrue(DateTime.TryParse(Birth.ToString(), out dateBirth), "Birth", "Por favor, digite uma data de nascimento válida!")
                    .IsFalse((Birth.Year > DateTime.Now.Year), "Birth", "Por favor, data de nascimento futura digite um data válida!")
            );            
        }
    }
}