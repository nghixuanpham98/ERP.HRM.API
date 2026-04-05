using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Handlers;
using ERP.HRM.Application.Features.Positions.Queries;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class GetPositionByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetPositionByIdQueryHandler>> _mockLogger;
        private readonly GetPositionByIdQueryHandler _handler;

        public GetPositionByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetPositionByIdQueryHandler>>();
            _handler = new GetPositionByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnPosition()
        {
            // Arrange
            var query = new GetPositionByIdQuery { PositionId = 1 };
            var position = new Position 
            { 
                Id = 1, 
                PositionCode = "POS001",
                PositionName = "Software Engineer" 
            };
            var positionDto = new PositionDto 
            { 
                PositionId = 1,
                PositionCode = "POS001",
                PositionName = "Software Engineer"
            };

            _mockUnitOfWork.Setup(x => x.PositionRepository.GetByIdAsync(1))
                .ReturnsAsync(position);
            _mockMapper.Setup(x => x.Map<PositionDto>(position))
                .Returns(positionDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PositionId);
            Assert.Equal("POS001", result.PositionCode);
            Assert.Equal("Software Engineer", result.PositionName);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var query = new GetPositionByIdQuery { PositionId = 999 };
            _mockUnitOfWork.Setup(x => x.PositionRepository.GetByIdAsync(999))
                .ReturnsAsync((Position)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
