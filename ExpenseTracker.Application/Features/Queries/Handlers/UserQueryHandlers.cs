using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByIdAsync(request.Id);
            return user == null ? null : mapper.Map<UserDto>(user);
        }
    }

    public class GetUserByEmailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetUserByEmailQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email);
            return user == null ? null : mapper.Map<UserDto>(user);
        }
    }

    public class GetUserByUsernameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetUserByUsernameQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Users.GetByUsernameAsync(request.Username);
            return user == null ? null : mapper.Map<UserDto>(user);
        }
    }

    public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await unitOfWork.Users.GetAllAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
