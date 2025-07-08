using Moq;
using TodoApp.Application.DTOs;
using TodoApp.Application.Services;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using Xunit;

namespace TodoApp.Tests.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockRepository;
        private readonly TodoService _todoService;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITodoRepository>();
            _todoService = new TodoService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test 1", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TodoItem { Id = 2, Title = "Test 2", IsCompleted = true, CreatedAt = DateTime.UtcNow }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(todos);

            // Act
            var result = await _todoService.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value!.Count());
        }

        [Fact]
        public async Task GetByIdAsync_WhenTodoExists_ShouldReturnTodo()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Title = "Test Todo", IsCompleted = false, CreatedAt = DateTime.UtcNow };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);

            // Act
            var result = await _todoService.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Test Todo", result.Value!.Title);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTodoDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TodoItem?)null);

            // Act
            var result = await _todoService.GetByIdAsync(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Todo item not found", result.Error);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldCreateTodo()
        {
            // Arrange
            var createDto = new CreateTodoItemDto { Title = "New Todo", Description = "Description" };
            var createdTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "New Todo", 
                Description = "Description", 
                IsCompleted = false, 
                CreatedAt = DateTime.UtcNow 
            };
            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<TodoItem>())).ReturnsAsync(createdTodo);

            // Act
            var result = await _todoService.CreateAsync(createDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("New Todo", result.Value!.Title);
            Assert.Equal("Description", result.Value.Description);
            Assert.False(result.Value.IsCompleted);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CreateAsync_WithInvalidTitle_ShouldReturnFailure(string title)
        {
            // Arrange
            var createDto = new CreateTodoItemDto { Title = title };

            // Act
            var result = await _todoService.CreateAsync(createDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Title is required", result.Error);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldUpdateTodo()
        {
            // Arrange
            var existingTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Old Title", 
                IsCompleted = false, 
                CreatedAt = DateTime.UtcNow 
            };
            var updateDto = new UpdateTodoItemDto 
            { 
                Title = "Updated Title", 
                Description = "Updated Description", 
                IsCompleted = true 
            };
            var updatedTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Updated Title", 
                Description = "Updated Description", 
                IsCompleted = true, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingTodo);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TodoItem>())).ReturnsAsync(updatedTodo);

            // Act
            var result = await _todoService.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Updated Title", result.Value!.Title);
            Assert.Equal("Updated Description", result.Value.Description);
            Assert.True(result.Value.IsCompleted);
        }

        [Fact]
        public async Task UpdateAsync_WhenTodoDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var updateDto = new UpdateTodoItemDto { Title = "Updated Title", IsCompleted = true };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TodoItem?)null);

            // Act
            var result = await _todoService.UpdateAsync(1, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Todo item not found", result.Error);
        }

        [Fact]
        public async Task DeleteAsync_WhenTodoExists_ShouldDeleteTodo()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
            _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _todoService.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task DeleteAsync_WhenTodoDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            _mockRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _todoService.DeleteAsync(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Todo item not found", result.Error);
        }
    }
}