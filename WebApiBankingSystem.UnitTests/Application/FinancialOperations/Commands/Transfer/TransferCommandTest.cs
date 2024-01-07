using Application.Common.Interfaces;
using Application.FinancialOperations.Commands.Transfer.Commands;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBankingSystem.UnitTests.Application.FinancialOperations.Commands.Transfer
{
    public class TransferCommandTest
    {
        [Fact]
        public async Task WhenConditionsAreMet_Transfer_ShouldBeSuccessful()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var transferCommand = new TransferCommand(unitOfWorkMock.Object, loggerServiceMock.Object);
            transferCommand.ModelDto = new TransferDto { SenderAccountId = 1, ReceiverAccountId = 2, Amount = 50 };

            var senderAccount = new Account { Balance = 100, TransferLimit = 9 };
            var receiverAccount = new Account { Balance = 80 };

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(1, true))
                          .ReturnsAsync(senderAccount);

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(2, true))
                          .ReturnsAsync(receiverAccount);

            // Act
            await transferCommand.Handle();

            // Assert

            Assert.Equal(50, senderAccount.Balance); 
            Assert.Equal(130, receiverAccount.Balance); 
        }
        
        [Fact]
        public async Task WhenTransfertIsSuccessful_Handle_ShouldCallTransactionMethods()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var transferCommand = new TransferCommand(unitOfWorkMock.Object, loggerServiceMock.Object);
            transferCommand.ModelDto = new TransferDto { SenderAccountId = 1, ReceiverAccountId = 2, Amount = 50 };

            var senderAccount = new Account { Balance = 100, TransferLimit = 9 };
            var receiverAccount = new Account { Balance = 80 };

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(1, true))
                          .ReturnsAsync(senderAccount);

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(2, true))
                          .ReturnsAsync(receiverAccount);

            // Act
            await transferCommand.Handle();

            // Assert
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.TransactionCommitAsync(), Times.Once);

        }

        [Fact]
        public async Task WhenSenderAccountIsNull_Handle_ThrowsInvalidOperationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var transferCommand = new TransferCommand(unitOfWorkMock.Object, loggerServiceMock.Object);
            transferCommand.ModelDto = new TransferDto { SenderAccountId = 1, ReceiverAccountId = 2, Amount = 50 };

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(1, true))
                          .ReturnsAsync((Account)null); 

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await transferCommand.Handle());
        }

        [Fact]
        public async Task WhenBalanceIsLessThanAmount_Handle_ThrowsInvalidOperationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var transferCommand = new TransferCommand(unitOfWorkMock.Object, loggerServiceMock.Object);
            transferCommand.ModelDto = new TransferDto { SenderAccountId = 1, ReceiverAccountId = 2, Amount = 100 };

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(It.IsAny<int>(), true))
                          .ReturnsAsync(new Account { Balance = 50 }); 

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await transferCommand.Handle());
        }

        [Fact]
        public async Task WhenTransferLimitExceeded_Handle_ThrowsInvalidOperationException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerServiceMock = new Mock<ILoggerService>();

            var transferCommand = new TransferCommand(unitOfWorkMock.Object, loggerServiceMock.Object);
            transferCommand.ModelDto = new TransferDto { SenderAccountId = 1, ReceiverAccountId = 2, Amount = 50 };

            unitOfWorkMock.Setup(u => u.Account.GetOneByIdAsync(It.IsAny<int>(), true))
                          .ReturnsAsync(new Account { Balance = 100, TransferLimit = 11 }); 

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await transferCommand.Handle());
        }

    }
}
