using MediatR;

namespace RDS.Forms.Fabrica.Application.Features.Fichas.Commands;

public record RegistrarSaidaCommand(int FichaId) : IRequest;
