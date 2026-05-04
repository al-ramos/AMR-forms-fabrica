using MediatR;
using AMR.Forms.Fabrica.Domain.Entities;

namespace AMR.Forms.Fabrica.Application.Features.NotasFiscais.Queries;


public record GetNotasFiscaisQuery(int CdFilial, DateOnly? DtInicio, DateOnly? DtFim) 
    : IRequest<IEnumerable<NotaFiscal>>;
public record GetNotaFiscalItensQuery(int Numero, string Serie) : IRequest<IEnumerable<NotaFiscalDetalhe>>;
