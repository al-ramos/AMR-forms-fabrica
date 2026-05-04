using MediatR;

namespace AMR.Forms.Fabrica.Application.Features.Fichas.Commands;

public record AvancarPassoCommand(int FichaId, int ProximoPasso) : IRequest;
