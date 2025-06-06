using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetByIdQueryHandler<GetUserByIdQuery, UserDto, User>(unitOfWork, mapper)
    {
        protected override int GetEntityId(GetUserByIdQuery query) => query.Id;
        
        protected override async Task<User?> GetEntityByIdAsync(int id) => 
            await UnitOfWork.Users.GetByIdAsync(id);
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
        : BaseGetCollectionQueryHandler<GetAllUsersQuery, UserDto, User>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<User>> GetEntitiesAsync(GetAllUsersQuery query) =>
            await UnitOfWork.Users.GetAllAsync();
    }
}
