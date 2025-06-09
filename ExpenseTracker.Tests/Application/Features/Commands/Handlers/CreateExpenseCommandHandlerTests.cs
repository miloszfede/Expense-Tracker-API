using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Commands;
using ExpenseTracker.Application.Features.Commands.Handlers;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTracker.Tests.Application.Features.Commands.Handlers
{
    public class CreateExpenseCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<CreateExpenseDto>> _validatorMock;
        private readonly Mock<ILogger<CreateExpenseCommandHandler>> _loggerMock;
        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly CreateExpenseCommandHandler _handler;

        public CreateExpenseCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<CreateExpenseDto>>();
            _loggerMock = new Mock<ILogger<CreateExpenseCommandHandler>>();
            _expenseRepositoryMock = new Mock<IExpenseRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            
            _unitOfWorkMock.Setup(x => x.Expenses).Returns(_expenseRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.Categories).Returns(_categoryRepositoryMock.Object);
            
            _handler = new CreateExpenseCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateExpenseAndReturnDto()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Note = "Test expense",
                CategoryId = 1,
                UserId = 1
            };

            var expense = new Expense
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                UserId = command.UserId
            };
            expense.SetId(1);

            var validationResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var categoryType = typeof(Category);
            var category = (Category)Activator.CreateInstance(categoryType)!;
            var nameProperty = categoryType.GetProperty("Name")!;
            nameProperty.SetValue(category, "Food & Dining");
            category.SetId(command.CategoryId);

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryId))
                .ReturnsAsync(category);

            var expectedDto = new ExpenseDto
            {
                Id = 1,
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                CategoryName = "Food & Dining", 
                UserId = command.UserId
            };

            _mapperMock.Setup(x => x.Map<Expense>(It.IsAny<CreateExpenseDto>()))
                .Returns(expense);
            _mapperMock.Setup(x => x.Map<ExpenseDto>(expense))
                .Returns(expectedDto);

            _expenseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Expense>()))
                .ReturnsAsync(expense);
            
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedDto);
            result.Id.Should().Be(1);
            result.Amount.Should().Be(command.Amount);
            result.Note.Should().Be(command.Note);
            result.CategoryId.Should().Be(command.CategoryId);
            result.UserId.Should().Be(command.UserId);

            _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(command.CategoryId), Times.Once);
            _mapperMock.Verify(x => x.Map<Expense>(It.IsAny<CreateExpenseDto>()), Times.Once);
            _expenseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = -100.50m, 
                Date = DateTime.UtcNow,
                Note = "",
                CategoryId = 0, 
                UserId = 1
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Amount", "Amount must be positive"));
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("CategoryId", "Category ID is required"));

            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));

            _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _expenseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void MapToCreateDto_ShouldMapCommandPropertiesCorrectly()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 250.75m,
                Date = new DateTime(2023, 6, 15),
                Note = "Business lunch",
                CategoryId = 3,
                UserId = 5
            };

            var mapToCreateDtoMethod = typeof(CreateExpenseCommandHandler)
                .GetMethod("MapToCreateDto", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (CreateExpenseDto)mapToCreateDtoMethod!.Invoke(_handler, new object[] { command })!;

            // Assert
            result.Should().NotBeNull();
            result.Amount.Should().Be(command.Amount);
            result.Date.Should().Be(command.Date);
            result.Note.Should().Be(command.Note);
            result.CategoryId.Should().Be(command.CategoryId);
            result.UserId.Should().Be(command.UserId);
        }

        [Fact]
        public async Task Handle_WithValidCommand_WhenCategoryFound_ShouldPopulateCategoryName()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Note = "Test expense with category",
                CategoryId = 1,
                UserId = 1
            };

            var categoryType = typeof(Category);
            var categoryConstructor = categoryType.GetConstructors()[0];
            var category = (Category)Activator.CreateInstance(categoryType)!;
            var nameProperty = categoryType.GetProperty("Name")!;
            nameProperty.SetValue(category, "Food & Dining");
            
            category.SetId(1);

            var expense = new Expense
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                UserId = command.UserId
            };
            expense.SetId(1);

            var expectedDto = new ExpenseDto
            {
                Id = 1,
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                CategoryName = "Food & Dining", 
                UserId = command.UserId
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryId))
                .ReturnsAsync(category);

            _mapperMock.Setup(x => x.Map<Expense>(It.IsAny<CreateExpenseDto>()))
                .Returns(expense);
            _mapperMock.Setup(x => x.Map<ExpenseDto>(expense))
                .Returns(expectedDto);

            _expenseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Expense>()))
                .ReturnsAsync(expense);
            
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            _unitOfWorkMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CategoryName.Should().Be("Food & Dining");
            
            _expenseRepositoryMock.Verify(x => x.AddAsync(It.Is<Expense>(e => 
                e.CategoryName == "Food & Dining" && 
                e.CategoryId == command.CategoryId)), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentCategory_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateExpenseCommand
            {
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Note = "Test expense",
                CategoryId = 999, 
                UserId = 1
            };

            var expense = new Expense
            {
                Amount = command.Amount,
                Date = command.Date,
                Note = command.Note,
                CategoryId = command.CategoryId,
                UserId = command.UserId
            };
            expense.SetId(1);

            var validationResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(command.CategoryId))
                .ReturnsAsync((Category?)null);

            _mapperMock.Setup(x => x.Map<Expense>(It.IsAny<CreateExpenseDto>()))
                .Returns(expense);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
                () => _handler.Handle(command, CancellationToken.None));

            exception.Message.Should().Contain("Category with ID 999 does not exist");
            _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<CreateExpenseDto>(), It.IsAny<CancellationToken>()), Times.Once);
            _categoryRepositoryMock.Verify(x => x.GetByIdAsync(command.CategoryId), Times.Once);
            _expenseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Expense>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
