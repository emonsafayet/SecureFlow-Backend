using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureFlow.Application.Common.Interfaces;
using SecureFlow.Application.Users;
using SecureFlow.Shared.Models;

namespace SecureFlow.Application.Auth.Users.Queries;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, PaginationResponse<UserDto>>
{
    private readonly IAppDbContext _db;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(
        IAppDbContext db,
        IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        // Build base query
        var query = _db.Users.AsNoTracking();

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination if requested
        if (filter.HasPaging)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            query = query.Skip(skip).Take(filter.PageSize);
        }

        // Execute query and map to DTOs
        var users = await query
            .OrderBy(u => u.Email)
            .ToListAsync(cancellationToken);

        var userDtos = _mapper.Map<List<UserDto>>(users);

        // Return pagination response
        if (!filter.HasPaging)
        {
            return new PaginationResponse<UserDto>(
                userDtos,
                totalCount,
                pageNumber: 1,
                pageSize: totalCount);
        }

        return new PaginationResponse<UserDto>(
            userDtos,
            totalCount,
            filter.PageNumber,
            filter.PageSize);
    }
}
