using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Features.Employees.Commands;
using ERP.HRM.Application.Features.Employees.Handlers;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class DeleteEmployeeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DeleteEmployeeCommandHandler>> _mockLogger;
        private readonly DeleteEmployeeCommandHandler _handler;

        public DeleteEmployeeCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteEmployeeCommandHandler>>();
            _handler = new DeleteEmployeeCommandHandler(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidEmployeeId_ShouldDeleteEmployee()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { EmployeeId = 1 };
            var employee = new Employee { Id = 1, FullName = "John Doe", EmployeeCode = "EMP001" };

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(1))
                .ReturnsAsync(employee);
            _mockUnitOfWork.Setup(x => x.EmployeeRepository.DeleteAsync(employee))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(x => x.EmployeeRepository.DeleteAsync(employee), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidEmployeeId_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { EmployeeId = 999 };
            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(999))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
