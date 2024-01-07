using Application.AccountOperations.Commands.CreateAccount;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiBankingSystem.UnitTests.Application.AccountOperations.Commands.CreateAccount
{
    public class CreateAccountCommandTest 
    {
        
        [Fact]
        public async Task WhenValidInputAreGiven_Account_ShouldBeCreated()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            var createAccountDto = new CreateAccountDto { UserId = "123", Name = "Investment", Balance = 100 };
            
            var user = new User { Id = "123" };
            
            var command = new CreateAccountCommand(mapperMock.Object, userManagerMock.Object, unitOfWorkMock.Object)
            {
                ModelDto = createAccountDto
            };

            userManagerMock.Setup(u => u.FindByIdAsync("123"))
                           .ReturnsAsync(user);

            mapperMock.Setup(m => m.Map<Account>(It.IsAny<CreateAccountDto>()))
                        .Returns((CreateAccountDto source) => new Account
                        {
                            UserId = source.UserId,
                            Name = source.Name,
                            Balance = source.Balance,
                            TransferLimit = 0
                        });

            unitOfWorkMock.Setup(uow => uow.Account).Returns(accountRepositoryMock.Object);

            // Act
            await command.Handle();

            // Assert
            accountRepositoryMock.Verify(repo => repo.CreateOne(It.IsAny<Account>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        public async Task WhenInvalidUserIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var mapperMock = new Mock<IMapper>();
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            var createAccountDto = new CreateAccountDto { UserId = "123", Name = "TestAccount", Balance = 100 };
            var command = new CreateAccountCommand(mapperMock.Object, userManagerMock.Object, unitOfWorkMock.Object)
            {
                ModelDto = createAccountDto
            };

            userManagerMock.Setup(u => u.FindByIdAsync("123"))
                           .ReturnsAsync((User)null);

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => command.Handle());
        }

    }
}
