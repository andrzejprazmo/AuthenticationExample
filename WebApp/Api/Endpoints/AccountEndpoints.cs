
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Core.Commands.Account.Create;
using WebApp.Core.Commands.Account.Delete;
using WebApp.Core.Commands.Account.Password;
using WebApp.Core.Commands.Account.Update;
using WebApp.Core.Common.Response;
using WebApp.Core.Queries.Account.Edit;
using WebApp.Core.Queries.Account.GetAll;


namespace WebApp.Api.Endpoints;

public class AccountEndpoints : IEndpointsModule
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/create", handler: Create).RequireAuthorization().WithName("Create");
        app.MapGet("/api/account/edit/{id}", handler: Edit).RequireAuthorization().WithName("Edit");
        app.MapPut("/api/account/update", handler: Update).RequireAuthorization().WithName("Update");
        app.MapGet("/api/account/list", handler: List).RequireAuthorization().WithName("List");
        app.MapDelete("/api/account/delete/{id}", handler: Delete).RequireAuthorization().WithName("Delete");
        app.MapPut("/api/account/password", handler: Password).RequireAuthorization().WithName("Password");
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

    [ProducesResponseType(200, Type = typeof(AccountDto))]
    private async Task<IResult> Edit(HttpContext context, [FromServices] IMediator mediator, [FromRoute] int id)
    {
        var result = await mediator.Send(new EditAccountRequest(id));
        return Results.Ok(result);
    }

    [ProducesResponseType(200)]
    private async Task<IResult> Update(HttpContext context, [FromServices] IMediator mediator, [FromBody] UpdateAccountRequest request)
    {
        var result = await mediator.Send(request);
        return result.Match((data) =>
        {
            return Results.Ok(data);
        }, (errors) => Results.BadRequest(errors));
    }

    [ProducesResponseType(200)]
    private async Task<IResult> Password(HttpContext context, [FromServices] IMediator mediator, [FromBody] SetPasswordRequest request)
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
