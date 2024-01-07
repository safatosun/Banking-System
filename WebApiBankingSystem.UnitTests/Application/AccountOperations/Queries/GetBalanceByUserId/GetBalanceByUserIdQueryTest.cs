using Application.AccountOperations.Queries.GetBalanceByUserId;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBankingSystem.UnitTests.Application.AccountOperations.Queries.GetBalanceByUserId
{
    public class GetBalanceByUserIdQueryTest
    {
        [Fact]
        public async Task WhenValidInputAreGiven_Account_ShouldBeReturn()
        {
            var userId = "1234";
            var accountName = "Investment";

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();

            var account = new Account
            {
                UserId = userId,
                Name = accountName,
                Balance = 100
            };

            mockUnitOfWork.Setup(uow => uow.Account.GetByUserIdAndAccountNameWithUserDetailsAsync(userId, accountName, true))
                .ReturnsAsync(account);

            var query = new GetBalanceByUserIdQuery(mockMapper.Object, mockUnitOfWork.Object)
            {
                UserId = userId,
                AccountName = accountName
            };

            // Act
            var result = await query.Handle();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(account.Balance, result.Balance);
        }

        [Fact]
        public async Task WhenInvalidInputAreGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var userId = "1234";
            var accountName = "Investment";

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
           
            mockUnitOfWork.Setup(uow => uow.Account.GetByUserIdAndAccountNameWithUserDetailsAsync(userId, accountName, true))
                .ReturnsAsync((Account)null); 

            var query = new GetBalanceByUserIdQuery(mockMapper.Object, mockUnitOfWork.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => query.Handle());
        }
    }
}
