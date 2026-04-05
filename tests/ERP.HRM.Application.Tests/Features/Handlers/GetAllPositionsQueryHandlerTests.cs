using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Handlers;
using ERP.HRM.Application.Features.Positions.Queries;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class GetAllPositionsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetAllPositionsQueryHandler>> _mockLogger;
        private readonly GetAllPositionsQueryHandler _handler;

        public GetAllPositionsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllPositionsQueryHandler>>();
            _handler = new GetAllPositionsQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldReturnPagedPositions()
        {
            // Arrange
            var query = new GetAllPositionsQuery { PageNumber = 1, PageSize = 10 };
            var positions = new List<Position>
            {
                new Position { Id = 1, PositionCode = "POS001", PositionName = "Software Engineer" },
                new Position { Id = 2, PositionCode = "POS002", PositionName = "Manager" }
            };
            var dtos = new List<PositionDto>
            {
                new PositionDto { PositionId = 1, PositionCode = "POS001", PositionName = "Software Engineer" },
                new PositionDto { PositionId = 2, PositionCode = "POS002", PositionName = "Manager" }
            };

            _mockUnitOfWork.Setup(x => x.PositionRepository.GetPagedAsync(1, 10))
                .ReturnsAsync((positions, 2));
            _mockMapper.Setup(x => x.Map<IEnumerable<PositionDto>>(positions))
                .Returns(dtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyPagedResult()
        {
            // Arrange
            var query = new GetAllPositionsQuery { PageNumber = 1, PageSize = 10 };
            var positions = new List<Position>();
            var dtos = new List<PositionDto>();

            _mockUnitOfWork.Setup(x => x.PositionRepository.GetPagedAsync(1, 10))
                .ReturnsAsync((positions, 0));
            _mockMapper.Setup(x => x.Map<IEnumerable<PositionDto>>(positions))
                .Returns(dtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }
    }
}
