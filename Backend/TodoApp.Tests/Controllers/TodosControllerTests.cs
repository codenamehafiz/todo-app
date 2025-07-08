using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.API.Controllers;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Core.Common;
using Xunit;

namespace TodoApp.Tests.Controllers
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _mockService;
        private readonly TodosController _controller;

        public TodosControllerTests()
        {
            _mockService = new Mock<ITodoService>();
            _controller = new TodosController(_mockService.Object);
        }

        [Fact]
        public async Task GetTodos_ShouldReturnOkWithTodos()
        {
            // Arrange
            var todos = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Title = "Test 1", IsCompleted = false, CreatedAt = DateTime.UtcNow },
                new TodoItemDto { Id = 2, Title = "Test 2", IsCompleted = true, CreatedAt = DateTime.UtcNow }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(Result<IEnumerable<TodoItemDto>>.Success(todos));

            // Act
            var result = await _controller.GetTodos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTodos = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(okResult.Value);
            Assert.Equal(2, returnedTodos.Count());
        }

        [Fact]
        public async Task GetTodo_WhenTodoExists_ShouldReturnOkWithTodo()
        {
            // Arrange
            var todo = new TodoItemDto { Id = 1, Title = "Test Todo", IsCompleted = false, CreatedAt = DateTime.UtcNow };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(Result<TodoItemDto>.Success(todo));

            // Act
            var result = await _controller.GetTodo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTodo = Assert.IsType<TodoItemDto>(okResult.Value);
            Assert.Equal("Test Todo", returnedTodo.Title);
        }

        [Fact]
        public async Task GetTodo_WhenTodoDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(Result<TodoItemDto>.Failure("Todo item not found"));

            // Act
            var result = await _controller.GetTodo(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Todo item not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateTodo_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var createDto = new CreateTodoItemDto { Title = "New Todo", Description = "Description" };
            var createdTodo = new TodoItemDto 
            { 
                Id = 1, 
                Title = "New Todo", 
                Description = "Description", 
                IsCompleted = false, 
                CreatedAt = DateTime.UtcNow 
            };
            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(Result<TodoItemDto>.Success(createdTodo));

            // Act
            var result = await _controller.CreateTodo(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTodo = Assert.IsType<TodoItemDto>(createdResult.Value);
            Assert.Equal("New Todo", returnedTodo.Title);
        }

        [Fact]
        public async Task CreateTodo_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var createDto = new CreateTodoItemDto { Title = "" };
            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(Result<TodoItemDto>.Failure("Title is required"));

            // Act
            var result = await _controller.CreateTodo(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Title is required", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateTodo_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var updateDto = new UpdateTodoItemDto { Title = "Updated Title", IsCompleted = true };
            var updatedTodo = new TodoItemDto 
            { 
                Id = 1, 
                Title = "Updated Title", 
                IsCompleted = true, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(Result<TodoItemDto>.Success(updatedTodo));

            // Act
            var result = await _controller.UpdateTodo(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTodo = Assert.IsType<TodoItemDto>(okResult.Value);
            Assert.Equal("Updated Title", returnedTodo.Title);
        }

        [Fact]
        public async Task DeleteTodo_WhenTodoExists_ShouldReturnNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _controller.DeleteTodo(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodo_WhenTodoDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(Result<bool>.Failure("Todo item not found"));

            // Act
            var result = await _controller.DeleteTodo(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Todo item not found", notFoundResult.Value);
        }
    }
}