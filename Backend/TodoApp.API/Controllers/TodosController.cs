using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns>A list of todo items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodos()
        {
            var result = await _todoService.GetAllAsync();
            return Ok(result.Value);
        }

        /// <summary>
        /// Get a specific todo item by ID
        /// </summary>
        /// <param name="id">The ID of the todo item</param>
        /// <returns>The todo item</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDto>> GetTodo(int id)
        {
            var result = await _todoService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        /// <param name="createDto">The todo item to create</param>
        /// <returns>The created todo item</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDto>> CreateTodo(CreateTodoItemDto createDto)
        {
            var result = await _todoService.CreateAsync(createDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetTodo), new { id = result.Value!.Id }, result.Value);
        }

        /// <summary>
        /// Update an existing todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to update</param>
        /// <param name="updateDto">The updated todo item data</param>
        /// <returns>The updated todo item</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDto>> UpdateTodo(int id, UpdateTodoItemDto updateDto)
        {
            var result = await _todoService.UpdateAsync(id, updateDto);
            if (!result.IsSuccess)
            {
                if (result.Error == "Todo item not found")
                    return NotFound(result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        /// <param name="id">The ID of the todo item to delete</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var result = await _todoService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return NoContent();
        }
    }
}