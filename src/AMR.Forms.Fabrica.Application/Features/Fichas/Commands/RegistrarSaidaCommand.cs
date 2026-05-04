using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Commands;

public record RegistrarSaidaCommand(int FichaId) : IRequest;
