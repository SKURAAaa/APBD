using System;
using LegacyApp;
using NUnit.Framework;

namespace TestProject2;

public class Tests
{
    [Test]
    public void AddUser_ValidData_ReturnsTrue()
    {
        // Arrange
        var userService = new UserService();
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var clientId = 1;

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.IsTrue(result, "Should return true for valid user data.");
    }

    [Test]
    public void AddUser_InvalidData_ReturnsFalse()
    {
        // Arrange
        var userService = new UserService();
        var firstName = ""; // Pusty pierwszy imię
        var lastName = "Doe";
        var email = "invalid_email"; // Nieprawidłowy format adresu e-mail
        var dateOfBirth = new DateTime(2010, 1, 1); // Niepełnoletni
        var clientId = 1;

        // Act
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);

        // Assert
        Assert.IsFalse(result, "Should return false for invalid user data.");
    }
}