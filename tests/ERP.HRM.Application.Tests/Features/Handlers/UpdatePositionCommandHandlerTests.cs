using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Commands;
using ERP.HRM.Application.Features.Positions.Handlers;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class UpdatePositionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdatePositionCommandHandler>> _mockLogger;
        private readonly UpdatePositionCommandHandler _handler;

        public UpdatePositionCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdatePositionCommandHandler>>();
            _handler = new UpdatePositionCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdatePosition()
        {
            // Arrange
            var command = new UpdatePositionCommand
            {
                PositionId = 1,
                PositionCode = "POS001",
                PositionName = "Senior Software Engineer",
                DepartmentId = 1
            };

            var existingPosition = new Position 
            { 
                Id = 1,
                PositionCode = "POS001",
                PositionName = "Software Engineer"
            };

            var updatedPositionDto = new PositionDto
            {
                PositionId = 1,
                PositionCode = "POS001",
                PositionName = "Senior Software Engineer"
            };

            _mockUnitOfWork.Setup(x => x.PositionRepository.GetByIdAsync(1))
                .ReturnsAsync(existingPosition);
            _mockMapper.Setup(x => x.Map(command, existingPosition));
            _mockUnitOfWork.Setup(x => x.PositionRepository.UpdateAsync(existingPosition))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map<PositionDto>(existingPosition))
                .Returns(updatedPositionDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Senior Software Engineer", result.PositionName);
            _mockUnitOfWork.Verify(x => x.PositionRepository.UpdateAsync(It.IsAny<Position>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
