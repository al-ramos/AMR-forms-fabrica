using MediatR;
using RDS.Forms.Fabrica.Domain.Entities;

namespace RDS.Forms.Fabrica.Application.Features.NotasFiscais.Queries;


public record GetNotasFiscaisQuery(int CdFilial, DateOnly? DtInicio, DateOnly? DtFim) 
    : IRequest<IEnumerable<NotaFiscal>>;
public record GetNotaFiscalItensQuery(int Numero, string Serie) : IRequest<IEnumerable<NotaFiscalDetalhe>>;
