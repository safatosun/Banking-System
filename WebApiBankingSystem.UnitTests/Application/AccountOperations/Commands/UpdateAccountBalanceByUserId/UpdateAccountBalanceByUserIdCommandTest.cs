using Application.AccountOperations.Commands.UpdateAccountBalance;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBankingSystem.UnitTests.Application.AccountOperations.Commands.UpdateAccountBalanceByUserId
{
    public class UpdateAccountBalanceByUserIdCommandTest
    {
        [Fact]
        public async Task Handle_ValidAccount_ShouldUpdateBalance()
        {
            // Arrange
            var userId = "123";
            var accountName = "Investment";
            var balance = 100;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();

            unitOfWorkMock.Setup(uow => uow.Account).Returns(accountRepositoryMock.Object);

            var mapperMock = new Mock<IMapper>();

            var command = new UpdateAccountBalanceByUserIdCommand(mapperMock.Object, unitOfWorkMock.Object)
            {
                UserId = userId,
                ModelDto = new UpdateAccountBalanceDto { AccountName = accountName, Balance = balance }
            };

            var existingAccount = new Account 
            {
                UserId = userId,
                Name = accountName,
                Balance = 500                          
            };

            accountRepositoryMock.Setup(repo => repo.GetByUserIdAndAccountNameWithUserDetailsAsync(userId, accountName, true))
                .ReturnsAsync(existingAccount);

            // Act
            await command.Handle();

            // Assert
            Assert.Equal(balance, existingAccount.Balance);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ShouldThrowException()
        {
            // Arrange
            var userId = "123";
            var accountName = "Investment";
            var balance = 100;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();

            unitOfWorkMock.Setup(uow => uow.Account).Returns(accountRepositoryMock.Object);

            var mapperMock = new Mock<IMapper>();

            var command = new UpdateAccountBalanceByUserIdCommand(mapperMock.Object, unitOfWorkMock.Object)
            {
                UserId = userId,
                ModelDto = new UpdateAccountBalanceDto { AccountName = accountName, Balance = balance }
            };

            accountRepositoryMock.Setup(repo => repo.GetByUserIdAndAccountNameWithUserDetailsAsync(userId, accountName, true))
                .ReturnsAsync((Account)null);

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => command.Handle());
        }

    }

}
