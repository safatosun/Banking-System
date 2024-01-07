using Application.Common.Interfaces;
using Application.FinancialOperations.Commands.Deposit;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBankingSystem.UnitTests.Application.FinancialOperations.Commands.Deposit
{
    public class DepositCommandTest
    {

        [Fact]
        public async Task WhenValidInputAreGiven_Deposit_ShouldBeSuccessful()
        {
            //Arrange
            var dbContextMock = new Mock<IBankingSystemDbContext>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var depositDto = new DepositDto
            {
                AccountName = "Invesment",
                Amount = 100
            };

            var usersAccount = new Account
            {
                UserId = "123",
                Name = "Invesment",
                Balance = 500
               
            };

            unitOfWorkMock.Setup(u => u.Account.GetByUserIdAndAccountNameWithUserDetailsAsync("123", "Invesment", true))
                .ReturnsAsync(usersAccount);

            var depositCommand = new DepositCommand(dbContextMock.Object, unitOfWorkMock.Object, loggerServiceMock.Object)
            {
                UserId = "123",
                ModelDto = depositDto
            };

            // Act
            await depositCommand.Handle();

            // Assert
            Assert.Equal(600, usersAccount.Balance);
        }

        [Fact]
        public async Task WhenInValidInputAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var dbContextMock = new Mock<IBankingSystemDbContext>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            unitOfWorkMock.Setup(u => u.Account.GetByUserIdAndAccountNameWithUserDetailsAsync("123", "null", true))
                .ReturnsAsync((Account)null);

            var depositCommand = new DepositCommand(dbContextMock.Object, unitOfWorkMock.Object, loggerServiceMock.Object)
            {
                UserId = "123",
                ModelDto = new DepositDto { AccountName = "null", Amount = 100.0m }
            };

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => depositCommand.Handle());
        
        }

        [Fact]
        public async Task WhenDepositIsSuccessful_Handle_ShouldCallTransactionMethods()
        {
            // Arrange
            var dbContextMock = new Mock<IBankingSystemDbContext>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var depositDto = new DepositDto
            {
                AccountName = "Invesment",
                Amount = 100
            };

            var usersAccount = new Account
            {
                UserId = "123",
                Name = "Invesment",
                Balance = 500
              
            };

            unitOfWorkMock.Setup(u => u.Account.GetByUserIdAndAccountNameWithUserDetailsAsync("123", "Invesment", true))
                .ReturnsAsync(usersAccount);

            var depositCommand = new DepositCommand(dbContextMock.Object, unitOfWorkMock.Object, loggerServiceMock.Object)
            {
                UserId = "123",
                ModelDto = depositDto
            };

            // Act
            await depositCommand.Handle();

            // Assert
            unitOfWorkMock.Verify(u => u.BeginTransactionAsyncWithPessimisticLocking(), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.TransactionCommitAsync(), Times.Once);

        }

        [Fact]
        public async Task WhenDepositIsSuccessful_Handle_ShouldCallLogInfo()
        {
            // Arrange
            var dbContextMock = new Mock<IBankingSystemDbContext>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var depositDto = new DepositDto
            {
                AccountName = "Invesment",
                Amount = 100
            };

            var usersAccount = new Account
            {
                UserId = "123",
                Name = "Invesment",
                Balance = 500

            };

            unitOfWorkMock.Setup(u => u.Account.GetByUserIdAndAccountNameWithUserDetailsAsync("123", "Invesment", true))
                .ReturnsAsync(usersAccount);

            var depositCommand = new DepositCommand(dbContextMock.Object, unitOfWorkMock.Object, loggerServiceMock.Object)
            {
                UserId = "123",
                ModelDto = depositDto
            };

            // Act
            await depositCommand.Handle();

            // Assert
            unitOfWorkMock.Verify(u => u.BeginTransactionAsyncWithPessimisticLocking(), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.TransactionCommitAsync(), Times.Once);

            loggerServiceMock.Verify(l =>
            l.LogInfo(It.Is<string>(message => message.Contains("The user with ID number 123 deposit 100 USD Invesment account"))), Times.Once);
        }


    }
}
