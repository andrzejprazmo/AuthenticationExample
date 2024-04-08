
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Account.Create;
using WebApp.Core.Commands.Account.Delete;
using WebApp.Core.Common.Response;
using WebApp.Core.Queries.GetAllAccounts;

namespace WebApp.Api.Endpoints;

public class AccountEndpoints : IEndpointsModule
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/create", handler: Create).WithName("Create");
        app.MapGet("/api/account/list", handler: List).WithName("List");
        app.MapDelete("/api/account/delete/{id}", handler: Delete).WithName("Delete");
    }

    [ProducesResponseType(200, Type = typeof(IEnumerable<AccountDto>))]
    private async Task<IResult> List(HttpContext context, [FromServices] IMediator mediator)
    {
        return Results.Ok(await mediator.Send(new GetAllAccountsRequest()));
    }

    [ProducesResponseType(200, Type = typeof(string))]
    private async Task<IResult> Create(HttpContext context, [FromServices] IMediator mediator, [FromBody] CreateAccountRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match((data) =>
        {
            return Results.Ok(data);
        }, (errors) => Results.BadRequest(errors));
    }

    [ProducesResponseType(200)]
    private async Task<IResult> Delete(HttpContext context, [FromServices] IMediator mediator, [FromRoute] int id)
    {
        var result = await mediator.Send(new DeleteAccountRequest(id));
        return result.Match((data) =>
        {
            return Results.Ok(data);
        }, (errors) => Results.BadRequest(errors));
    }
}
