using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Commands;
using ERP.HRM.Application.Features.Positions.Handlers;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class CreatePositionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreatePositionCommandHandler>> _mockLogger;
        private readonly CreatePositionCommandHandler _handler;

        public CreatePositionCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreatePositionCommandHandler>>();
            _handler = new CreatePositionCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreatePosition()
        {
            // Arrange
            var command = new CreatePositionCommand
            {
                PositionCode = "POS001",
                PositionName = "Software Engineer",
                DepartmentId = 1
            };

            var position = new Position 
            { 
                PositionCode = command.PositionCode,
                PositionName = command.PositionName,
                DepartmentId = command.DepartmentId
            };

            var positionDto = new PositionDto
            {
                PositionId = 1,
                PositionCode = command.PositionCode,
                PositionName = command.PositionName
            };

            _mockUnitOfWork.Setup(x => x.PositionRepository.GetAllAsync())
                .ReturnsAsync(new List<Position>());
            _mockMapper.Setup(x => x.Map<Position>(command))
                .Returns(position);
            _mockUnitOfWork.Setup(x => x.PositionRepository.AddAsync(position))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<PositionDto>(It.IsAny<Position>()))
                .Returns(positionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.PositionCode, result.PositionCode);
            Assert.Equal(command.PositionName, result.PositionName);
            _mockUnitOfWork.Verify(x => x.PositionRepository.AddAsync(It.IsAny<Position>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDuplicatePositionCode_ShouldThrowConflictException()
        {
            // Arrange
            var command = new CreatePositionCommand
            {
                PositionCode = "POS001",
                PositionName = "Software Engineer"
            };

            var existingPosition = new Position { PositionCode = "POS001", PositionName = "Existing" };
            _mockUnitOfWork.Setup(x => x.PositionRepository.GetAllAsync())
                .ReturnsAsync(new List<Position> { existingPosition });

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenSaveChangesFails_ShouldThrow()
        {
            // Arrange
            var command = new CreatePositionCommand
            {
                PositionCode = "POS001",
                PositionName = "Software Engineer"
            };

            var position = new Position { PositionCode = command.PositionCode };
            _mockUnitOfWork.Setup(x => x.PositionRepository.GetAllAsync())
                .ReturnsAsync(new List<Position>());
            _mockMapper.Setup(x => x.Map<Position>(command))
                .Returns(position);
            _mockUnitOfWork.Setup(x => x.PositionRepository.AddAsync(position))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
