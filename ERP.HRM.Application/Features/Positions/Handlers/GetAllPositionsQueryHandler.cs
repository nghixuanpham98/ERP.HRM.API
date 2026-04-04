using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Features.Positions.Queries;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Positions.Handlers
{
    /// <summary>
    /// Handler for getting all positions with pagination and filtering
    /// </summary>
    public class GetAllPositionsQueryHandler : IRequestHandler<GetAllPositionsQuery, PagedResult<PositionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPositionsQueryHandler> _logger;

        public GetAllPositionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllPositionsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<PositionDto>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting all positions. Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}", 
                    request.PageNumber, request.PageSize, request.SearchTerm);

                var (positions, totalCount) = await _unitOfWork.PositionRepository.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize);

                // Apply client-side filtering for search, status, and level
                var filteredPositions = positions.AsEnumerable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    filteredPositions = filteredPositions.Where(p =>
                        p.PositionName.Contains(request.SearchTerm) ||
                        p.PositionCode.Contains(request.SearchTerm) ||
                        (p.Description != null && p.Description.Contains(request.SearchTerm)));
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    filteredPositions = filteredPositions.Where(p => p.Status == request.Status);
                }

                if (request.Level.HasValue)
                {
                    filteredPositions = filteredPositions.Where(p => p.Level == request.Level);
                }

                var filteredList = filteredPositions.ToList();
                var filteredCount = filteredList.Count;

                return new PagedResult<PositionDto>
                {
                    Items = _mapper.Map<IEnumerable<PositionDto>>(filteredList),
                    TotalCount = filteredCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting positions");
                throw;
            }
        }
    }
}
