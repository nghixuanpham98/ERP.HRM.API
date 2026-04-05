using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Features.Departments.Handlers;
using ERP.HRM.Application.Features.Departments.Queries;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class GetAllDepartmentsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetAllDepartmentsQueryHandler>> _mockLogger;
        private readonly GetAllDepartmentsQueryHandler _handler;

        public GetAllDepartmentsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetAllDepartmentsQueryHandler>>();
            _handler = new GetAllDepartmentsQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldReturnPagedDepartments()
        {
            // Arrange
            var query = new GetAllDepartmentsQuery { PageNumber = 1, PageSize = 10 };
            var departments = new List<Department>
            {
                new Department { Id = 1, DepartmentName = "IT" },
                new Department { Id = 2, DepartmentName = "HR" }
            };
            var dtos = new List<DepartmentDto>
            {
                new DepartmentDto { DepartmentId = 1, DepartmentName = "IT" },
                new DepartmentDto { DepartmentId = 2, DepartmentName = "HR" }
            };

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetPagedAsync(1, 10))
                .ReturnsAsync((departments, 2));
            _mockMapper.Setup(x => x.Map<IEnumerable<DepartmentDto>>(departments))
                .Returns(dtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            _mockUnitOfWork.Verify(x => x.DepartmentRepository.GetPagedAsync(1, 10), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDifferentPageSize_ShouldReturnCorrectPageSize()
        {
            // Arrange
            var query = new GetAllDepartmentsQuery { PageNumber = 2, PageSize = 5 };
            var departments = new List<Department>();
            var dtos = new List<DepartmentDto>();

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetPagedAsync(2, 5))
                .ReturnsAsync((departments, 0));
            _mockMapper.Setup(x => x.Map<IEnumerable<DepartmentDto>>(departments))
                .Returns(dtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(5, result.PageSize);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ShouldThrow()
        {
            // Arrange
            var query = new GetAllDepartmentsQuery { PageNumber = 1, PageSize = 10 };
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetPagedAsync(1, 10))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
