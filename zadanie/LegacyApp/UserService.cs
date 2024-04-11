using System;

namespace LegacyApp
{
    public class UserService
    {
        // Metoda dodająca użytkownika
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            // Sprawdzenie poprawności danych użytkownika
            if (!IsValidUser(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            // Pobranie informacji o kliencie
            var client = GetClient(clientId);
            if (client == null)
            {
                return false;
            }

            // Utworzenie nowego użytkownika
            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            // Ustawienie limitu kredytowego dla użytkownika
            SetUserCredit(user);

            // Sprawdzenie czy limit kredytowy jest wystarczający
            if (!IsUserCreditLimitValid(user))
            {
                return false;
            }

            // Dodanie użytkownika do bazy danych
            UserDataAccess.AddUser(user);
            return true;
        }

        // Metoda sprawdzająca poprawność danych użytkownika
        private bool IsValidUser(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return !string.IsNullOrEmpty(firstName) &&
                   !string.IsNullOrEmpty(lastName) &&
                   email.Contains("@") &&
                   email.Contains(".") &&
                   CalculateAge(dateOfBirth) >= 21;
        }

        // Metoda obliczająca wiek na podstawie daty urodzenia
        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }
            return age;
        }

        // Metoda pobierająca informacje o kliencie
        private Client GetClient(int clientId)
        {
            var clientRepository = new ClientRepository();
            return clientRepository.GetById(clientId);
        }

        // Metoda tworząca nowego użytkownika
        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return user;
        }

        // Metoda ustawiająca limit kredytowy dla użytkownika
        private void SetUserCredit(User user)
        {
            using (var userCreditService = new UserCreditService())
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                if (Object.Equals("ImportantClient", StringComparison.OrdinalIgnoreCase))
                {
                    creditLimit *= 2;
                }
                user.CreditLimit = creditLimit;
                user.HasCreditLimit = !Object.Equals("VeryImportantClient", StringComparison.OrdinalIgnoreCase);
            }
        }

        // Metoda sprawdzająca czy limit kredytowy użytkownika jest wystarczający
        private bool IsUserCreditLimitValid(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }
    }
}
