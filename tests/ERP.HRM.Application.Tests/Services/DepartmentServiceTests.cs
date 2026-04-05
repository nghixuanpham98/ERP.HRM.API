using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<DepartmentService>> _mockLogger;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<DepartmentService>>();
            _service = new DepartmentService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnPagedDepartments()
        {
            // Arrange
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
            var result = await _service.GetAllDepartmentsAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_WithValidId_ShouldReturnDepartment()
        {
            // Arrange
            var departmentId = 1;
            var department = new Department { Id = departmentId, DepartmentName = "IT" };
            var departmentDto = new DepartmentDto { DepartmentId = departmentId, DepartmentName = "IT" };

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetByIdAsync(departmentId))
                .ReturnsAsync(department);
            _mockMapper.Setup(x => x.Map<DepartmentDto>(department))
                .Returns(departmentDto);

            // Act
            var result = await _service.GetDepartmentByIdAsync(departmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(departmentId, result.DepartmentId);
            Assert.Equal("IT", result.DepartmentName);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var departmentId = 999;
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetDepartmentByIdAsync(departmentId));
        }

        [Fact]
        public async Task AddDepartmentAsync_WithValidData_ShouldCreateDepartment()
        {
            // Arrange
            var createDto = new CreateDepartmentDto { DepartmentName = "NewDept" };
            var department = new Department { Id = 1, DepartmentName = "NewDept" };
            var departmentDto = new DepartmentDto { DepartmentId = 1, DepartmentName = "NewDept" };

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.ExistsByNameAsync("NewDept"))
                .ReturnsAsync(false);
            _mockMapper.Setup(x => x.Map<Department>(createDto))
                .Returns(department);
            _mockMapper.Setup(x => x.Map<DepartmentDto>(department))
                .Returns(departmentDto);
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.AddAsync(department))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddDepartmentAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NewDept", result.DepartmentName);
            _mockUnitOfWork.Verify(x => x.DepartmentRepository.AddAsync(It.IsAny<Department>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddDepartmentAsync_WithDuplicateName_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var createDto = new CreateDepartmentDto { DepartmentName = "ExistingDept" };
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.ExistsByNameAsync("ExistingDept"))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() => _service.AddDepartmentAsync(createDto));
        }

        [Fact]
        public async Task UpdateDepartmentAsync_WithValidData_ShouldUpdateDepartment()
        {
            // Arrange
            var updateDto = new UpdateDepartmentDto { DepartmentId = 1, DepartmentName = "UpdatedDept" };
            var department = new Department { Id = 1, DepartmentName = "OldDept" };
            var departmentDto = new DepartmentDto { DepartmentId = 1, DepartmentName = "UpdatedDept" };

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetByIdAsync(1))
                .ReturnsAsync(department);
            _mockMapper.Setup(x => x.Map(updateDto, department));
            _mockMapper.Setup(x => x.Map<DepartmentDto>(department))
                .Returns(departmentDto);
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.UpdateAsync(department))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateDepartmentAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(x => x.DepartmentRepository.UpdateAsync(It.IsAny<Department>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_WithValidId_ShouldDeleteDepartment()
        {
            // Arrange
            var departmentId = 1;
            var department = new Department { Id = departmentId, DepartmentName = "IT" };

            _mockUnitOfWork.Setup(x => x.DepartmentRepository.GetByIdAsync(departmentId))
                .ReturnsAsync(department);
            _mockUnitOfWork.Setup(x => x.DepartmentRepository.DeleteAsync(department))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteDepartmentAsync(departmentId);

            // Assert
            _mockUnitOfWork.Verify(x => x.DepartmentRepository.DeleteAsync(It.IsAny<Department>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
