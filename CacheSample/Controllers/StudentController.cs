using CacheSample.Api.ControllersConfig;
using CacheSample.Application.Commands.Students;
using CacheSample.Application.Queries;
using CacheSample.Application.Queries.Students;
using Microsoft.AspNetCore.Mvc;

namespace CacheSample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ApiBaseController<StudentController>
{
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentCommand createCommand)
    {
        Logger.LogInformation("Add new student");

        var result = await Mediator.Send(createCommand);

        return ApiResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] UpdateStudentCommand updateStudent)
    {
        Logger.LogInformation($"Update student with ID = {id}");

        updateStudent.Id = id;

        var result = await Mediator.Send(updateStudent);

        return ApiResult(result);
    }




    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Logger.LogInformation($"Try to get student ID: {id}");

        var query = new GetStudentByIdQuery() { Id = id };

        var result = await Mediator.Send(query);

        return ApiResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllStudentsQuery query, CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Try to get all students");

        var result = await Mediator.Send(query, cancellationToken);

        return ApiResult(result);
    }
}
